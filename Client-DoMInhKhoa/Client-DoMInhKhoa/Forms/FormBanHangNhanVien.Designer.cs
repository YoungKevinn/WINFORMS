namespace Client_DoMInhKhoa.Forms
{
    partial class FormBanHangNhanVien
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelTop;
        private Label lblTitle;
        private Label lblXinChao;
        private Button btnDangXuat;

        private SplitContainer splitMain;

        private Label lblDanhSachBan;
        private FlowLayoutPanel flpBan;

        private Label lblBanHienTai;
        private Panel panelOrderTop;
        private Label lblDanhMuc;
        private ComboBox cboDanhMuc;
        private Label lblMon;
        private ComboBox cboMon;
        private Label lblSoLuong;
        private NumericUpDown nudSoLuong;
        private Button btnThemMon;
        private Button btnXoaMon;

        private DataGridView dgvChiTiet;

        private Panel panelBottom;
        private Label lblBanFrom;
        private ComboBox cboBanFrom;
        private Label lblBanTo;
        private ComboBox cboBanTo;
        private Button btnChuyenBan;
        private Button btnGopBan;
        private Label lblGiamGia;
        private NumericUpDown nudGiamGia;
        private Label lblTongTienText;
        private Label lblTongTien;
        private Button btnThanhToan;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTop = new Panel();
            btnDangXuat = new Button();
            lblXinChao = new Label();
            lblTitle = new Label();
            splitMain = new SplitContainer();
            flpBan = new FlowLayoutPanel();
            lblDanhSachBan = new Label();
            dgvChiTiet = new DataGridView();
            panelBottom = new Panel();
            btnThanhToan = new Button();
            lblTongTien = new Label();
            lblTongTienText = new Label();
            nudGiamGia = new NumericUpDown();
            lblGiamGia = new Label();
            btnGopBan = new Button();
            btnChuyenBan = new Button();
            cboBanTo = new ComboBox();
            lblBanTo = new Label();
            cboBanFrom = new ComboBox();
            lblBanFrom = new Label();
            panelOrderTop = new Panel();
            btnXoaMon = new Button();
            btnThemMon = new Button();
            nudSoLuong = new NumericUpDown();
            lblSoLuong = new Label();
            cboMon = new ComboBox();
            lblMon = new Label();
            cboDanhMuc = new ComboBox();
            lblDanhMuc = new Label();
            lblBanHienTai = new Label();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudGiamGia).BeginInit();
            panelOrderTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.RoyalBlue;
            panelTop.Controls.Add(btnDangXuat);
            panelTop.Controls.Add(lblXinChao);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(3, 4, 3, 4);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1353, 67);
            panelTop.TabIndex = 0;
            // 
            // btnDangXuat
            // 
            btnDangXuat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDangXuat.BackColor = Color.White;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Location = new Point(1229, 13);
            btnDangXuat.Margin = new Padding(3, 4, 3, 4);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(103, 40);
            btnDangXuat.TabIndex = 2;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = false;
            btnDangXuat.Click += btnDangXuat_Click;
            // 
            // lblXinChao
            // 
            lblXinChao.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblXinChao.AutoSize = true;
            lblXinChao.ForeColor = Color.White;
            lblXinChao.Location = new Point(1086, 24);
            lblXinChao.Name = "lblXinChao";
            lblXinChao.Size = new Size(136, 20);
            lblXinChao.TabIndex = 1;
            lblXinChao.Text = "Xin chào, nhân viên";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(14, 19);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(261, 28);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Cafe Manager - Nhân viên";
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 67);
            splitMain.Margin = new Padding(3, 4, 3, 4);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(flpBan);
            splitMain.Panel1.Controls.Add(lblDanhSachBan);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.BackColor = SystemColors.Info;
            splitMain.Panel2.Controls.Add(dgvChiTiet);
            splitMain.Panel2.Controls.Add(panelBottom);
            splitMain.Panel2.Controls.Add(panelOrderTop);
            splitMain.Panel2.Controls.Add(lblBanHienTai);
            splitMain.Size = new Size(1353, 814);
            splitMain.SplitterDistance = 365;
            splitMain.SplitterWidth = 5;
            splitMain.TabIndex = 1;
            // 
            // flpBan
            // 
            flpBan.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flpBan.Location = new Point(14, 47);
            flpBan.Margin = new Padding(3, 4, 3, 4);
            flpBan.Name = "flpBan";
            flpBan.Size = new Size(337, 751);
            flpBan.TabIndex = 1;
            // 
            // lblDanhSachBan
            // 
            lblDanhSachBan.AutoSize = true;
            lblDanhSachBan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDanhSachBan.Location = new Point(14, 13);
            lblDanhSachBan.Name = "lblDanhSachBan";
            lblDanhSachBan.Size = new Size(126, 23);
            lblDanhSachBan.TabIndex = 0;
            lblDanhSachBan.Text = "Danh sách bàn";
            // 
            // dgvChiTiet
            // 
            dgvChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTiet.Location = new Point(14, 148);
            dgvChiTiet.Margin = new Padding(3, 4, 3, 4);
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.RowHeadersWidth = 51;
            dgvChiTiet.RowTemplate.Height = 25;
            dgvChiTiet.Size = new Size(960, 506);
            dgvChiTiet.TabIndex = 2;
            // 
            // panelBottom
            // 
            panelBottom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelBottom.BorderStyle = BorderStyle.FixedSingle;
            panelBottom.Controls.Add(btnThanhToan);
            panelBottom.Controls.Add(lblTongTien);
            panelBottom.Controls.Add(lblTongTienText);
            panelBottom.Controls.Add(nudGiamGia);
            panelBottom.Controls.Add(lblGiamGia);
            panelBottom.Controls.Add(btnGopBan);
            panelBottom.Controls.Add(btnChuyenBan);
            panelBottom.Controls.Add(cboBanTo);
            panelBottom.Controls.Add(lblBanTo);
            panelBottom.Controls.Add(cboBanFrom);
            panelBottom.Controls.Add(lblBanFrom);
            panelBottom.Location = new Point(14, 662);
            panelBottom.Margin = new Padding(3, 4, 3, 4);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(960, 133);
            panelBottom.TabIndex = 3;
            // 
            // btnThanhToan
            // 
            btnThanhToan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnThanhToan.BackColor = Color.FromArgb(0, 120, 215);
            btnThanhToan.ForeColor = Color.White;
            btnThanhToan.Location = new Point(800, 47);
            btnThanhToan.Margin = new Padding(3, 4, 3, 4);
            btnThanhToan.Name = "btnThanhToan";
            btnThanhToan.Size = new Size(137, 53);
            btnThanhToan.TabIndex = 10;
            btnThanhToan.Text = "Thanh toán";
            btnThanhToan.UseVisualStyleBackColor = false;
            // 
            // lblTongTien
            // 
            lblTongTien.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTongTien.ForeColor = Color.DarkBlue;
            lblTongTien.Location = new Point(681, 13);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(43, 28);
            lblTongTien.TabIndex = 9;
            lblTongTien.Text = "0 đ";
            // 
            // lblTongTienText
            // 
            lblTongTienText.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTongTienText.AutoSize = true;
            lblTongTienText.Location = new Point(606, 19);
            lblTongTienText.Name = "lblTongTienText";
            lblTongTienText.Size = new Size(75, 20);
            lblTongTienText.TabIndex = 8;
            lblTongTienText.Text = "Tổng tiền:";
            // 
            // nudGiamGia
            // 
            nudGiamGia.Location = new Point(410, 15);
            nudGiamGia.Margin = new Padding(3, 4, 3, 4);
            nudGiamGia.Name = "nudGiamGia";
            nudGiamGia.Size = new Size(69, 27);
            nudGiamGia.TabIndex = 7;
            // 
            // lblGiamGia
            // 
            lblGiamGia.AutoSize = true;
            lblGiamGia.Location = new Point(331, 19);
            lblGiamGia.Name = "lblGiamGia";
            lblGiamGia.Size = new Size(85, 20);
            lblGiamGia.TabIndex = 6;
            lblGiamGia.Text = "Giảm giá %";
            // 
            // btnGopBan
            // 
            btnGopBan.Location = new Point(194, 52);
            btnGopBan.Margin = new Padding(3, 4, 3, 4);
            btnGopBan.Name = "btnGopBan";
            btnGopBan.Size = new Size(103, 31);
            btnGopBan.TabIndex = 5;
            btnGopBan.Text = "Gộp bàn";
            btnGopBan.UseVisualStyleBackColor = true;
            // 
            // btnChuyenBan
            // 
            btnChuyenBan.Location = new Point(194, 13);
            btnChuyenBan.Margin = new Padding(3, 4, 3, 4);
            btnChuyenBan.Name = "btnChuyenBan";
            btnChuyenBan.Size = new Size(103, 31);
            btnChuyenBan.TabIndex = 4;
            btnChuyenBan.Text = "Chuyển bàn";
            btnChuyenBan.UseVisualStyleBackColor = true;
            // 
            // cboBanTo
            // 
            cboBanTo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBanTo.Location = new Point(93, 53);
            cboBanTo.Margin = new Padding(3, 4, 3, 4);
            cboBanTo.Name = "cboBanTo";
            cboBanTo.Size = new Size(91, 28);
            cboBanTo.TabIndex = 3;
            // 
            // lblBanTo
            // 
            lblBanTo.AutoSize = true;
            lblBanTo.Location = new Point(11, 57);
            lblBanTo.Name = "lblBanTo";
            lblBanTo.Size = new Size(79, 20);
            lblBanTo.TabIndex = 2;
            lblBanTo.Text = "Chuyển tới";
            // 
            // cboBanFrom
            // 
            cboBanFrom.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBanFrom.Location = new Point(93, 15);
            cboBanFrom.Margin = new Padding(3, 4, 3, 4);
            cboBanFrom.Name = "cboBanFrom";
            cboBanFrom.Size = new Size(91, 28);
            cboBanFrom.TabIndex = 1;
            // 
            // lblBanFrom
            // 
            lblBanFrom.AutoSize = true;
            lblBanFrom.Location = new Point(11, 19);
            lblBanFrom.Name = "lblBanFrom";
            lblBanFrom.Size = new Size(87, 20);
            lblBanFrom.TabIndex = 0;
            lblBanFrom.Text = "Bàn hiện tại";
            // 
            // panelOrderTop
            // 
            panelOrderTop.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelOrderTop.BorderStyle = BorderStyle.FixedSingle;
            panelOrderTop.Controls.Add(btnXoaMon);
            panelOrderTop.Controls.Add(btnThemMon);
            panelOrderTop.Controls.Add(nudSoLuong);
            panelOrderTop.Controls.Add(lblSoLuong);
            panelOrderTop.Controls.Add(cboMon);
            panelOrderTop.Controls.Add(lblMon);
            panelOrderTop.Controls.Add(cboDanhMuc);
            panelOrderTop.Controls.Add(lblDanhMuc);
            panelOrderTop.Location = new Point(14, 47);
            panelOrderTop.Margin = new Padding(3, 4, 3, 4);
            panelOrderTop.Name = "panelOrderTop";
            panelOrderTop.Size = new Size(960, 93);
            panelOrderTop.TabIndex = 1;
            // 
            // btnXoaMon
            // 
            btnXoaMon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXoaMon.BackColor = Color.Firebrick;
            btnXoaMon.ForeColor = Color.White;
            btnXoaMon.Location = new Point(853, 8);
            btnXoaMon.Margin = new Padding(3, 4, 3, 4);
            btnXoaMon.Name = "btnXoaMon";
            btnXoaMon.Size = new Size(91, 36);
            btnXoaMon.TabIndex = 7;
            btnXoaMon.Text = "Xóa món";
            btnXoaMon.UseVisualStyleBackColor = false;
            // 
            // btnThemMon
            // 
            btnThemMon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnThemMon.BackColor = Color.FromArgb(0, 120, 215);
            btnThemMon.ForeColor = Color.White;
            btnThemMon.Location = new Point(754, 8);
            btnThemMon.Margin = new Padding(3, 4, 3, 4);
            btnThemMon.Name = "btnThemMon";
            btnThemMon.Size = new Size(91, 36);
            btnThemMon.TabIndex = 6;
            btnThemMon.Text = "Thêm món";
            btnThemMon.UseVisualStyleBackColor = false;
            // 
            // nudSoLuong
            // 
            nudSoLuong.Location = new Point(666, 11);
            nudSoLuong.Margin = new Padding(3, 4, 3, 4);
            nudSoLuong.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSoLuong.Name = "nudSoLuong";
            nudSoLuong.Size = new Size(69, 27);
            nudSoLuong.TabIndex = 5;
            nudSoLuong.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblSoLuong
            // 
            lblSoLuong.AutoSize = true;
            lblSoLuong.Location = new Point(594, 15);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new Size(72, 20);
            lblSoLuong.TabIndex = 4;
            lblSoLuong.Text = "Số lượng:";
            // 
            // cboMon
            // 
            cboMon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMon.Location = new Point(345, 11);
            cboMon.Margin = new Padding(3, 4, 3, 4);
            cboMon.Name = "cboMon";
            cboMon.Size = new Size(228, 28);
            cboMon.TabIndex = 3;
            // 
            // lblMon
            // 
            lblMon.AutoSize = true;
            lblMon.Location = new Point(297, 15);
            lblMon.Name = "lblMon";
            lblMon.Size = new Size(42, 20);
            lblMon.TabIndex = 2;
            lblMon.Text = "Món:";
            // 
            // cboDanhMuc
            // 
            cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDanhMuc.Location = new Point(91, 11);
            cboDanhMuc.Margin = new Padding(3, 4, 3, 4);
            cboDanhMuc.Name = "cboDanhMuc";
            cboDanhMuc.Size = new Size(182, 28);
            cboDanhMuc.TabIndex = 1;
            // 
            // lblDanhMuc
            // 
            lblDanhMuc.AutoSize = true;
            lblDanhMuc.Location = new Point(11, 15);
            lblDanhMuc.Name = "lblDanhMuc";
            lblDanhMuc.Size = new Size(79, 20);
            lblDanhMuc.TabIndex = 0;
            lblDanhMuc.Text = "Danh mục:";
            // 
            // lblBanHienTai
            // 
            lblBanHienTai.AutoSize = true;
            lblBanHienTai.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblBanHienTai.Location = new Point(14, 13);
            lblBanHienTai.Name = "lblBanHienTai";
            lblBanHienTai.Size = new Size(110, 23);
            lblBanHienTai.TabIndex = 0;
            lblBanHienTai.Text = "Bàn hiện tại:";
            // 
            // FormBanHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1353, 881);
            Controls.Add(splitMain);
            Controls.Add(panelTop);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormBanHang";
            Text = "Bán hàng";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel1.PerformLayout();
            splitMain.Panel2.ResumeLayout(false);
            splitMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudGiamGia).EndInit();
            panelOrderTop.ResumeLayout(false);
            panelOrderTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).EndInit();
            ResumeLayout(false);
        }
    }
}