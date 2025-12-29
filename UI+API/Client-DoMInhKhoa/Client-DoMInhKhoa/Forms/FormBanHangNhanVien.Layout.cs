using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien
    {
        private const int LEFT_MIN = 230;
        private const int LEFT_DEFAULT = 260;

        private const int BILL_MIN = 460;
        private const double BILL_RATIO = 0.38; // ✅ FIX: bỏ allowreadonly

        // ✅ MỞ APP MẶC ĐỊNH GIỐNG “HÌNH 2”
        private static readonly Size DEFAULT_CLIENT_SIZE = new Size(1600, 860);
        private static readonly Size MIN_FORM_SIZE = new Size(1350, 720);

        // ✅ GIỮ THAM CHIẾU ĐỂ RE-FLOW BOTTOM
        private Panel? _panelBottomDon;
        private FlowLayoutPanel? _flowBottomRow1; // Từ/Sang/Chuyển/Gộp
        private FlowLayoutPanel? _flowBottomRow2; // Tổng tiền + nút

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

            ReflowDonBottom();
        }

        private void ReflowMenuTop()
        {
            if (_panelMenuTop == null) return;

            int padL = 10;
            int padR = 12;

            int yDm = 8;
            int yMon = 41;
            int ySearch = 71;

            // ---- Đăng xuất ----
            btnDangXuat.Location = new Point(_panelMenuTop.ClientSize.Width - btnDangXuat.Width - padR, yDm);

            // ---- Danh mục ----
            int maxW_Dm = btnDangXuat.Left - 90 - 10;
            if (maxW_Dm < 120) maxW_Dm = 120;
            cboDanhMuc.Width = Math.Min(260, maxW_Dm);

            // ---- Món ----
            int maxW_Mon = btnDangXuat.Left - 90 - 10;
            if (maxW_Mon < 120) maxW_Mon = 120;
            cboMon.Width = Math.Min(260, maxW_Mon);

            // ---- 2 nút Thêm/Xóa ----
            int startX = cboMon.Right + 20;

            int needW = btnThemMon.Width + 10 + btnXoaMon.Width;
            int availW = btnDangXuat.Left - startX - padR;

            bool wrapXoa = availW < needW;

            btnThemMon.Location = new Point(startX, yMon);

            if (!wrapXoa)
            {
                btnXoaMon.Location = new Point(btnThemMon.Right + 10, yMon);
                ySearch = 71;
                _panelMenuTop.Height = 130;
            }
            else
            {
                btnXoaMon.Location = new Point(startX, yMon + btnThemMon.Height + 6);
                ySearch = btnXoaMon.Bottom + 8;

                int newH = ySearch + 30 + 10;
                if (newH < 130) newH = 130;
                _panelMenuTop.Height = newH;
            }

            // ---- Reflow "Tìm món" ----
            var lblSearch = _panelMenuTop.Controls.OfType<Label>().FirstOrDefault(x => x.Name == "lblTimMon");
            var txtSearch = _panelMenuTop.Controls.OfType<TextBox>().FirstOrDefault(x => x.Name == "txtTimMon");

            if (lblSearch != null)
                lblSearch.Location = new Point(padL, ySearch + 4);

            if (txtSearch != null)
                txtSearch.Location = new Point(90, ySearch);
        }

        private void ReflowDonBottom()
        {
            if (_panelBottomDon == null || _flowBottomRow1 == null || _flowBottomRow2 == null) return;

            _panelBottomDon.AutoScroll = false;

            int w = _panelBottomDon.ClientSize.Width;
            if (w <= 0) return;

            // ✅ ổn định hơn PreferredSize khi wrap
            Size row1Pref = _flowBottomRow1.GetPreferredSize(new Size(w, 0));
            Size row2Pref = _flowBottomRow2.GetPreferredSize(new Size(w, 0));

            bool stack2Rows =
                row1Pref.Width > w ||
                row2Pref.Width > w ||
                (row1Pref.Width + row2Pref.Width) > w;

            if (stack2Rows)
            {
                _flowBottomRow1.Dock = DockStyle.Top;
                _flowBottomRow2.Dock = DockStyle.Top;

                _flowBottomRow1.WrapContents = true;
                _flowBottomRow2.WrapContents = true;

                _flowBottomRow1.AutoSize = true;
                _flowBottomRow2.AutoSize = true;

                _flowBottomRow1.PerformLayout();
                _flowBottomRow2.PerformLayout();

                int h1 = _flowBottomRow1.GetPreferredSize(new Size(w, 0)).Height;
                int h2 = _flowBottomRow2.GetPreferredSize(new Size(w, 0)).Height;

                int newH = h1 + h2 + 14;
                if (newH < 110) newH = 110;
                _panelBottomDon.Height = newH;
            }
            else
            {
                _flowBottomRow1.Dock = DockStyle.Left;
                _flowBottomRow2.Dock = DockStyle.Right;

                _flowBottomRow1.WrapContents = false;
                _flowBottomRow2.WrapContents = false;

                _flowBottomRow1.AutoSize = true;
                _flowBottomRow2.AutoSize = true;

                _panelBottomDon.Height = 80;
            }

            _panelBottomDon.PerformLayout();
        }

        private void BuildLayout()
        {
            Text = "Bán hàng - Nhân viên";

            // ✅ MẶC ĐỊNH RUN RA GIỐNG “HÌNH 2”
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimumSize = MIN_FORM_SIZE;

            // ✅ set ClientSize (ổn định)
            ClientSize = DEFAULT_CLIENT_SIZE;

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

            SizeChanged += (s, e) => ApplyMainColumnsLayout();
            Shown += (s, e) =>
            {
                ApplyMainColumnsLayout();
                ReflowMenuTop();
                ReflowDonBottom();
            };

            // ===== LEFT: BÀN =====
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

            // ===== MENU TOP =====
            _panelMenuTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 130,
                BackColor = Color.WhiteSmoke
            };

            _panelMenuTop.Controls.Add(btnDangXuat);
            _panelMenuTop.Resize += (s, e) => ReflowMenuTop();

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

            // ===== DON TOP =====
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.WhiteSmoke };

            lblBanHienTai = new Label
            {
                Text = "Bàn hiện tại:",
                AutoSize = true,
                Location = new Point(12, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panelTop.Controls.Add(lblBanHienTai);

            // ===== DON CENTER =====
            flpChiTiet = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(8),
                BackColor = Color.White
            };

            // ===== DON BOTTOM =====
            _panelBottomDon = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Color.WhiteSmoke,
                AutoScroll = false
            };

            // Row1
            _flowBottomRow1 = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(8, 6, 8, 0),
                Margin = new Padding(0),
                Dock = DockStyle.Left
            };

            var lblBanFrom = new Label { Text = "Từ:", AutoSize = true, Margin = new Padding(0, 7, 6, 0) };
            cboBanFrom = new ComboBox { Width = 120, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(0, 4, 10, 0) };
            btnChuyenBan = new Button { Text = "Chuyển", Size = new Size(70, 26), Margin = new Padding(0, 4, 14, 0) };

            var lblBanTo = new Label { Text = "Sang:", AutoSize = true, Margin = new Padding(0, 7, 6, 0) };
            cboBanTo = new ComboBox { Width = 120, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(0, 4, 10, 0) };
            btnGopBan = new Button { Text = "Gộp", Size = new Size(60, 26), Margin = new Padding(0, 4, 0, 0) };

            _flowBottomRow1.Controls.Add(lblBanFrom);
            _flowBottomRow1.Controls.Add(cboBanFrom);
            _flowBottomRow1.Controls.Add(btnChuyenBan);
            _flowBottomRow1.Controls.Add(lblBanTo);
            _flowBottomRow1.Controls.Add(cboBanTo);
            _flowBottomRow1.Controls.Add(btnGopBan);

            // Row2
            _flowBottomRow2 = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 6, 8, 0),
                Margin = new Padding(0),
                Dock = DockStyle.Right
            };

            var lblTong = new Label
            {
                Text = "Tổng tiền:",
                AutoSize = true,
                Margin = new Padding(0, 7, 8, 0)
            };

            lblTongTien = new Label
            {
                Text = "0 đ",
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.RoyalBlue,
                Margin = new Padding(0, 4, 12, 0)
            };

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

            _flowBottomRow2.Controls.Add(lblTong);
            _flowBottomRow2.Controls.Add(lblTongTien);
            _flowBottomRow2.Controls.Add(btnInTam);
            _flowBottomRow2.Controls.Add(btnThanhToanTungPhan);
            _flowBottomRow2.Controls.Add(btnThanhToan);

            _panelBottomDon.Controls.Add(_flowBottomRow2);
            _panelBottomDon.Controls.Add(_flowBottomRow1);

            _panelBottomDon.SizeChanged += (s, e) => ReflowDonBottom();
            _panelDon.SizeChanged += (s, e) => ReflowDonBottom();

            _panelDon.Controls.Add(flpChiTiet);
            _panelDon.Controls.Add(_panelBottomDon);
            _panelDon.Controls.Add(panelTop);

            _panelMenuTop.PerformLayout();
            ReflowMenuTop();
            ReflowDonBottom();
        }
    }
}
