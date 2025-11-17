using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormHoaDon : Form
    {
        private readonly int _donGoiId;
        private readonly HoaDonService _hoaDonService;
        private List<ChiTietHoaDonDto> _chiTiet = new();

        public FormHoaDon(int donGoiId)
        {
            InitializeComponent();

            _donGoiId = donGoiId;
            _hoaDonService = new HoaDonService();
            this.StartPosition = FormStartPosition.CenterParent;

        }
        private async void FormHoaDon_Load(object sender, EventArgs e)
        {
            try
                {
                _chiTiet = await _hoaDonService.GetChiTietAsync(_donGoiId)
                           ?? new List<ChiTietHoaDonDto>();

                dgvChiTiet.AutoGenerateColumns = true;
                dgvChiTiet.DataSource = _chiTiet;

                lblMaHoaDon.Text = $"Đơn gọi #{_donGoiId}";
                lblNgayGio.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                decimal tong = _chiTiet.Sum(c => c.ThanhTien);
                lblTongTien.Text = tong.ToString("N0") + " đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hóa đơn: " + ex.Message);
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = printDocument1;
                preview.ShowDialog();
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Vẽ nội dung hóa đơn ra giấy
        /// </summary>
        private void PrintDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            float y = 20;

            using var fontTitle = new Font("Arial", 14, FontStyle.Bold);
            using var font = new Font("Arial", 10);

            g.DrawString("QUÁN CAFE YENA", fontTitle, Brushes.Black, 20, y);
            y += 30;
            g.DrawString(lblMaHoaDon.Text, font, Brushes.Black, 20, y);
            y += 20;
            g.DrawString(lblNgayGio.Text, font, Brushes.Black, 20, y);
            y += 30;

            g.DrawString("Tên món", font, Brushes.Black, 20, y);
            g.DrawString("SL", font, Brushes.Black, 220, y);
            g.DrawString("Đơn giá", font, Brushes.Black, 260, y);
            g.DrawString("Thành tiền", font, Brushes.Black, 360, y);
            y += 20;

            foreach (var c in _chiTiet)
            {
                g.DrawString(c.TenMon, font, Brushes.Black, 20, y);
                g.DrawString(c.SoLuong.ToString(), font, Brushes.Black, 220, y);
                g.DrawString(c.DonGia.ToString("N0"), font, Brushes.Black, 260, y);
                g.DrawString(c.ThanhTien.ToString("N0"), font, Brushes.Black, 360, y);
                y += 20;
            }

            y += 20;
            g.DrawString("Tổng: " + lblTongTien.Text, fontTitle, Brushes.Black, 260, y);
        }
    }
}
