using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormNhanVienEdit : Form
    {
        public string MaNhanVien => txtMa.Text.Trim();
        public string HoTen => txtHoTen.Text.Trim();
        public string VaiTro => (cboVaiTro.SelectedItem?.ToString() ?? "NhanVien").Trim();

        public FormNhanVienEdit(string title, string? maNv = null, string? hoTen = null, string? vaiTro = null)
        {
            InitializeComponent();

            Text = title;

            cboVaiTro.Items.Clear();
            cboVaiTro.Items.AddRange(new object[] { "NhanVien", "Admin" });
            cboVaiTro.SelectedIndex = 0;

            if (!string.IsNullOrWhiteSpace(maNv)) txtMa.Text = maNv;
            if (!string.IsNullOrWhiteSpace(hoTen)) txtHoTen.Text = hoTen;
            if (!string.IsNullOrWhiteSpace(vaiTro)) cboVaiTro.SelectedItem = vaiTro;

            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MaNhanVien) || string.IsNullOrWhiteSpace(HoTen))
            {
                MessageBox.Show("Vui lòng nhập đủ Mã NV và Họ tên.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MaNhanVien.Length < 3 || MaNhanVien.Length > 20 ||
                !Regex.IsMatch(MaNhanVien, @"^[A-Za-z0-9]+$"))
            {
                MessageBox.Show("Mã NV phải 3-20 ký tự và chỉ gồm chữ + số (không dấu/không khoảng trắng).",
                    "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (HoTen.Length > 100)
            {
                MessageBox.Show("Họ tên tối đa 100 ký tự.", "Sai định dạng",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
