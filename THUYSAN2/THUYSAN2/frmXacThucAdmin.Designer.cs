namespace THUYSAN2
{
    partial class frmXacThucAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btn_LoadFile = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.btn_GiaiMa = new System.Windows.Forms.Button();
            this.btn_XacThuc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Bauhaus 93", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(672, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "AUTHENTICATION";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label2.Location = new System.Drawing.Point(36, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Văn bản mã hóa:";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(169, 59);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(479, 20);
            this.textBox1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label3.Location = new System.Drawing.Point(36, 156);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Kết quả giải mã:";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(169, 104);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(479, 20);
            this.textBox2.TabIndex = 4;
            // 
            // btn_LoadFile
            // 
            this.btn_LoadFile.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_LoadFile.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LoadFile.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_LoadFile.Location = new System.Drawing.Point(39, 102);
            this.btn_LoadFile.Margin = new System.Windows.Forms.Padding(2);
            this.btn_LoadFile.Name = "btn_LoadFile";
            this.btn_LoadFile.Size = new System.Drawing.Size(106, 27);
            this.btn_LoadFile.TabIndex = 5;
            this.btn_LoadFile.Text = "File private key";
            this.btn_LoadFile.UseVisualStyleBackColor = false;
            this.btn_LoadFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(169, 154);
            this.textBox3.Margin = new System.Windows.Forms.Padding(2);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(479, 20);
            this.textBox3.TabIndex = 6;
            // 
            // btn_GiaiMa
            // 
            this.btn_GiaiMa.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_GiaiMa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_GiaiMa.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_GiaiMa.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_GiaiMa.Location = new System.Drawing.Point(198, 203);
            this.btn_GiaiMa.Margin = new System.Windows.Forms.Padding(2);
            this.btn_GiaiMa.Name = "btn_GiaiMa";
            this.btn_GiaiMa.Size = new System.Drawing.Size(106, 33);
            this.btn_GiaiMa.TabIndex = 7;
            this.btn_GiaiMa.Text = "Giải mã";
            this.btn_GiaiMa.UseVisualStyleBackColor = false;
            this.btn_GiaiMa.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_XacThuc
            // 
            this.btn_XacThuc.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_XacThuc.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_XacThuc.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_XacThuc.Location = new System.Drawing.Point(383, 203);
            this.btn_XacThuc.Margin = new System.Windows.Forms.Padding(2);
            this.btn_XacThuc.Name = "btn_XacThuc";
            this.btn_XacThuc.Size = new System.Drawing.Size(106, 33);
            this.btn_XacThuc.TabIndex = 8;
            this.btn_XacThuc.Text = "Xác thực";
            this.btn_XacThuc.UseVisualStyleBackColor = false;
            this.btn_XacThuc.Click += new System.EventHandler(this.button3_Click);
            // 
            // frmXacThucAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(694, 268);
            this.Controls.Add(this.btn_XacThuc);
            this.Controls.Add(this.btn_GiaiMa);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.btn_LoadFile);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmXacThucAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmXacThucAdmin";
            this.Load += new System.EventHandler(this.frmXacThucAdmin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btn_LoadFile;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button btn_GiaiMa;
        private System.Windows.Forms.Button btn_XacThuc;
    }
}