using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien
    {
        private const int LEFT_MIN = 230;
        private const int LEFT_DEFAULT = 260;
        private const int BILL_MIN = 460;
        private const double BILL_RATIO = 0.38;

        private void ApplyMainColumnsLayout()
        {
            if (_panelBan == null || _panelDon == null || _panelMenu == null) return;

            int totalW = ClientSize.Width;
            if (totalW <= 0) return;

            int leftW = LEFT_DEFAULT;
            if (leftW < LEFT_MIN) leftW = LEFT_MIN;

            int billW = (int)(totalW * BILL_RATIO);
            if (billW < BILL_MIN) billW = BILL_MIN;

            int menuMin = 320;
            int maxBillW = totalW - leftW - menuMin;
            if (maxBillW < BILL_MIN) maxBillW = BILL_MIN;

            if (billW > maxBillW) billW = maxBillW;
            if (billW < BILL_MIN) billW = BILL_MIN;

            _panelBan.Width = leftW;
            _panelDon.Width = billW;
        }

        private void BuildLayout()
        {
            Text = "Bán hàng - Nhân viên";
            Width = 1200;
            Height = 720;
            StartPosition = FormStartPosition.CenterScreen;

            _panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 0,
                BackColor = Color.FromArgb(30, 144, 255),
                Visible = false
            };

            lblXinChao = new Label { Text = "", Visible = false };

            btnDangXuat = new Button
            {
                Text = "Đăng xuất",
                AutoSize = false,
                Size = new Size(95, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.RoyalBlue,
                Visible = true
            };
            btnDangXuat.FlatAppearance.BorderSize = 1;
            btnDangXuat.FlatAppearance.BorderColor = Color.RoyalBlue;

            _panelBan = new Panel { Dock = DockStyle.Left, Width = 260, BackColor = Color.WhiteSmoke };
            _panelMenu = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            _panelDon = new Panel { Dock = DockStyle.Right, Width = 520, BackColor = Color.WhiteSmoke };

            Controls.Add(_panelMenu);
            Controls.Add(_panelDon);
            Controls.Add(_panelBan);

            ApplyMainColumnsLayout();
            Resize += (s, e) => ApplyMainColumnsLayout();
            Shown += (s, e) => ApplyMainColumnsLayout();

            var lblBan = new Label
            {
                Text = "Danh sách bàn",
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            flpBan = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(8),
                BackColor = Color.White
            };

            _panelBan.Controls.Add(flpBan);
            _panelBan.Controls.Add(lblBan);

            _panelMenuTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 130,
                BackColor = Color.WhiteSmoke
            };

            _panelMenuTop.Controls.Add(btnDangXuat);
            _panelMenuTop.Resize += (s, e) =>
            {
                btnDangXuat.Location = new Point(_panelMenuTop.Width - btnDangXuat.Width - 12, 8);
            };
            btnDangXuat.Location = new Point(_panelMenuTop.Width - btnDangXuat.Width - 12, 8);

            var lblDm = new Label
            {
                Text = "Danh mục:",
                AutoSize = true,
                Location = new Point(10, 12),
                Font = new Font("Segoe UI", 9)
            };

            cboDanhMuc = new ComboBox
            {
                Width = 260,
                Location = new Point(90, 8),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            var lblMon = new Label
            {
                Text = "Món:",
                AutoSize = true,
                Location = new Point(10, 45),
                Font = new Font("Segoe UI", 9)
            };

            cboMon = new ComboBox
            {
                Width = 260,
                Location = new Point(90, 41),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnThemMon = new Button
            {
                Text = "Thêm món",
                Size = new Size(110, 28),
                Location = new Point(370, 40)
            };

            btnXoaMon = new Button
            {
                Text = "Xóa món",
                Size = new Size(110, 28),
                Location = new Point(490, 40)
            };

            _panelMenuTop.Controls.Add(lblDm);
            _panelMenuTop.Controls.Add(cboDanhMuc);
            _panelMenuTop.Controls.Add(lblMon);
            _panelMenuTop.Controls.Add(cboMon);
            _panelMenuTop.Controls.Add(btnThemMon);
            _panelMenuTop.Controls.Add(btnXoaMon);

            flpMonNhanh = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            _panelMenu.Controls.Add(flpMonNhanh);
            _panelMenu.Controls.Add(_panelMenuTop);

            var panelTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.WhiteSmoke };

            lblBanHienTai = new Label
            {
                Text = "Bàn hiện tại:",
                AutoSize = true,
                Location = new Point(12, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            panelTop.Controls.Add(lblBanHienTai);

            flpChiTiet = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(8),
                BackColor = Color.White
            };

            var panelBottom = new Panel { Dock = DockStyle.Bottom, Height = 80, BackColor = Color.WhiteSmoke };

            var lblBanFrom = new Label { Text = "Từ:", AutoSize = true, Location = new Point(10, 10) };
            cboBanFrom = new ComboBox { Width = 120, Location = new Point(40, 7), DropDownStyle = ComboBoxStyle.DropDownList };
            btnChuyenBan = new Button { Text = "Chuyển", Size = new Size(70, 26), Location = new Point(165, 6) };

            var lblBanTo = new Label { Text = "Sang:", AutoSize = true, Location = new Point(245, 10) };
            cboBanTo = new ComboBox { Width = 120, Location = new Point(290, 7), DropDownStyle = ComboBoxStyle.DropDownList };
            btnGopBan = new Button { Text = "Gộp", Size = new Size(60, 26), Location = new Point(415, 6) };

            panelBottom.Controls.Add(lblBanFrom);
            panelBottom.Controls.Add(cboBanFrom);
            panelBottom.Controls.Add(btnChuyenBan);
            panelBottom.Controls.Add(lblBanTo);
            panelBottom.Controls.Add(cboBanTo);
            panelBottom.Controls.Add(btnGopBan);

            var payFlow = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            var lblTong = new Label
            {
                Text = "Tổng tiền:",
                AutoSize = true,
                Margin = new Padding(0, 8, 8, 0)
            };

            lblTongTien = new Label
            {
                Text = "0 đ",
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.RoyalBlue,
                Margin = new Padding(0, 5, 10, 0)
            };

            // ✅ THÊM NÚT IN TẠM
            btnInTam = new Button
            {
                Text = "In tạm",
                Size = new Size(90, 32),
                Margin = new Padding(0, 2, 10, 0)
            };

            btnThanhToanTungPhan = new Button
            {
                Text = "Thanh toán từng phần",
                Size = new Size(160, 32),
                Margin = new Padding(0, 2, 10, 0)
            };

            btnThanhToan = new Button
            {
                Text = "Thanh toán",
                Size = new Size(110, 32),
                Margin = new Padding(0, 2, 0, 0)
            };

            payFlow.Controls.Add(lblTong);
            payFlow.Controls.Add(lblTongTien);
            payFlow.Controls.Add(btnInTam); // ✅ add
            payFlow.Controls.Add(btnThanhToanTungPhan);
            payFlow.Controls.Add(btnThanhToan);

            panelBottom.Controls.Add(payFlow);

            panelBottom.Resize += (s, e) =>
            {
                int rightPad = 12;
                int bottomPad = 8;

                payFlow.Location = new Point(
                    panelBottom.ClientSize.Width - payFlow.PreferredSize.Width - rightPad,
                    panelBottom.ClientSize.Height - payFlow.PreferredSize.Height - bottomPad
                );
            };

            panelBottom.PerformLayout();

            _panelDon.Controls.Add(flpChiTiet);
            _panelDon.Controls.Add(panelBottom);
            _panelDon.Controls.Add(panelTop);
        }
    }
}
