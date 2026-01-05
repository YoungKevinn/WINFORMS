using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormChonThanhToan : Form
    {
        private readonly CultureInfo _vi = new CultureInfo("vi-VN");
        private readonly decimal _tongTien;

        private bool _formatting = false;

        public string PhuongThuc { get; private set; } = "Tiền mặt";
        public decimal KhachDua { get; private set; } = 0m;
        public decimal TienThoi { get; private set; } = 0m;

        public FormChonThanhToan(decimal tongTien)
        {
            _tongTien = tongTien;

            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            // init UI
            lblTong.Text = string.Format(_vi, "{0:N0} đ", _tongTien);
            lblTienThoi.Text = "0 đ";
            txtKhachDua.Text = "";

            // default
            rdoTienMat.Checked = true;

            // events
            rdoTienMat.CheckedChanged += (_, __) => SetMode();
            rdoChuyenKhoan.CheckedChanged += (_, __) => SetMode();

            txtKhachDua.KeyPress += TxtKhachDua_KeyPress;
            txtKhachDua.TextChanged += TxtKhachDua_TextChanged;

            btnOK.Click += (_, __) => OnOk();
            btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

            AcceptButton = btnOK;
            CancelButton = btnCancel;

            SetMode();
        }

        private void SetMode()
        {
            bool isTienMat = rdoTienMat.Checked;

            txtKhachDua.Enabled = isTienMat;
            txtKhachDua.ReadOnly = !isTienMat;

            if (!isTienMat)
            {
                // Chuyển khoản: xem như trả đúng
                PhuongThuc = "Chuyển khoản";

                _formatting = true;
                txtKhachDua.Text = string.Format(_vi, "{0:N0}", _tongTien);
                txtKhachDua.SelectionStart = txtKhachDua.Text.Length;
                _formatting = false;

                lblTienThoi.Text = "0 đ";
                KhachDua = _tongTien;
                TienThoi = 0m;
            }
            else
            {
                PhuongThuc = "Tiền mặt";

                // tiền mặt: để nhập
                if (string.IsNullOrWhiteSpace(txtKhachDua.Text))
                {
                    lblTienThoi.Text = "0 đ";
                    KhachDua = 0m;
                    TienThoi = 0m;
                }
                else
                {
                    RecalcTienThoi(ParseMoney(txtKhachDua.Text));
                }

                txtKhachDua.Focus();
            }
        }

        // Chỉ cho nhập số + backspace
        private void TxtKhachDua_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // Format tiền Việt ngay khi gõ + giữ caret “đỡ nhảy”
        private void TxtKhachDua_TextChanged(object? sender, EventArgs e)
        {
            if (_formatting) return;
            if (!rdoTienMat.Checked) return;

            _formatting = true;
            try
            {
                int oldSel = txtKhachDua.SelectionStart;

                // số lượng digit trước caret (để phục hồi caret)
                int digitBeforeCaret = CountDigits(txtKhachDua.Text.Substring(0, Math.Min(oldSel, txtKhachDua.Text.Length)));

                // lấy chuỗi số
                string digits = new string(txtKhachDua.Text.Where(char.IsDigit).ToArray());

                if (string.IsNullOrEmpty(digits))
                {
                    txtKhachDua.Text = "";
                    txtKhachDua.SelectionStart = 0;
                    RecalcTienThoi(0m);
                    return;
                }

                if (!decimal.TryParse(digits, out decimal value))
                    value = 0m;

                // format 1.000.000
                string formatted = value.ToString("N0", _vi);

                txtKhachDua.Text = formatted;
                txtKhachDua.SelectionStart = CaretIndexFromDigitCount(formatted, digitBeforeCaret);

                RecalcTienThoi(value);
            }
            finally
            {
                _formatting = false;
            }
        }

        private void RecalcTienThoi(decimal khach)
        {
            if (!rdoTienMat.Checked)
            {
                lblTienThoi.Text = "0 đ";
                KhachDua = _tongTien;
                TienThoi = 0m;
                return;
            }

            decimal thoi = khach - _tongTien;
            if (thoi < 0) thoi = 0;

            lblTienThoi.Text = string.Format(_vi, "{0:N0} đ", thoi);

            KhachDua = khach;
            TienThoi = thoi;
        }

        private decimal ParseMoney(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;

            string raw = new string(s.Where(char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(raw)) return 0m;

            return decimal.TryParse(raw, out var v) ? v : 0m;
        }

        private static int CountDigits(string s) => s.Count(char.IsDigit);

        private static int CaretIndexFromDigitCount(string formatted, int digitCount)
        {
            if (digitCount <= 0) return 0;

            int count = 0;
            for (int i = 0; i < formatted.Length; i++)
            {
                if (char.IsDigit(formatted[i]))
                {
                    count++;
                    if (count >= digitCount) return i + 1;
                }
            }
            return formatted.Length;
        }

        private void OnOk()
        {
            if (rdoChuyenKhoan.Checked)
            {
                PhuongThuc = "Chuyển khoản";
                KhachDua = _tongTien;
                TienThoi = 0m;

                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            decimal khach = ParseMoney(txtKhachDua.Text);

            if (khach <= 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền khách đưa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtKhachDua.Focus();
                return;
            }

            if (khach < _tongTien)
            {
                MessageBox.Show("Tiền khách đưa chưa đủ.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKhachDua.Focus();
                return;
            }

            PhuongThuc = "Tiền mặt";
            KhachDua = khach;
            TienThoi = khach - _tongTien;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
