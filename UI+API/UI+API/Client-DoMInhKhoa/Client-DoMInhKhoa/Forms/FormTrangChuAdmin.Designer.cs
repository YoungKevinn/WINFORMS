namespace Client_DoMInhKhoa.Forms
{
    partial class FormTrangChuAdmin
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnDoiMatKhau = new System.Windows.Forms.Button();
            this.btnDangXuat = new System.Windows.Forms.Button();
            this.lblXinChao = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnAuditLog = new System.Windows.Forms.Button();
            this.btnBaoCaoDoanhThu = new System.Windows.Forms.Button();
            this.btnHoaDon = new System.Windows.Forms.Button();
            this.btnQuanLyDanhMuc = new System.Windows.Forms.Button();
            this.btnQuanLyMon = new System.Windows.Forms.Button();
            this.btnQuanLyBan = new System.Windows.Forms.Button();
            this.btnQuanLyNhanVien = new System.Windows.Forms.Button();
            this.panelMenuHeader = new System.Windows.Forms.Panel();
            this.lblMenu = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.panelMenuHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(10, 25, 74);
            this.panelTop.Controls.Add(this.btnDoiMatKhau);
            this.panelTop.Controls.Add(this.btnDangXuat);
            this.panelTop.Controls.Add(this.lblXinChao);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(980, 50);
            this.panelTop.TabIndex = 0;
            // 
            // btnDoiMatKhau
            // 
            this.btnDoiMatKhau.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDoiMatKhau.BackColor = System.Drawing.Color.White;
            this.btnDoiMatKhau.FlatAppearance.BorderSize = 0;
            this.btnDoiMatKhau.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDoiMatKhau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDoiMatKhau.ForeColor = System.Drawing.Color.FromArgb(10, 25, 74);
            this.btnDoiMatKhau.Location = new System.Drawing.Point(760, 12);
            this.btnDoiMatKhau.Name = "btnDoiMatKhau";
            this.btnDoiMatKhau.Size = new System.Drawing.Size(100, 26);
            this.btnDoiMatKhau.TabIndex = 3;
            this.btnDoiMatKhau.Text = "Đổi mật khẩu";
            this.btnDoiMatKhau.UseVisualStyleBackColor = false;
            this.btnDoiMatKhau.Click += new System.EventHandler(this.btnDoiMatKhau_Click);
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDangXuat.BackColor = System.Drawing.Color.FromArgb(24, 119, 242);
            this.btnDangXuat.FlatAppearance.BorderSize = 0;
            this.btnDangXuat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnDangXuat.ForeColor = System.Drawing.Color.White;
            this.btnDangXuat.Location = new System.Drawing.Point(870, 12);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new System.Drawing.Size(90, 26);
            this.btnDangXuat.TabIndex = 2;
            this.btnDangXuat.Text = "Đăng xuất";
            this.btnDangXuat.UseVisualStyleBackColor = false;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // lblXinChao
            // 
            this.lblXinChao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblXinChao.AutoSize = true;
            this.lblXinChao.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblXinChao.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblXinChao.Location = new System.Drawing.Point(540, 18);
            this.lblXinChao.Name = "lblXinChao";
            this.lblXinChao.Size = new System.Drawing.Size(96, 15);
            this.lblXinChao.TabIndex = 1;
            this.lblXinChao.Text = "Xin chào, Admin";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(217, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Cafe Manager - Trang quản trị";
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(13, 45, 104);
            this.panelMenu.Controls.Add(this.btnAuditLog);
            this.panelMenu.Controls.Add(this.btnBaoCaoDoanhThu);
            this.panelMenu.Controls.Add(this.btnHoaDon);
            this.panelMenu.Controls.Add(this.btnQuanLyDanhMuc);
            this.panelMenu.Controls.Add(this.btnQuanLyMon);
            this.panelMenu.Controls.Add(this.btnQuanLyBan);
            this.panelMenu.Controls.Add(this.btnQuanLyNhanVien);
            this.panelMenu.Controls.Add(this.panelMenuHeader);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 50);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(210, 530);
            this.panelMenu.TabIndex = 1;
            // 
            // btnAuditLog
            // 
            this.btnAuditLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAuditLog.FlatAppearance.BorderSize = 0;
            this.btnAuditLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAuditLog.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAuditLog.ForeColor = System.Drawing.Color.White;
            this.btnAuditLog.Location = new System.Drawing.Point(0, 260);
            this.btnAuditLog.Name = "btnAuditLog";
            this.btnAuditLog.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnAuditLog.Size = new System.Drawing.Size(210, 40);
            this.btnAuditLog.TabIndex = 7;
            this.btnAuditLog.Text = "Nhật ký hệ thống";
            this.btnAuditLog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAuditLog.UseVisualStyleBackColor = true;
            this.btnAuditLog.Click += new System.EventHandler(this.btnAuditLog_Click);
            // 
            // btnBaoCaoDoanhThu
            // 
            this.btnBaoCaoDoanhThu.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBaoCaoDoanhThu.FlatAppearance.BorderSize = 0;
            this.btnBaoCaoDoanhThu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBaoCaoDoanhThu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnBaoCaoDoanhThu.ForeColor = System.Drawing.Color.White;
            this.btnBaoCaoDoanhThu.Location = new System.Drawing.Point(0, 220);
            this.btnBaoCaoDoanhThu.Name = "btnBaoCaoDoanhThu";
            this.btnBaoCaoDoanhThu.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnBaoCaoDoanhThu.Size = new System.Drawing.Size(210, 40);
            this.btnBaoCaoDoanhThu.TabIndex = 6;
            this.btnBaoCaoDoanhThu.Text = "Báo cáo doanh thu";
            this.btnBaoCaoDoanhThu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBaoCaoDoanhThu.UseVisualStyleBackColor = true;
            this.btnBaoCaoDoanhThu.Click += new System.EventHandler(this.btnBaoCaoDoanhThu_Click);
            // 
            // btnHoaDon
            // 
            this.btnHoaDon.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHoaDon.FlatAppearance.BorderSize = 0;
            this.btnHoaDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHoaDon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnHoaDon.ForeColor = System.Drawing.Color.White;
            this.btnHoaDon.Location = new System.Drawing.Point(0, 180);
            this.btnHoaDon.Name = "btnHoaDon";
            this.btnHoaDon.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnHoaDon.Size = new System.Drawing.Size(210, 40);
            this.btnHoaDon.TabIndex = 5;
            this.btnHoaDon.Text = "Hóa đơn / tra cứu";
            this.btnHoaDon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHoaDon.UseVisualStyleBackColor = true;
            this.btnHoaDon.Click += new System.EventHandler(this.btnHoaDon_Click);
            // 
            // btnQuanLyDanhMuc
            // 
            this.btnQuanLyDanhMuc.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnQuanLyDanhMuc.FlatAppearance.BorderSize = 0;
            this.btnQuanLyDanhMuc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuanLyDanhMuc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnQuanLyDanhMuc.ForeColor = System.Drawing.Color.White;
            this.btnQuanLyDanhMuc.Location = new System.Drawing.Point(0, 140);
            this.btnQuanLyDanhMuc.Name = "btnQuanLyDanhMuc";
            this.btnQuanLyDanhMuc.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnQuanLyDanhMuc.Size = new System.Drawing.Size(210, 40);
            this.btnQuanLyDanhMuc.TabIndex = 4;
            this.btnQuanLyDanhMuc.Text = "Quản lý danh mục";
            this.btnQuanLyDanhMuc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQuanLyDanhMuc.UseVisualStyleBackColor = true;
            this.btnQuanLyDanhMuc.Click += new System.EventHandler(this.btnQuanLyDanhMuc_Click);
            // 
            // btnQuanLyMon
            // 
            this.btnQuanLyMon.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnQuanLyMon.FlatAppearance.BorderSize = 0;
            this.btnQuanLyMon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuanLyMon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnQuanLyMon.ForeColor = System.Drawing.Color.White;
            this.btnQuanLyMon.Location = new System.Drawing.Point(0, 100);
            this.btnQuanLyMon.Name = "btnQuanLyMon";
            this.btnQuanLyMon.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnQuanLyMon.Size = new System.Drawing.Size(210, 40);
            this.btnQuanLyMon.TabIndex = 3;
            this.btnQuanLyMon.Text = "Quản lý món ăn/đồ uống";
            this.btnQuanLyMon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQuanLyMon.UseVisualStyleBackColor = true;
            this.btnQuanLyMon.Click += new System.EventHandler(this.btnQuanLyMon_Click);
            // 
            // btnQuanLyBan
            // 
            this.btnQuanLyBan.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnQuanLyBan.FlatAppearance.BorderSize = 0;
            this.btnQuanLyBan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuanLyBan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnQuanLyBan.ForeColor = System.Drawing.Color.White;
            this.btnQuanLyBan.Location = new System.Drawing.Point(0, 60);
            this.btnQuanLyBan.Name = "btnQuanLyBan";
            this.btnQuanLyBan.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnQuanLyBan.Size = new System.Drawing.Size(210, 40);
            this.btnQuanLyBan.TabIndex = 2;
            this.btnQuanLyBan.Text = "Quản lý bàn";
            this.btnQuanLyBan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQuanLyBan.UseVisualStyleBackColor = true;
            this.btnQuanLyBan.Click += new System.EventHandler(this.btnQuanLyBan_Click);
            // 
            // btnQuanLyNhanVien
            // 
            this.btnQuanLyNhanVien.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnQuanLyNhanVien.FlatAppearance.BorderSize = 0;
            this.btnQuanLyNhanVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuanLyNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnQuanLyNhanVien.ForeColor = System.Drawing.Color.White;
            this.btnQuanLyNhanVien.Location = new System.Drawing.Point(0, 60);
            this.btnQuanLyNhanVien.Name = "btnQuanLyNhanVien";
            this.btnQuanLyNhanVien.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnQuanLyNhanVien.Size = new System.Drawing.Size(210, 40);
            this.btnQuanLyNhanVien.TabIndex = 1;
            this.btnQuanLyNhanVien.Text = "Quản lý nhân viên";
            this.btnQuanLyNhanVien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQuanLyNhanVien.UseVisualStyleBackColor = true;
            this.btnQuanLyNhanVien.Click += new System.EventHandler(this.btnQuanLyNhanVien_Click);
            // 
            // panelMenuHeader
            // 
            this.panelMenuHeader.BackColor = System.Drawing.Color.FromArgb(10, 35, 90);
            this.panelMenuHeader.Controls.Add(this.lblMenu);
            this.panelMenuHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMenuHeader.Location = new System.Drawing.Point(0, 0);
            this.panelMenuHeader.Name = "panelMenuHeader";
            this.panelMenuHeader.Size = new System.Drawing.Size(210, 60);
            this.panelMenuHeader.TabIndex = 0;
            // 
            // lblMenu
            // 
            this.lblMenu.AutoSize = true;
            this.lblMenu.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMenu.ForeColor = System.Drawing.Color.White;
            this.lblMenu.Location = new System.Drawing.Point(20, 20);
            this.lblMenu.Name = "lblMenu";
            this.lblMenu.Size = new System.Drawing.Size(121, 20);
            this.lblMenu.TabIndex = 0;
            this.lblMenu.Text = "Bảng điều khiển";
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(210, 50);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(770, 530);
            this.panelContent.TabIndex = 2;
            // 
            // FormTrangChuAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(980, 580);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = true;
            this.Name = "FormTrangChuAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trang quản trị - Cafe Manager";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.panelMenuHeader.ResumeLayout(false);
            this.panelMenuHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.Label lblXinChao;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button btnAuditLog;
        private System.Windows.Forms.Button btnBaoCaoDoanhThu;
        private System.Windows.Forms.Button btnHoaDon;
        private System.Windows.Forms.Button btnQuanLyDanhMuc;
        private System.Windows.Forms.Button btnQuanLyNhanVien;
        private System.Windows.Forms.Panel panelMenuHeader;
        private System.Windows.Forms.Label lblMenu;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Button btnDoiMatKhau;
        private System.Windows.Forms.Button btnQuanLyBan;
        private System.Windows.Forms.Button btnQuanLyMon;
    }
}
