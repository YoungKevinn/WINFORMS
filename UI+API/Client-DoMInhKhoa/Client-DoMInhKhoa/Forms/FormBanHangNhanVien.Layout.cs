using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien : Form
    {
        // ================== BUILD LAYOUT 3 CỘT ==================
        private void BuildLayout()
        {
            Text = "Cafe Manager - Nhân viên";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Font = new Font("Segoe UI", 9F);
            BackColor = Color.WhiteSmoke;

            // ===== HEADER =====
            _panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.FromArgb(0, 120, 215)
            };

            var lblTitle = new Label
            {
                Text = "Cafe Manager - Nhân viên",
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                Width = 300,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Padding = new Padding(10, 0, 0, 0)
            };

            lblXinChao = new Label
            {
                Text = "",
                ForeColor = Color.White,
                Dock = DockStyle.Right,
                Width = 220,
                TextAlign = ContentAlignment.MiddleRight
            };

            // ===== NÚT ĐĂNG XUẤT =====
            btnDangXuat = new Button
            {
                Text = "Đăng xuất",
                Dock = DockStyle.Right,
                Width = 90,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 120, 215),
                UseVisualStyleBackColor = false
            };
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 100, 190);
            btnDangXuat.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 80, 160);

            _panelHeader.Controls.Add(btnDangXuat);
            _panelHeader.Controls.Add(lblXinChao);
            _panelHeader.Controls.Add(lblTitle);

            // ===== 3 CỘT CHÍNH =====
            _panelDon = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(8, 12, 8, 8),
                BackColor = Color.FromArgb(255, 255, 240)
            };

            _panelMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 420,
                Padding = new Padding(8),
                BackColor = Color.FromArgb(249, 249, 249)
            };

            _panelBan = new Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                Padding = new Padding(8),
                BackColor = Color.FromArgb(242, 242, 242)
            };

            Controls.Add(_panelDon);
            Controls.Add(_panelMenu);
            Controls.Add(_panelBan);
            Controls.Add(_panelHeader);

            BuildBanColumn();
            BuildMenuColumn();
            BuildDonColumn();
        }

        // ===== CỘT TRÁI: DANH SÁCH BÀN =====
        private void BuildBanColumn()
        {
            var lbl = new Label
            {
                Text = "Danh sách bàn",
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            flpBan = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                Padding = new Padding(4)
            };

            _panelBan.Controls.Add(flpBan);
            _panelBan.Controls.Add(lbl);
        }

        // ===== CỘT GIỮA: MENU (DANH MỤC + MÓN) =====
        private void BuildMenuColumn()
        {
            var lblTitle = new Label
            {
                Text = "Menu",
                Dock = DockStyle.Top,
                Height = 26,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            _panelMenuTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 130,          // đủ chỗ cho Danh mục + Món + Tìm món
                Padding = new Padding(5)
            };

            // Danh mục
            var lblDm = new Label
            {
                Text = "Danh mục:",
                Location = new Point(5, 10),
                AutoSize = true
            };

            cboDanhMuc = new ComboBox
            {
                Location = new Point(90, 6),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Món
            var lblMon = new Label
            {
                Text = "Món:",
                Location = new Point(5, 45),
                AutoSize = true
            };

            cboMon = new ComboBox
            {
                Location = new Point(90, 41),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // nud số lượng (ẩn)
            nudSoLuong = new NumericUpDown
            {
                Visible = false
            };

            _panelMenuTop.Controls.Add(lblDm);
            _panelMenuTop.Controls.Add(cboDanhMuc);
            _panelMenuTop.Controls.Add(lblMon);
            _panelMenuTop.Controls.Add(cboMon);
            _panelMenuTop.Controls.Add(nudSoLuong);

            // LIST BUTTON MÓN NHANH
            flpMonNhanh = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                Padding = new Padding(4)
            };

            _panelMenu.Controls.Add(flpMonNhanh);
            _panelMenu.Controls.Add(_panelMenuTop);
            _panelMenu.Controls.Add(lblTitle);
        }

        // ===== CỘT PHẢI: ĐƠN HIỆN TẠI =====
        private void BuildDonColumn()
        {
            var panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60
            };

            lblBanHienTai = new Label
            {
                Text = "Bàn hiện tại: (chưa chọn)",
                AutoSize = false,
                Location = new Point(5, 8),
                Size = new Size(420, 22),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var panelButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 120,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 8, 0, 0)
            };

            btnThemMon = new Button
            {
                Text = "Thêm món",
                Width = 90,
                Height = 30,
                Visible = false
            };

            btnXoaMon = new Button
            {
                Text = "Xóa món",
                Width = 80,
                Height = 30
            };

            panelButtons.Controls.Add(btnXoaMon);

            panelTop.Controls.Add(panelButtons);
            panelTop.Controls.Add(lblBanHienTai);

            flpChiTiet = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 4, 0, 4)
            };

            var panelBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80
            };

            var lblBanFrom = new Label
            {
                Text = "Bàn hiện tại:",
                Location = new Point(5, 10),
                AutoSize = true
            };
            cboBanFrom = new ComboBox
            {
                Location = new Point(90, 6),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnChuyenBan = new Button
            {
                Text = "Chuyển bàn",
                Location = new Point(220, 5),
                Size = new Size(90, 27)
            };

            var lblBanTo = new Label
            {
                Text = "Chuyển tới:",
                Location = new Point(5, 45),
                AutoSize = true
            };
            cboBanTo = new ComboBox
            {
                Location = new Point(90, 41),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnGopBan = new Button
            {
                Text = "Gộp bàn",
                Location = new Point(220, 40),
                Size = new Size(90, 27)
            };

            var lblTong = new Label
            {
                Text = "Tổng tiền:",
                AutoSize = true,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(panelBottom.Width - 260, 18)
            };
            lblTongTien = new Label
            {
                Text = "0 đ",
                AutoSize = true,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.RoyalBlue,
                Location = new Point(panelBottom.Width - 190, 14)
            };

            btnThanhToan = new Button
            {
                Text = "Thanh toán",
                Size = new Size(110, 32),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(panelBottom.Width - 120, 20)
            };

            panelBottom.Resize += (s, e) =>
            {
                lblTong.Location = new Point(panelBottom.Width - 260, 18);
                lblTongTien.Location = new Point(panelBottom.Width - 190, 14);
                btnThanhToan.Location = new Point(panelBottom.Width - 120, 20);
            };

            panelBottom.Controls.Add(lblBanFrom);
            panelBottom.Controls.Add(cboBanFrom);
            panelBottom.Controls.Add(btnChuyenBan);
            panelBottom.Controls.Add(lblBanTo);
            panelBottom.Controls.Add(cboBanTo);
            panelBottom.Controls.Add(btnGopBan);
            panelBottom.Controls.Add(lblTong);
            panelBottom.Controls.Add(lblTongTien);
            panelBottom.Controls.Add(btnThanhToan);

            _panelDon.Controls.Add(flpChiTiet);
            _panelDon.Controls.Add(panelBottom);
            _panelDon.Controls.Add(panelTop);
        }
    }
}
