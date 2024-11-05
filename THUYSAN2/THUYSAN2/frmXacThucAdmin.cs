using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;


namespace THUYSAN2
{
    public partial class frmXacThucAdmin : Form
    {
        private OracleConnect db; 

        public frmXacThucAdmin()
        {
            InitializeComponent();
            Authentication();
            db = new OracleConnect(); 
        }
        public string TaoNgauNhienText(int length = 5)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private RSAParameters GetPublicKeyFromPEMFile(string pemFilePath)
        {
            using (TextReader reader = new StreamReader(pemFilePath))
            {
                PemReader pemReader = new PemReader(reader);
                var publicKeyParam = (RsaKeyParameters)pemReader.ReadObject();
                return DotNetUtilities.ToRSAParameters(publicKeyParam);
            }
        }
        private RSAParameters GetPrivateKeyFromPEMFile(Stream pemStream)
        {
            using (TextReader reader = new StreamReader(pemStream))
            {
                PemReader pemReader = new PemReader(reader);
                var keyObject = pemReader.ReadObject();

                if (keyObject is AsymmetricCipherKeyPair keyPair)
                {
                    var privateKeyParam = (RsaPrivateCrtKeyParameters)keyPair.Private;
                    return DotNetUtilities.ToRSAParameters(privateKeyParam);
                }
                else if (keyObject is RsaPrivateCrtKeyParameters privateKeyParam)
                {
                    return DotNetUtilities.ToRSAParameters(privateKeyParam);
                }
                else
                {
                    throw new PemException("Unsupported key format.");
                }
            }
        }
        public string EncryptChallengeWithRSA(string plainText)
        {
            string publicPemFilePath = "E:\\hufi\\DoAnChuyenNganh\\Project\\DOANCHUYENNGANH\\Pem\\public.pem";
            var rsaParameters = GetPublicKeyFromPEMFile(publicPemFilePath);

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParameters);
                var encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
                return Convert.ToBase64String(encryptedData);
            }
        }
        private RSAParameters GetRSAParametersFromPEM(string pem)
        {
            using (TextReader reader = new StringReader(pem))
            {
                PemReader pemReader = new PemReader(reader);
                AsymmetricKeyParameter publicKeyParam = (AsymmetricKeyParameter)pemReader.ReadObject();
                RsaKeyParameters rsaKeyParams = (RsaKeyParameters)publicKeyParam;
                return DotNetUtilities.ToRSAParameters(rsaKeyParams);
            }
        }
        public string EncryptPrivateKeyWithAES(string username)
        {
            var aesKey = Environment.GetEnvironmentVariable("AES_KEY");
            OracleConnect db = new OracleConnect();

            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_username", username),
                new OracleParameter("p_privateKey", OracleDbType.Varchar2, ParameterDirection.Output)
            };

            db.ExecuteQuery("GetPrivateKey", parameters);
            string privateKey = parameters[1].Value.ToString();

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(aesKey);
                aes.GenerateIV();
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(privateKey);
                        }
                    }
                    var iv = aes.IV;
                    var encryptedContent = ms.ToArray();
                    var result = new byte[iv.Length + encryptedContent.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);
                    return Convert.ToBase64String(result);
                }
            }
        }
        public bool CompareEncryptedPrivateKey(string encryptedPrivateKey, string storedEncryptedPrivateKey)
        {
            return encryptedPrivateKey == storedEncryptedPrivateKey;
        }
        public string DecryptChallengeWithRSA(string encryptedText, Stream privateKeyStream)
        {
            var rsaParameters = GetPrivateKeyFromPEMFile(privateKeyStream);

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParameters);
                var decryptedData = rsa.Decrypt(Convert.FromBase64String(encryptedText), false);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        public bool VerifyDecryptedChallenge(string decryptedChallenge, string originalChallenge)
        {
            return decryptedChallenge == originalChallenge;
        }
        public void Authentication()
        {
            string randomText = TaoNgauNhienText();

            string encryptedText = EncryptChallengeWithRSA(randomText);

            textBox1.Text = encryptedText;

            TempData.OriginalChallenge = randomText;

            var model = new AuthenticationViewModel
            {
                EncryptedText = encryptedText,
            };
        }
        public static class TempData
        {
            public static string OriginalChallenge { get; set; }
        }
        public class AuthenticationViewModel
        {
            public string EncryptedText { get; set; }
            public string OriginalChallenge { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PEM files (*.pem)|*.pem|Key files (*.key)|*.key|All files (*.*)|*.*";
                openFileDialog.Title = "Tải lên file chứa khóa riêng";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string privateKeyPath = openFileDialog.FileName;
                    string privateKey = File.ReadAllText(privateKeyPath);
                    textBox2.Text = privateKey;
                }
            }
        }

        // Nút để giải mã
        private void button2_Click(object sender, EventArgs e)
        {
            string encryptedText = textBox1.Text;
            string privateKeyText = textBox2.Text;

            if (string.IsNullOrWhiteSpace(privateKeyText))
            {
                MessageBox.Show("Vui lòng nhập khóa riêng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Chuyển đổi nội dung khóa riêng thành Stream
                using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(privateKeyText)))
                {
                    // Gọi hàm giải mã thách thức
                    string decryptedChallenge = DecryptChallengeWithRSA(encryptedText, stream);

                    // Hiển thị kết quả giải mã trong TextBox
                    textBox3.Text = decryptedChallenge;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static class Session
        {
            public static int? FailedAttempts { get; set; }

            public static void Clear()
            {
                FailedAttempts = null;
            }
        }

        // Nút xác thực
        private void button3_Click(object sender, EventArgs e)
        {
            string decryptedText = textBox3.Text;
            string originalChallenge = TempData.OriginalChallenge; 

            // So sánh thông điệp đã giải mã với văn bản gốc
            if (VerifyDecryptedChallenge(decryptedText, originalChallenge))
            {
                MessageBox.Show("Xác thực thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                var mainForm = new Main();
                mainForm.Show();
                mainForm.ShowHome();
            }
            else
            {
                // Tăng số lần thử nghiệm không thành công
                int failedAttempts = (int)(Session.FailedAttempts ?? 0) + 1;
                Session.FailedAttempts = failedAttempts;

                // Kiểm tra nếu số lần thử nghiệm vượt quá 3
                if (failedAttempts >= 3)
                {
                    // Xóa phiên
                    Session.Clear();
                    MessageBox.Show("Bạn đã vượt quá số lần thử nghiệm. Ứng dụng sẽ đóng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("Xác thực không thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Hàm giải mã bằng AES
        private string Decrypt(string encryptedText, string secretKey)
        {
            byte[] fullCipher = Convert.FromBase64String(encryptedText);
            byte[] iv = new byte[16];
            byte[] cipherTextBytes = new byte[fullCipher.Length - iv.Length];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipherTextBytes, 0, cipherTextBytes.Length);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(secretKey);
                aes.IV = iv; // Sử dụng IV đã trích xuất

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd(); // Trả về văn bản đã giải mã
                            }
                        }
                    }
                }
            }
        }
        private void frmXacThucAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}
