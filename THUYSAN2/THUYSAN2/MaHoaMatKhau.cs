using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace THUYSAN2
{
    internal class MaHoaMatKhau
    {
        public string EncryptPassword(string password)
        {
            // Khóa DES (8 bytes)
            byte[] key = Encoding.UTF8.GetBytes("1AQ#7T78"); // Đảm bảo rằng đây là khóa 8 byte
            byte[] iv = new byte[8]; // Tạo một IV (8 bytes) bằng 0 hoặc ngẫu nhiên

            using (var des = DES.Create())
            {
                des.Key = key;
                des.IV = iv; // Thiết lập IV
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7; // Padding PKCS7 tương đương với PKCS5

                // Mã hóa
                using (var encryptor = des.CreateEncryptor())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                    return BitConverter.ToString(encryptedBytes).Replace("-", "").ToUpper();
                }
            }
        }
    }
}
