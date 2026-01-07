using Client_DoMInhKhoa.Services;
using System;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormDoiMatKhau : Form
    {
        private readonly TaiKhoanService _taiKhoanService;

        private readonly bool _batBuoc;
        public bool ChangedSuccessfully { get; private set; }

        public FormDoiMatKhau(bool batBuoc = false)
        {
            InitializeComponent();
            _taiKhoanService = new TaiKhoanService();

            _batBuoc = batBuoc;
            this.FormClosing += FormDoiMatKhau_FormClosing;

            if (_batBuoc)
            {
                this.Text = "Đổi mật khẩu (bắt buộc)";
                txtMatKhauCu.Text = "123456";
                txtMatKhauCu.ReadOnly = true;
                btnHuy.Visible = false;
            }
        }

        private void FormDoiMatKhau_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_batBuoc && !ChangedSuccessfully)
            {
                var r = MessageBox.Show(
                    "Bạn phải đổi mật khẩu để tiếp tục sử dụng hệ thống.\nBạn có muốn thoát ứng dụng không?",
                    "Bắt buộc đổi mật khẩu",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (r == DialogResult.Yes)
                {
                    Application.Exit();
                    return;
                }

                e.Cancel = true;
            }
        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            string mkCu = txtMatKhauCu.Text;
            string mkMoi = txtMatKhauMoi.Text;
            string mkXacNhan = txtXacNhan.Text;

            if (string.IsNullOrWhiteSpace(mkCu) ||
                string.IsNullOrWhiteSpace(mkMoi) ||
                string.IsNullOrWhiteSpace(mkXacNhan))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.",
                    "Thiếu dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (mkMoi != mkXacNhan)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận không khớp.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (mkMoi.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải từ 6 ký tự trở lên.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnLuu.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                await _taiKhoanService.DoiMatKhauAsync(mkCu, mkMoi);

                MessageBox.Show("Đổi mật khẩu thành công.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ChangedSuccessfully = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Đổi mật khẩu thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnLuu.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
