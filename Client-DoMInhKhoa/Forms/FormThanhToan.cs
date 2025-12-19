using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormThanhToan : Form
    {
        private readonly int _donGoiId;
        private readonly decimal _tongTien;
        private decimal _daThanhToan;
        private decimal _canTra;

        public bool IsThanhToanHet { get; private set; } = false;
        private readonly CultureInfo _vi = new CultureInfo("vi-VN");

        public FormThanhToan(int donGoiId, decimal tongTien)
        {
            InitializeComponent();
            _donGoiId = donGoiId;
            _tongTien = tongTien;

            // Setup Event
            btnXacNhan.Click += BtnXacNhan_Click;
            btnHuy.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            txtKhachDua.TextChanged += TxtKhachDua_TextChanged;

            this.Load += FormThanhToan_Load;
        }

        // 1. Thêm khai báo Service ở đầu class
        private readonly ThanhToanService _thanhToanService = new ThanhToanService();

        // 2. Sửa hàm Load
        private async void FormThanhToan_Load(object sender, EventArgs e)
        {
            try
            {
                // Gọi API lấy lịch sử các lần trả tiền trước đó
                var lichSu = await _thanhToanService.GetLichSuAsync(_donGoiId);

                if (lichSu != null)
                {
                    // Cộng dồn tất cả các lần trả trước
                    _daThanhToan = lichSu.Sum(x => x.SoTien);
                }
                else
                {
                    _daThanhToan = 0;
                }
            }
            catch
            {
                // Nếu lỗi kết nối hoặc chưa có API, tạm thời coi như chưa trả
                _daThanhToan = 0;
            }

            // Cập nhật lại giao diện sau khi đã có số liệu đúng
            UpdateSoLieu();
        }
        private void UpdateSoLieu()
        {
            _canTra = _tongTien - _daThanhToan;
            if (_canTra < 0) _canTra = 0;

            lblTongTienVal.Text = _tongTien.ToString("C0", _vi);
            lblDaTraVal.Text = _daThanhToan.ToString("C0", _vi);
            lblConLaiVal.Text = _canTra.ToString("C0", _vi);

            // Gợi ý số tiền
            txtKhachDua.Text = _canTra.ToString("N0", _vi);
            cboPhuongThuc.SelectedIndex = 0;
            this.ActiveControl = txtKhachDua;
        }

        private void TxtKhachDua_TextChanged(object sender, EventArgs e)
        {
            string input = txtKhachDua.Text.Replace(".", "").Replace(",", "").Replace(" ", "").Replace("đ", "");

            if (decimal.TryParse(input, out decimal khachDua))
            {
                decimal tienThua = khachDua - _canTra;

                if (tienThua >= 0)
                {
                    // Trả đủ hoặc dư
                    lblTienThuaLabel.Text = "Tiền trả lại:";
                    lblTienThuaVal.Text = tienThua.ToString("C0", _vi);
                    lblTienThuaVal.ForeColor = System.Drawing.Color.ForestGreen;

                    btnXacNhan.Text = "HOÀN TẤT THANH TOÁN";
                    btnXacNhan.BackColor = System.Drawing.Color.FromArgb(13, 45, 104);
                    IsThanhToanHet = true;
                }
                else
                {
                    // Trả thiếu
                    lblTienThuaLabel.Text = "Còn thiếu:";
                    lblTienThuaVal.Text = Math.Abs(tienThua).ToString("C0", _vi);
                    lblTienThuaVal.ForeColor = System.Drawing.Color.OrangeRed;

                    btnXacNhan.Text = "THANH TOÁN 1 PHẦN";
                    btnXacNhan.BackColor = System.Drawing.Color.OrangeRed;
                    IsThanhToanHet = false;
                }
                btnXacNhan.Enabled = true;
            }
            else
            {
                lblTienThuaVal.Text = "...";
                btnXacNhan.Enabled = false;
            }
        }

        private async void BtnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                string input = txtKhachDua.Text.Replace(".", "").Replace(",", "").Replace(" ", "");
                if (!decimal.TryParse(input, out decimal soTienThanhToan)) return;

                if (soTienThanhToan > _canTra) soTienThanhToan = _canTra;

                var dto = new ThanhToanCreateUpdateDto
                {
                    DonGoiId = _donGoiId,
                    SoTien = soTienThanhToan,
                    PhuongThuc = cboPhuongThuc.SelectedItem.ToString(),
                    ThanhToanLuc = DateTime.Now
                };

                await ApiClient.PostAsync<string>("api/ThanhToan", dto, includeAuth: true);

                string msg = IsThanhToanHet
                    ? "Đã thanh toán hoàn tất!"
                    : $"Đã thanh toán 1 phần ({soTienThanhToan:N0} đ). Đơn vẫn mở.";

                MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message);
            }
        }
    }
}
