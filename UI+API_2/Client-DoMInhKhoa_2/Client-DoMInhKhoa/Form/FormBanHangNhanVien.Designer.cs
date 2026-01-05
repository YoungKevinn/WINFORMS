using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    partial class FormBanHangNhanVien
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel tlpRoot;

        private TableLayoutPanel tlpHeader;
        private Label lblXinChao;
        private Button btnDangXuat;

        private Panel pnlLeft;
        private Label lblLeftTitle;
        private FlowLayoutPanel flpBan;

        private Panel pnlMenu;
        private Panel pnlMenuTop;
        private Label lblDanhMuc;
        private ComboBox cboDanhMuc;
        private Label lblMon;
        private ComboBox cboMon;
        private Button btnThemMon;
        private Button btnXoaMon;
        private Label lblTimMon;
        private TextBox txtTimMon;
        private FlowLayoutPanel flpMonNhanh;

        private Panel pnlDon;
        private Label lblBanHienTai;
        private FlowLayoutPanel flpChiTiet;

        // ✅ actions: In tạm + Thanh toán
        private Panel pnlDonActions;
        private Button btnInTam;
        private Button btnThanhToanTungPhan;
        private Button btnThanhToan;

        private Panel pnlDonFooter;
        private Label lblCaptionTong;
        private Label lblTongTien;

        private Panel pnlBottom;
        private Label lblFrom;
        private ComboBox cboBanFrom;
        private Label lblTo;
        private ComboBox cboBanTo;
        private Button btnChuyenBan;
        private Button btnGopBan;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tlpRoot = new TableLayoutPanel();
            tlpHeader = new TableLayoutPanel();
            lblXinChao = new Label();
            btnDangXuat = new Button();
            pnlLeft = new Panel();
            flpBan = new FlowLayoutPanel();
            lblLeftTitle = new Label();
            pnlMenu = new Panel();
            flpMonNhanh = new FlowLayoutPanel();
            pnlMenuTop = new Panel();
            lblDanhMuc = new Label();
            cboDanhMuc = new ComboBox();
            lblMon = new Label();
            cboMon = new ComboBox();
            btnThemMon = new Button();
            btnXoaMon = new Button();
            lblTimMon = new Label();
            txtTimMon = new TextBox();
            pnlDon = new Panel();
            flpChiTiet = new FlowLayoutPanel();
            pnlDonActions = new Panel();
            pnlDonFooter = new Panel();
            lblTongTien = new Label();
            lblCaptionTong = new Label();
            lblBanHienTai = new Label();
            pnlBottom = new Panel();
            btnThanhToan = new Button();
            btnThanhToanTungPhan = new Button();
            btnInTam = new Button();
            lblFrom = new Label();
            cboBanFrom = new ComboBox();
            lblTo = new Label();
            cboBanTo = new ComboBox();
            btnChuyenBan = new Button();
            btnGopBan = new Button();
            tlpRoot.SuspendLayout();
            tlpHeader.SuspendLayout();
            pnlLeft.SuspendLayout();
            pnlMenu.SuspendLayout();
            pnlMenuTop.SuspendLayout();
            pnlDon.SuspendLayout();
            pnlDonFooter.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // tlpRoot
            // 
            tlpRoot.ColumnCount = 3;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260F));
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 576F));
            tlpRoot.Controls.Add(tlpHeader, 0, 0);
            tlpRoot.Controls.Add(pnlLeft, 0, 1);
            tlpRoot.Controls.Add(pnlMenu, 1, 1);
            tlpRoot.Controls.Add(pnlDon, 2, 1);
            tlpRoot.Controls.Add(pnlBottom, 0, 2);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(0, 0);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.RowCount = 3;
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            tlpRoot.Size = new Size(1503, 760);
            tlpRoot.TabIndex = 0;
            // 
            // tlpHeader
            // 
            tlpHeader.BackColor = Color.WhiteSmoke;
            tlpHeader.ColumnCount = 2;
            tlpRoot.SetColumnSpan(tlpHeader, 3);
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 149F));
            tlpHeader.Controls.Add(lblXinChao, 0, 0);
            tlpHeader.Controls.Add(btnDangXuat, 1, 0);
            tlpHeader.Dock = DockStyle.Fill;
            tlpHeader.Location = new Point(3, 3);
            tlpHeader.Name = "tlpHeader";
            tlpHeader.Padding = new Padding(10, 6, 10, 6);
            tlpHeader.RowCount = 1;
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpHeader.Size = new Size(1497, 48);
            tlpHeader.TabIndex = 0;
            // 
            // lblXinChao
            // 
            lblXinChao.AutoSize = true;
            lblXinChao.BackColor = Color.DeepSkyBlue;
            lblXinChao.Dock = DockStyle.Fill;
            lblXinChao.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblXinChao.Location = new Point(13, 6);
            lblXinChao.Name = "lblXinChao";
            lblXinChao.Size = new Size(1322, 36);
            lblXinChao.TabIndex = 0;
            lblXinChao.Text = "Xin chào:";
            lblXinChao.TextAlign = ContentAlignment.MiddleLeft;
            lblXinChao.Click += lblXinChao_Click;
            // 
            // btnDangXuat
            // 
            btnDangXuat.Dock = DockStyle.Fill;
            btnDangXuat.Location = new Point(1341, 9);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(143, 30);
            btnDangXuat.TabIndex = 1;
            btnDangXuat.Text = "Đăng xuất";
            // 
            // pnlLeft
            // 
            pnlLeft.BackColor = Color.White;
            pnlLeft.Controls.Add(flpBan);
            pnlLeft.Controls.Add(lblLeftTitle);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Location = new Point(3, 57);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Padding = new Padding(10);
            pnlLeft.Size = new Size(254, 624);
            pnlLeft.TabIndex = 1;
            // 
            // flpBan
            // 
            flpBan.AutoScroll = true;
            flpBan.BackColor = Color.White;
            flpBan.Dock = DockStyle.Fill;
            flpBan.Location = new Point(10, 44);
            flpBan.Name = "flpBan";
            flpBan.Size = new Size(234, 570);
            flpBan.TabIndex = 0;
            // 
            // lblLeftTitle
            // 
            lblLeftTitle.Dock = DockStyle.Top;
            lblLeftTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblLeftTitle.Location = new Point(10, 10);
            lblLeftTitle.Name = "lblLeftTitle";
            lblLeftTitle.Size = new Size(234, 34);
            lblLeftTitle.TabIndex = 1;
            lblLeftTitle.Text = "Danh sách bàn";
            // 
            // pnlMenu
            // 
            pnlMenu.BackColor = Color.White;
            pnlMenu.Controls.Add(flpMonNhanh);
            pnlMenu.Controls.Add(pnlMenuTop);
            pnlMenu.Dock = DockStyle.Fill;
            pnlMenu.Location = new Point(263, 57);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Padding = new Padding(10);
            pnlMenu.Size = new Size(661, 624);
            pnlMenu.TabIndex = 2;
            // 
            // flpMonNhanh
            // 
            flpMonNhanh.AutoScroll = true;
            flpMonNhanh.BackColor = Color.White;
            flpMonNhanh.Dock = DockStyle.Fill;
            flpMonNhanh.Location = new Point(10, 106);
            flpMonNhanh.Name = "flpMonNhanh";
            flpMonNhanh.Padding = new Padding(4);
            flpMonNhanh.Size = new Size(641, 508);
            flpMonNhanh.TabIndex = 0;
            // 
            // pnlMenuTop
            // 
            pnlMenuTop.BackColor = Color.White;
            pnlMenuTop.Controls.Add(lblDanhMuc);
            pnlMenuTop.Controls.Add(cboDanhMuc);
            pnlMenuTop.Controls.Add(lblMon);
            pnlMenuTop.Controls.Add(cboMon);
            pnlMenuTop.Controls.Add(btnThemMon);
            pnlMenuTop.Controls.Add(btnXoaMon);
            pnlMenuTop.Controls.Add(lblTimMon);
            pnlMenuTop.Controls.Add(txtTimMon);
            pnlMenuTop.Dock = DockStyle.Top;
            pnlMenuTop.Location = new Point(10, 10);
            pnlMenuTop.Name = "pnlMenuTop";
            pnlMenuTop.Padding = new Padding(0, 0, 0, 6);
            pnlMenuTop.Size = new Size(641, 96);
            pnlMenuTop.TabIndex = 1;
            // 
            // lblDanhMuc
            // 
            lblDanhMuc.AutoSize = true;
            lblDanhMuc.Location = new Point(0, 6);
            lblDanhMuc.Name = "lblDanhMuc";
            lblDanhMuc.Size = new Size(79, 20);
            lblDanhMuc.TabIndex = 0;
            lblDanhMuc.Text = "Danh mục:";
            // 
            // cboDanhMuc
            // 
            cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDanhMuc.Location = new Point(80, 2);
            cboDanhMuc.Name = "cboDanhMuc";
            cboDanhMuc.Size = new Size(260, 28);
            cboDanhMuc.TabIndex = 1;
            // 
            // lblMon
            // 
            lblMon.AutoSize = true;
            lblMon.Location = new Point(0, 38);
            lblMon.Name = "lblMon";
            lblMon.Size = new Size(42, 20);
            lblMon.TabIndex = 2;
            lblMon.Text = "Món:";
            // 
            // cboMon
            // 
            cboMon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMon.Location = new Point(80, 34);
            cboMon.Name = "cboMon";
            cboMon.Size = new Size(260, 28);
            cboMon.TabIndex = 3;
            // 
            // btnThemMon
            // 
            btnThemMon.Location = new Point(438, 34);
            btnThemMon.Name = "btnThemMon";
            btnThemMon.Size = new Size(80, 28);
            btnThemMon.TabIndex = 4;
            btnThemMon.Text = "Thêm";
            // 
            // btnXoaMon
            // 
            btnXoaMon.Location = new Point(524, 34);
            btnXoaMon.Name = "btnXoaMon";
            btnXoaMon.Size = new Size(80, 28);
            btnXoaMon.TabIndex = 5;
            btnXoaMon.Text = "Xóa";
            // 
            // lblTimMon
            // 
            lblTimMon.AutoSize = true;
            lblTimMon.Location = new Point(0, 70);
            lblTimMon.Name = "lblTimMon";
            lblTimMon.Size = new Size(71, 20);
            lblTimMon.TabIndex = 6;
            lblTimMon.Text = "Tìm món:";
            // 
            // txtTimMon
            // 
            txtTimMon.Location = new Point(80, 66);
            txtTimMon.Name = "txtTimMon";
            txtTimMon.Size = new Size(340, 27);
            txtTimMon.TabIndex = 7;
            // 
            // pnlDon
            // 
            pnlDon.BackColor = Color.WhiteSmoke;
            pnlDon.Controls.Add(flpChiTiet);
            pnlDon.Controls.Add(pnlDonActions);
            pnlDon.Controls.Add(pnlDonFooter);
            pnlDon.Controls.Add(lblBanHienTai);
            pnlDon.Dock = DockStyle.Fill;
            pnlDon.Location = new Point(930, 57);
            pnlDon.Name = "pnlDon";
            pnlDon.Padding = new Padding(10);
            pnlDon.Size = new Size(570, 624);
            pnlDon.TabIndex = 3;
            // 
            // flpChiTiet
            // 
            flpChiTiet.AutoScroll = true;
            flpChiTiet.BackColor = Color.White;
            flpChiTiet.Dock = DockStyle.Fill;
            flpChiTiet.FlowDirection = FlowDirection.TopDown;
            flpChiTiet.Location = new Point(10, 44);
            flpChiTiet.Name = "flpChiTiet";
            flpChiTiet.Size = new Size(550, 482);
            flpChiTiet.TabIndex = 0;
            flpChiTiet.WrapContents = false;
            // 
            // pnlDonActions
            // 
            pnlDonActions.BackColor = Color.White;
            pnlDonActions.Dock = DockStyle.Bottom;
            pnlDonActions.Location = new Point(10, 526);
            pnlDonActions.Name = "pnlDonActions";
            pnlDonActions.Padding = new Padding(10, 6, 10, 6);
            pnlDonActions.Size = new Size(550, 44);
            pnlDonActions.TabIndex = 1;
            // 
            // pnlDonFooter
            // 
            pnlDonFooter.BackColor = Color.White;
            pnlDonFooter.Controls.Add(lblTongTien);
            pnlDonFooter.Controls.Add(lblCaptionTong);
            pnlDonFooter.Dock = DockStyle.Bottom;
            pnlDonFooter.Location = new Point(10, 570);
            pnlDonFooter.Name = "pnlDonFooter";
            pnlDonFooter.Padding = new Padding(10, 6, 10, 6);
            pnlDonFooter.Size = new Size(550, 44);
            pnlDonFooter.TabIndex = 2;
            // 
            // lblTongTien
            // 
            lblTongTien.Dock = DockStyle.Fill;
            lblTongTien.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTongTien.ForeColor = Color.DarkRed;
            lblTongTien.Location = new Point(80, 6);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(460, 32);
            lblTongTien.TabIndex = 0;
            lblTongTien.Text = "0 đ";
            lblTongTien.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCaptionTong
            // 
            lblCaptionTong.Dock = DockStyle.Left;
            lblCaptionTong.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCaptionTong.Location = new Point(10, 6);
            lblCaptionTong.Name = "lblCaptionTong";
            lblCaptionTong.Size = new Size(70, 32);
            lblCaptionTong.TabIndex = 1;
            lblCaptionTong.Text = "Tổng";
            lblCaptionTong.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBanHienTai
            // 
            lblBanHienTai.Dock = DockStyle.Top;
            lblBanHienTai.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblBanHienTai.Location = new Point(10, 10);
            lblBanHienTai.Name = "lblBanHienTai";
            lblBanHienTai.Size = new Size(550, 34);
            lblBanHienTai.TabIndex = 3;
            lblBanHienTai.Text = "Bàn hiện tại: (chưa chọn)";
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.DeepSkyBlue;
            tlpRoot.SetColumnSpan(pnlBottom, 3);
            pnlBottom.Controls.Add(btnThanhToan);
            pnlBottom.Controls.Add(btnThanhToanTungPhan);
            pnlBottom.Controls.Add(btnInTam);
            pnlBottom.Controls.Add(lblFrom);
            pnlBottom.Controls.Add(cboBanFrom);
            pnlBottom.Controls.Add(lblTo);
            pnlBottom.Controls.Add(cboBanTo);
            pnlBottom.Controls.Add(btnChuyenBan);
            pnlBottom.Controls.Add(btnGopBan);
            pnlBottom.Dock = DockStyle.Fill;
            pnlBottom.Location = new Point(3, 687);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Padding = new Padding(10);
            pnlBottom.Size = new Size(1497, 70);
            pnlBottom.TabIndex = 4;
            // 
            // btnThanhToan
            // 
            btnThanhToan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnThanhToan.Location = new Point(1255, 18);
            btnThanhToan.Name = "btnThanhToan";
            btnThanhToan.Size = new Size(194, 32);
            btnThanhToan.TabIndex = 2;
            btnThanhToan.Text = "Thanh toán";
            btnThanhToan.UseVisualStyleBackColor = true;
            // 
            // btnThanhToanTungPhan
            // 
            btnThanhToanTungPhan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnThanhToanTungPhan.Location = new Point(1079, 18);
            btnThanhToanTungPhan.Name = "btnThanhToanTungPhan";
            btnThanhToanTungPhan.Size = new Size(170, 32);
            btnThanhToanTungPhan.TabIndex = 1;
            btnThanhToanTungPhan.Text = "Thanh toán từng phần";
            btnThanhToanTungPhan.UseVisualStyleBackColor = true;
            // 
            // btnInTam
            // 
            btnInTam.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnInTam.Location = new Point(983, 18);
            btnInTam.Name = "btnInTam";
            btnInTam.Size = new Size(90, 32);
            btnInTam.TabIndex = 0;
            btnInTam.Text = "In tạm";
            btnInTam.UseVisualStyleBackColor = true;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Location = new Point(10, 26);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(58, 20);
            lblFrom.TabIndex = 0;
            lblFrom.Text = "Từ bàn:";
            // 
            // cboBanFrom
            // 
            cboBanFrom.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBanFrom.Location = new Point(70, 22);
            cboBanFrom.Name = "cboBanFrom";
            cboBanFrom.Size = new Size(220, 28);
            cboBanFrom.TabIndex = 1;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Location = new Point(310, 26);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(74, 20);
            lblTo.TabIndex = 2;
            lblTo.Text = "Sang bàn:";
            // 
            // cboBanTo
            // 
            cboBanTo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBanTo.Location = new Point(390, 22);
            cboBanTo.Name = "cboBanTo";
            cboBanTo.Size = new Size(220, 28);
            cboBanTo.TabIndex = 3;
            // 
            // btnChuyenBan
            // 
            btnChuyenBan.Location = new Point(640, 20);
            btnChuyenBan.Name = "btnChuyenBan";
            btnChuyenBan.Size = new Size(120, 30);
            btnChuyenBan.TabIndex = 4;
            btnChuyenBan.Text = "Chuyển bàn";
            // 
            // btnGopBan
            // 
            btnGopBan.Location = new Point(770, 20);
            btnGopBan.Name = "btnGopBan";
            btnGopBan.Size = new Size(120, 30);
            btnGopBan.TabIndex = 5;
            btnGopBan.Text = "Gộp bàn";
            // 
            // FormBanHangNhanVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1503, 760);
            Controls.Add(tlpRoot);
            MinimumSize = new Size(1200, 680);
            Name = "FormBanHangNhanVien";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bán hàng - Nhân viên";
            tlpRoot.ResumeLayout(false);
            tlpHeader.ResumeLayout(false);
            tlpHeader.PerformLayout();
            pnlLeft.ResumeLayout(false);
            pnlMenu.ResumeLayout(false);
            pnlMenuTop.ResumeLayout(false);
            pnlMenuTop.PerformLayout();
            pnlDon.ResumeLayout(false);
            pnlDonFooter.ResumeLayout(false);
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
        }
    }
}
