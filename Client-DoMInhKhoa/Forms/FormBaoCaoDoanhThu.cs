using Client_DoMInhKhoa.Services;
using System;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBaoCaoDoanhThu : Form
    {
        private readonly BaoCaoService _service = new BaoCaoService();

        public FormBaoCaoDoanhThu()
        {
            InitializeComponent();

            // Mặc định chọn từ đầu tháng
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = DateTime.Now;

            btnXem.Click += BtnXem_Click;
        }

        private async void BtnXem_Click(object sender, EventArgs e)
        {
            try
            {
                btnXem.Enabled = false;
                btnXem.Text = "Đang tải...";

                var data = await _service.GetDoanhThuNhanVienAsync(dtpFrom.Value, dtpTo.Value);

                dgvBaoCao.DataSource = data;

                if (data == null || data.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu trong khoảng thời gian này.", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message);
            }
            finally
            {
                btnXem.Enabled = true;
                btnXem.Text = "Xem Báo Cáo";
            }
        }
    }
}