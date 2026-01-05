using Client_DoMInhKhoa.Services;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormThanhToanQR : Form
    {
        private readonly decimal _tongTien;
        private readonly int _donGoiId;
        private readonly int _banId;

        public bool DaNhanTien { get; private set; } = false;

        // ✅ để bill lấy ảnh QR
        public Image? QrImage => picQR.Image;

        public FormThanhToanQR(decimal tongTien, int donGoiId, int banId)
        {
            InitializeComponent();

            _tongTien = tongTien;
            _donGoiId = donGoiId;
            _banId = banId;

            StartPosition = FormStartPosition.CenterParent;
            lblTongTien.Text = $"Tổng tiền: {_tongTien:N0} đ";

            Load += FormThanhToanQR_Load;
            btnDaNhanTien.Click += BtnDaNhanTien_Click;
            btnHuy.Click += BtnHuy_Click;
        }

        private void BtnDaNhanTien_Click(object? sender, EventArgs e)
        {
            DaNhanTien = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            DaNhanTien = false;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void FormThanhToanQR_Load(object? sender, EventArgs e)
        {
            await LoadQrAsync();
        }

        private async Task LoadQrAsync()
        {
            try
            {
                // ✅ addInfo theo format bạn đang dùng
                string addInfo = $"DG{_donGoiId}-BAN{_banId}";

                // ✅ amount decimal -> để API dễ parse: dùng invariant (tránh dấu , . theo máy)
                string amount = _tongTien.ToString(System.Globalization.CultureInfo.InvariantCulture);

                string url = $"api/ThanhToan/vietqr?amount={amount}&addInfo={Uri.EscapeDataString(addInfo)}";

                var res = await ApiClient.GetAsync<QrResponse>(url, includeAuth: true);
                if (res == null || string.IsNullOrWhiteSpace(res.QrImageUrl))
                {
                    MessageBox.Show("API trả về QR rỗng.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using var http = new HttpClient();
                var bytes = await http.GetByteArrayAsync(res.QrImageUrl);

                using var ms = new MemoryStream(bytes);
                picQR.Image = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được QR:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class QrResponse
        {
            public string QrImageUrl { get; set; } = "";
        }
    }
}
