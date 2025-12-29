using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public sealed class FormChonThanhToan : Form
    {
        private readonly CultureInfo _vi = new CultureInfo("vi-VN");
        private readonly decimal _tongTien;

        private RadioButton rdoTienMat = null!;
        private RadioButton rdoChuyenKhoan = null!;
        private TextBox txtKhachDua = null!;
        private Label lblTong = null!;
        private Label lblTienThoi = null!;
        private Button btnOK = null!;
        private Button btnCancel = null!;

        private bool _formatting = false;

        public string PhuongThuc { get; private set; } = "Tiền mặt";
        public decimal KhachDua { get; private set; } = 0m;
        public decimal TienThoi { get; private set; } = 0m;

        public FormChonThanhToan(decimal tongTien)
        {
            _tongTien = tongTien;

            Text = "Chọn phương thức thanh toán";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            AutoScaleMode = AutoScaleMode.Font;
            Font = new Font("Segoe UI", 9.5f);

            // ✅ đủ chỗ cho nút chắc chắn (kể cả DPI 125%)
            ClientSize = new Size(560, 300);

            BuildUI();
            Recalc();

            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }

        private void BuildUI()
        {
            // ===== Root =====
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(16, 14, 16, 12)
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // lblTong
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // groupbox
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // cash row
            Controls.Add(root);

            // ===== Tổng tiền =====
            lblTong = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Text = $"Tổng tiền: {_tongTien.ToString("N0", _vi)} đ",
                Margin = new Padding(0, 0, 0, 10)
            };
            root.Controls.Add(lblTong, 0, 0);

            // ===== Groupbox phương thức =====
            var grp = new GroupBox
            {
                Text = "Phương thức",
                Dock = DockStyle.Top,
                Height = 92,
                Margin = new Padding(0, 0, 0, 12)
            };

            rdoTienMat = new RadioButton
            {
                Text = "Tiền mặt",
                AutoSize = true,
                Location = new Point(18, 34),
                Checked = true
            };

            rdoChuyenKhoan = new RadioButton
            {
                Text = "Chuyển khoản (QR)",
                AutoSize = true,
                Location = new Point(140, 34)
            };

            rdoTienMat.CheckedChanged += (s, e) => Recalc();
            rdoChuyenKhoan.CheckedChanged += (s, e) => Recalc();

            grp.Controls.Add(rdoTienMat);
            grp.Controls.Add(rdoChuyenKhoan);
            root.Controls.Add(grp, 0, 1);

            // ===== Row: Khách đưa / Tiền thối =====
            var row = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 4,
                RowCount = 1,
                Margin = new Padding(0, 0, 0, 10)
            };
            row.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));         // "Khách đưa:"
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220f));  // textbox
            row.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));         // "Tiền thối:"
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));    // value

            var lblKD = new Label
            {
                AutoSize = true,
                Text = "Khách đưa:",
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 6, 10, 0)
            };

            txtKhachDua = new TextBox
            {
                Width = 220,
                Text = "0",
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 2, 18, 0)
            };

            txtKhachDua.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };

            txtKhachDua.TextChanged += (s, e) =>
            {
                if (_formatting) return;

                if (!rdoTienMat.Checked)
                {
                    Recalc();
                    return;
                }

                try
                {
                    _formatting = true;
                    var v = ParseMoney(txtKhachDua.Text);
                    txtKhachDua.Text = v.ToString("N0", _vi);
                    txtKhachDua.SelectionStart = txtKhachDua.Text.Length;
                }
                finally
                {
                    _formatting = false;
                }

                Recalc();
            };

            var lblTT = new Label
            {
                AutoSize = true,
                Text = "Tiền thối:",
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 6, 10, 0)
            };

            lblTienThoi = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = "0 đ",
                Margin = new Padding(0, 3, 0, 0)
            };

            row.Controls.Add(lblKD, 0, 0);
            row.Controls.Add(txtKhachDua, 1, 0);
            row.Controls.Add(lblTT, 2, 0);
            row.Controls.Add(lblTienThoi, 3, 0);

            root.Controls.Add(row, 0, 2);

            // ===== Buttons (Dock Bottom luôn hiện) =====
            var bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 54,
                Padding = new Padding(16, 8, 16, 10),
                BackColor = SystemColors.Control
            };

            var flowBtns = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            btnOK = new Button
            {
                Text = "Xác nhận",
                Size = new Size(120, 36),
                Margin = new Padding(0, 0, 12, 0)
            };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(110, 36),
                Margin = new Padding(0, 0, 0, 0)
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            flowBtns.Controls.Add(btnOK);
            flowBtns.Controls.Add(btnCancel);

            bottom.Controls.Add(flowBtns);
            Controls.Add(bottom);
        }

        private void Recalc()
        {
            bool isCash = rdoTienMat.Checked;

            txtKhachDua.Enabled = isCash;
            txtKhachDua.ReadOnly = !isCash;

            if (!isCash)
            {
                lblTienThoi.Text = "0 đ";
                return;
            }

            var kd = ParseMoney(txtKhachDua.Text);
            var thoi = kd - _tongTien;
            if (thoi < 0) thoi = 0;

            lblTienThoi.Text = thoi.ToString("N0", _vi) + " đ";
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            PhuongThuc = rdoTienMat.Checked ? "Tiền mặt" : "Chuyển khoản";

            if (PhuongThuc == "Tiền mặt")
            {
                var kd = ParseMoney(txtKhachDua.Text);
                if (kd < _tongTien)
                {
                    MessageBox.Show("Tiền khách đưa chưa đủ.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                KhachDua = kd;
                TienThoi = kd - _tongTien;
            }
            else
            {
                KhachDua = 0;
                TienThoi = 0;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private static decimal ParseMoney(string? s)
        {
            var raw = (s ?? "").Trim();
            if (string.IsNullOrWhiteSpace(raw)) return 0m;
            var digits = new string(raw.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(digits)) return 0m;
            return decimal.TryParse(digits, out var v) ? v : 0m;
        }
    }
}
