namespace Client_DoMInhKhoa.Forms
{
    partial class FormNhanVienDangNhap
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelTaiKhoan = new System.Windows.Forms.Label();
            this.txtMaNhanVien = new System.Windows.Forms.TextBox();
            this.labelMatKhau = new System.Windows.Forms.Label();
            this.txtMatKhau = new System.Windows.Forms.TextBox();
            this.btnBatDauCa = new System.Windows.Forms.Button();
            this.btnQuayLai = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 80);
            this.panel1.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(18, 23);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(221, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Đăng nhập nhân viên";
            // 
            // labelTaiKhoan
            // 
            this.labelTaiKhoan.AutoSize = true;
            this.labelTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelTaiKhoan.Location = new System.Drawing.Point(53, 122);
            this.labelTaiKhoan.Name = "labelTaiKhoan";
            this.labelTaiKhoan.Size = new System.Drawing.Size(71, 19);
            this.labelTaiKhoan.TabIndex = 1;
            this.labelTaiKhoan.Text = "Tài khoản";
            // 
            // txtMaNhanVien
            // 
            this.txtMaNhanVien.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMaNhanVien.Location = new System.Drawing.Point(57, 145);
            this.txtMaNhanVien.Name = "txtMaNhanVien";
            this.txtMaNhanVien.Size = new System.Drawing.Size(470, 27);
            this.txtMaNhanVien.TabIndex = 2;
            // 
            // labelMatKhau
            // 
            this.labelMatKhau.AutoSize = true;
            this.labelMatKhau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelMatKhau.Location = new System.Drawing.Point(53, 190);
            this.labelMatKhau.Name = "labelMatKhau";
            this.labelMatKhau.Size = new System.Drawing.Size(66, 19);
            this.labelMatKhau.TabIndex = 3;
            this.labelMatKhau.Text = "Mật khẩu";
            // 
            // txtMatKhau
            // 
            this.txtMatKhau.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMatKhau.Location = new System.Drawing.Point(57, 213);
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.PasswordChar = '●';
            this.txtMatKhau.Size = new System.Drawing.Size(470, 27);
            this.txtMatKhau.TabIndex = 4;
            this.txtMatKhau.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtMatKhau_KeyDown);
            // 
            // btnBatDauCa
            // 
            this.btnBatDauCa.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnBatDauCa.Location = new System.Drawing.Point(57, 270);
            this.btnBatDauCa.Name = "btnBatDauCa";
            this.btnBatDauCa.Size = new System.Drawing.Size(470, 42);
            this.btnBatDauCa.TabIndex = 5;
            this.btnBatDauCa.Text = "Đăng nhập";
            this.btnBatDauCa.UseVisualStyleBackColor = true;
            this.btnBatDauCa.Click += new System.EventHandler(this.btnBatDauCa_Click);
            // 
            // btnQuayLai
            // 
            this.btnQuayLai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnQuayLai.Location = new System.Drawing.Point(57, 326);
            this.btnQuayLai.Name = "btnQuayLai";
            this.btnQuayLai.Size = new System.Drawing.Size(470, 36);
            this.btnQuayLai.TabIndex = 6;
            this.btnQuayLai.Text = "Quay lại";
            this.btnQuayLai.UseVisualStyleBackColor = true;
            this.btnQuayLai.Click += new System.EventHandler(this.btnQuayLai_Click);
            // 
            // FormNhanVienDangNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.btnQuayLai);
            this.Controls.Add(this.btnBatDauCa);
            this.Controls.Add(this.txtMatKhau);
            this.Controls.Add(this.labelMatKhau);
            this.Controls.Add(this.txtMaNhanVien);
            this.Controls.Add(this.labelTaiKhoan);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormNhanVienDangNhap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập nhân viên";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelTaiKhoan;
        private System.Windows.Forms.TextBox txtMaNhanVien;
        private System.Windows.Forms.Label labelMatKhau;
        private System.Windows.Forms.TextBox txtMatKhau;
        private System.Windows.Forms.Button btnBatDauCa;
        private System.Windows.Forms.Button btnQuayLai;
    }
}
