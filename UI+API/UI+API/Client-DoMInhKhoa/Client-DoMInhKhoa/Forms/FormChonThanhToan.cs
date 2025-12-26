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
            ClientSize = new Size(520, 260);

            BuildUI();
            Recalc();
        }

        private void BuildUI()
        {
            lblTong = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(16, 14),
                Text = $"Tổng tiền: {_tongTien.ToString("N0", _vi)} đ"
            };

            var grp = new GroupBox
            {
                Text = "Phương thức",
                Location = new Point(16, 52),
                Size = new Size(488, 90)
            };

            rdoTienMat = new RadioButton
            {
                Text = "Tiền mặt",
                Location = new Point(18, 32),
                AutoSize = true,
                Checked = true
            };

            rdoChuyenKhoan = new RadioButton
            {
                Text = "Chuyển khoản (QR)",
                Location = new Point(140, 32),
                AutoSize = true
            };

            rdoTienMat.CheckedChanged += (s, e) => Recalc();
            rdoChuyenKhoan.CheckedChanged += (s, e) => Recalc();

            grp.Controls.Add(rdoTienMat);
            grp.Controls.Add(rdoChuyenKhoan);

            var lblKD = new Label
            {
                AutoSize = true,
                Text = "Khách đưa:",
                Location = new Point(18, 154)
            };

            txtKhachDua = new TextBox
            {
                Location = new Point(96, 150),
                Width = 180,
                Text = "0"
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
                Location = new Point(300, 154)
            };

            lblTienThoi = new Label
            {
                AutoSize = true,
                Location = new Point(360, 154),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = "0 đ"
            };

            btnOK = new Button
            {
                Text = "Xác nhận",
                Size = new Size(120, 36),
                Location = new Point(260, 204)
            };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(110, 36),
                Location = new Point(394, 204)
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(lblTong);
            Controls.Add(grp);
            Controls.Add(lblKD);
            Controls.Add(txtKhachDua);
            Controls.Add(lblTT);
            Controls.Add(lblTienThoi);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
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
