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
            panelTop = new Panel();
            btnDoiMatKhau = new Button();
            btnDangXuat = new Button();
            lblXinChao = new Label();
            lblTitle = new Label();
            panelMenu = new Panel();
            btnAuditLog = new Button();
            btnBaoCaoDoanhThu = new Button();
            btnHoaDon = new Button();
            btnQuanLyDanhMuc = new Button();
            btnQuanLyMon = new Button();
            btnQuanLyBan = new Button();
            btnQuanLyNhanVien = new Button();
            panelMenuHeader = new Panel();
            lblMenu = new Label();
            panelContent = new Panel();
            panelTop.SuspendLayout();
            panelMenu.SuspendLayout();
            panelMenuHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(10, 25, 74);
            panelTop.Controls.Add(btnDoiMatKhau);
            panelTop.Controls.Add(btnDangXuat);
            panelTop.Controls.Add(lblXinChao);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(3, 4, 3, 4);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1375, 67);
            panelTop.TabIndex = 0;
            // 
            // btnDoiMatKhau
            // 
            btnDoiMatKhau.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDoiMatKhau.BackColor = Color.White;
            btnDoiMatKhau.FlatAppearance.BorderSize = 0;
            btnDoiMatKhau.FlatStyle = FlatStyle.Flat;
            btnDoiMatKhau.Font = new Font("Segoe UI", 9F);
            btnDoiMatKhau.ForeColor = Color.FromArgb(10, 25, 74);
            btnDoiMatKhau.Location = new Point(1124, 16);
            btnDoiMatKhau.Margin = new Padding(3, 4, 3, 4);
            btnDoiMatKhau.Name = "btnDoiMatKhau";
            btnDoiMatKhau.Size = new Size(114, 35);
            btnDoiMatKhau.TabIndex = 3;
            btnDoiMatKhau.Text = "Đổi mật khẩu";
            btnDoiMatKhau.UseVisualStyleBackColor = false;
            btnDoiMatKhau.Click += btnDoiMatKhau_Click;
            // 
            // btnDangXuat
            // 
            btnDangXuat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDangXuat.BackColor = Color.FromArgb(24, 119, 242);
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDangXuat.ForeColor = Color.White;
            btnDangXuat.Location = new Point(1249, 16);
            btnDangXuat.Margin = new Padding(3, 4, 3, 4);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(103, 35);
            btnDangXuat.TabIndex = 2;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = false;
            btnDangXuat.Click += btnDangXuat_Click;
            // 
            // lblXinChao
            // 
            lblXinChao.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblXinChao.AutoSize = true;
            lblXinChao.Font = new Font("Segoe UI", 9F);
            lblXinChao.ForeColor = Color.Gainsboro;
            lblXinChao.Location = new Point(872, 24);
            lblXinChao.Name = "lblXinChao";
            lblXinChao.Size = new Size(117, 20);
            lblXinChao.TabIndex = 1;
            lblXinChao.Text = "Xin chào, Admin";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(23, 19);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(285, 28);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Cafe Manager - Trang quản trị";
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(13, 45, 104);
            panelMenu.Controls.Add(btnAuditLog);
            panelMenu.Controls.Add(btnBaoCaoDoanhThu);
            panelMenu.Controls.Add(btnHoaDon);
            panelMenu.Controls.Add(btnQuanLyDanhMuc);
            panelMenu.Controls.Add(btnQuanLyMon);
            panelMenu.Controls.Add(btnQuanLyBan);
            panelMenu.Controls.Add(btnQuanLyNhanVien);
            panelMenu.Controls.Add(panelMenuHeader);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 67);
            panelMenu.Margin = new Padding(3, 4, 3, 4);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(240, 706);
            panelMenu.TabIndex = 1;
            // 
            // btnAuditLog
            // 
            btnAuditLog.Dock = DockStyle.Top;
            btnAuditLog.FlatAppearance.BorderSize = 0;
            btnAuditLog.FlatStyle = FlatStyle.Flat;
            btnAuditLog.Font = new Font("Segoe UI", 10F);
            btnAuditLog.ForeColor = Color.White;
            btnAuditLog.Location = new Point(0, 398);
            btnAuditLog.Margin = new Padding(3, 4, 3, 4);
            btnAuditLog.Name = "btnAuditLog";
            btnAuditLog.Padding = new Padding(23, 0, 0, 0);
            btnAuditLog.Size = new Size(240, 53);
            btnAuditLog.TabIndex = 7;
            btnAuditLog.Text = "Nhật ký hệ thống";
            btnAuditLog.TextAlign = ContentAlignment.MiddleLeft;
            btnAuditLog.UseVisualStyleBackColor = true;
            btnAuditLog.Click += btnAuditLog_Click;
            // 
            // btnBaoCaoDoanhThu
            // 
            btnBaoCaoDoanhThu.Dock = DockStyle.Top;
            btnBaoCaoDoanhThu.FlatAppearance.BorderSize = 0;
            btnBaoCaoDoanhThu.FlatStyle = FlatStyle.Flat;
            btnBaoCaoDoanhThu.Font = new Font("Segoe UI", 10F);
            btnBaoCaoDoanhThu.ForeColor = Color.White;
            btnBaoCaoDoanhThu.Location = new Point(0, 345);
            btnBaoCaoDoanhThu.Margin = new Padding(3, 4, 3, 4);
            btnBaoCaoDoanhThu.Name = "btnBaoCaoDoanhThu";
            btnBaoCaoDoanhThu.Padding = new Padding(23, 0, 0, 0);
            btnBaoCaoDoanhThu.Size = new Size(240, 53);
            btnBaoCaoDoanhThu.TabIndex = 6;
            btnBaoCaoDoanhThu.Text = "Báo cáo doanh thu";
            btnBaoCaoDoanhThu.TextAlign = ContentAlignment.MiddleLeft;
            btnBaoCaoDoanhThu.UseVisualStyleBackColor = true;
            btnBaoCaoDoanhThu.Click += btnBaoCaoDoanhThu_Click;
            // 
            // btnHoaDon
            // 
            btnHoaDon.Dock = DockStyle.Top;
            btnHoaDon.FlatAppearance.BorderSize = 0;
            btnHoaDon.FlatStyle = FlatStyle.Flat;
            btnHoaDon.Font = new Font("Segoe UI", 10F);
            btnHoaDon.ForeColor = Color.White;
            btnHoaDon.Location = new Point(0, 292);
            btnHoaDon.Margin = new Padding(3, 4, 3, 4);
            btnHoaDon.Name = "btnHoaDon";
            btnHoaDon.Padding = new Padding(23, 0, 0, 0);
            btnHoaDon.Size = new Size(240, 53);
            btnHoaDon.TabIndex = 5;
            btnHoaDon.Text = "Hóa đơn / tra cứu";
            btnHoaDon.TextAlign = ContentAlignment.MiddleLeft;
            btnHoaDon.UseVisualStyleBackColor = true;
            btnHoaDon.Click += btnHoaDon_Click;
            // 
            // btnQuanLyDanhMuc
            // 
            btnQuanLyDanhMuc.Dock = DockStyle.Top;
            btnQuanLyDanhMuc.FlatAppearance.BorderSize = 0;
            btnQuanLyDanhMuc.FlatStyle = FlatStyle.Flat;
            btnQuanLyDanhMuc.Font = new Font("Segoe UI", 10F);
            btnQuanLyDanhMuc.ForeColor = Color.White;
            btnQuanLyDanhMuc.Location = new Point(0, 239);
            btnQuanLyDanhMuc.Margin = new Padding(3, 4, 3, 4);
            btnQuanLyDanhMuc.Name = "btnQuanLyDanhMuc";
            btnQuanLyDanhMuc.Padding = new Padding(23, 0, 0, 0);
            btnQuanLyDanhMuc.Size = new Size(240, 53);
            btnQuanLyDanhMuc.TabIndex = 4;
            btnQuanLyDanhMuc.Text = "Quản lý danh mục";
            btnQuanLyDanhMuc.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyDanhMuc.UseVisualStyleBackColor = true;
            btnQuanLyDanhMuc.Click += btnQuanLyDanhMuc_Click;
            // 
            // btnQuanLyMon
            // 
            btnQuanLyMon.Dock = DockStyle.Top;
            btnQuanLyMon.FlatAppearance.BorderSize = 0;
            btnQuanLyMon.FlatStyle = FlatStyle.Flat;
            btnQuanLyMon.Font = new Font("Segoe UI", 10F);
            btnQuanLyMon.ForeColor = Color.White;
            btnQuanLyMon.Location = new Point(0, 186);
            btnQuanLyMon.Margin = new Padding(3, 4, 3, 4);
            btnQuanLyMon.Name = "btnQuanLyMon";
            btnQuanLyMon.Padding = new Padding(23, 0, 0, 0);
            btnQuanLyMon.Size = new Size(240, 53);
            btnQuanLyMon.TabIndex = 3;
            btnQuanLyMon.Text = "Quản lý món ăn/đồ uống";
            btnQuanLyMon.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyMon.UseVisualStyleBackColor = true;
            btnQuanLyMon.Click += btnQuanLyMon_Click;
            // 
            // btnQuanLyBan
            // 
            btnQuanLyBan.Dock = DockStyle.Top;
            btnQuanLyBan.FlatAppearance.BorderSize = 0;
            btnQuanLyBan.FlatStyle = FlatStyle.Flat;
            btnQuanLyBan.Font = new Font("Segoe UI", 10F);
            btnQuanLyBan.ForeColor = Color.White;
            btnQuanLyBan.Location = new Point(0, 133);
            btnQuanLyBan.Margin = new Padding(3, 4, 3, 4);
            btnQuanLyBan.Name = "btnQuanLyBan";
            btnQuanLyBan.Padding = new Padding(23, 0, 0, 0);
            btnQuanLyBan.Size = new Size(240, 53);
            btnQuanLyBan.TabIndex = 2;
            btnQuanLyBan.Text = "Quản lý bàn";
            btnQuanLyBan.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyBan.UseVisualStyleBackColor = true;
            btnQuanLyBan.Click += btnQuanLyBan_Click;
            // 
            // btnQuanLyNhanVien
            // 
            btnQuanLyNhanVien.Dock = DockStyle.Top;
            btnQuanLyNhanVien.FlatAppearance.BorderSize = 0;
            btnQuanLyNhanVien.FlatStyle = FlatStyle.Flat;
            btnQuanLyNhanVien.Font = new Font("Segoe UI", 10F);
            btnQuanLyNhanVien.ForeColor = Color.White;
            btnQuanLyNhanVien.Location = new Point(0, 80);
            btnQuanLyNhanVien.Margin = new Padding(3, 4, 3, 4);
            btnQuanLyNhanVien.Name = "btnQuanLyNhanVien";
            btnQuanLyNhanVien.Padding = new Padding(23, 0, 0, 0);
            btnQuanLyNhanVien.Size = new Size(240, 53);
            btnQuanLyNhanVien.TabIndex = 1;
            btnQuanLyNhanVien.Text = "Quản lý nhân viên";
            btnQuanLyNhanVien.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyNhanVien.UseVisualStyleBackColor = true;
            btnQuanLyNhanVien.Click += btnQuanLyNhanVien_Click;
            // 
            // panelMenuHeader
            // 
            panelMenuHeader.BackColor = Color.FromArgb(10, 35, 90);
            panelMenuHeader.Controls.Add(lblMenu);
            panelMenuHeader.Dock = DockStyle.Top;
            panelMenuHeader.Location = new Point(0, 0);
            panelMenuHeader.Margin = new Padding(3, 4, 3, 4);
            panelMenuHeader.Name = "panelMenuHeader";
            panelMenuHeader.Size = new Size(240, 80);
            panelMenuHeader.TabIndex = 0;
            // 
            // lblMenu
            // 
            lblMenu.AutoSize = true;
            lblMenu.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblMenu.ForeColor = Color.White;
            lblMenu.Location = new Point(23, 27);
            lblMenu.Name = "lblMenu";
            lblMenu.Size = new Size(150, 25);
            lblMenu.TabIndex = 0;
            lblMenu.Text = "Bảng điều khiển";
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.White;
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(240, 67);
            panelContent.Margin = new Padding(3, 4, 3, 4);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(1135, 706);
            panelContent.TabIndex = 2;
            // 
            // FormTrangChuAdmin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1375, 773);
            Controls.Add(panelContent);
            Controls.Add(panelMenu);
            Controls.Add(panelTop);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormTrangChuAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Trang quản trị - Cafe Manager";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelMenu.ResumeLayout(false);
            panelMenuHeader.ResumeLayout(false);
            panelMenuHeader.PerformLayout();
            ResumeLayout(false);

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
