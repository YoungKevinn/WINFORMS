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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMaNV = new System.Windows.Forms.Label();
            this.txtMaNhanVien = new System.Windows.Forms.TextBox();
            this.btnBatDauCa = new System.Windows.Forms.Button();
            this.btnQuayLai = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(10, 25, 74);
            this.lblTitle.Location = new System.Drawing.Point(40, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(317, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Nhập mã nhân viên để bắt đầu ca";
            // 
            // lblMaNV
            // 
            this.lblMaNV.AutoSize = true;
            this.lblMaNV.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMaNV.Location = new System.Drawing.Point(42, 90);
            this.lblMaNV.Name = "lblMaNV";
            this.lblMaNV.Size = new System.Drawing.Size(91, 19);
            this.lblMaNV.TabIndex = 1;
            this.lblMaNV.Text = "Mã nhân viên";
            // 
            // txtMaNhanVien
            // 
            this.txtMaNhanVien.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMaNhanVien.Location = new System.Drawing.Point(46, 118);
            this.txtMaNhanVien.Name = "txtMaNhanVien";
            this.txtMaNhanVien.Size = new System.Drawing.Size(320, 27);
            this.txtMaNhanVien.TabIndex = 2;
            // 
            // btnBatDauCa
            // 
            this.btnBatDauCa.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnBatDauCa.Location = new System.Drawing.Point(46, 170);
            this.btnBatDauCa.Name = "btnBatDauCa";
            this.btnBatDauCa.Size = new System.Drawing.Size(150, 40);
            this.btnBatDauCa.TabIndex = 3;
            this.btnBatDauCa.Text = "Bắt đầu ca";
            this.btnBatDauCa.UseVisualStyleBackColor = true;
            this.btnBatDauCa.Click += new System.EventHandler(this.btnBatDauCa_Click);
            // 
            // btnQuayLai
            // 
            this.btnQuayLai.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnQuayLai.Location = new System.Drawing.Point(216, 170);
            this.btnQuayLai.Name = "btnQuayLai";
            this.btnQuayLai.Size = new System.Drawing.Size(150, 40);
            this.btnQuayLai.TabIndex = 4;
            this.btnQuayLai.Text = "Quay lại";
            this.btnQuayLai.UseVisualStyleBackColor = true;
            this.btnQuayLai.Click += new System.EventHandler(this.btnQuayLai_Click);
            // 
            // FormNhanVienDangNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(414, 251);
            this.Controls.Add(this.btnQuayLai);
            this.Controls.Add(this.btnBatDauCa);
            this.Controls.Add(this.txtMaNhanVien);
            this.Controls.Add(this.lblMaNV);
            this.Controls.Add(this.lblTitle);
            this.MaximizeBox = false;
            this.Name = "FormNhanVienDangNhap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập nhân viên";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMaNV;
        private System.Windows.Forms.TextBox txtMaNhanVien;
        private System.Windows.Forms.Button btnBatDauCa;
        private System.Windows.Forms.Button btnQuayLai;
    }
}