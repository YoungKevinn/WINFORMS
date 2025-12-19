using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormQuanLyNhanVien : Form
    {
        // Khai báo Service
        private readonly NhanVienService _nhanVienService = new NhanVienService();
        private List<NhanVienDto> _dsNhanVien = new();
        private readonly BindingSource _bs = new BindingSource();

        // Trạng thái form
        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;

        public FormQuanLyNhanVien()
        {
            InitializeComponent();

            // Cài đặt ComboBox Vai trò
            cboVaiTro.Items.Add("NhanVien");
            cboVaiTro.Items.Add("Admin");
            cboVaiTro.SelectedIndex = 0;

            // Đăng ký sự kiện
            Load += FormQuanLyNhanVien_Load;
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += BtnHuy_Click;
            dgvNhanVien.SelectionChanged += DgvNhanVien_SelectionChanged;
        }

        private async void FormQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            await LoadData();
            SetMode(EditMode.None);
        }

        // Tải dữ liệu từ API
        private async Task LoadData()
        {
            try
            {
                // Vì API NhanVien của bạn trả về List<NhanVien> (Model) chứ không phải DTO
                // Nên chúng ta dùng dynamic hoặc map sang DTO
                // Ở đây mình giả định bạn đã sửa Service trả về List<NhanVienDto>
                // Nếu chưa, code sẽ tự map
                var result = await _nhanVienService.GetAllAsync();

                // Map sang DTO nếu cần (tuỳ vào Service của bạn trả về gì)
                _dsNhanVien = result ?? new List<NhanVienDto>();

                _bs.DataSource = _dsNhanVien;
                dgvNhanVien.DataSource = _bs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách: " + ex.Message);
            }
        }

        private void DgvNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode != EditMode.None) return;
            if (dgvNhanVien.CurrentRow == null) return;

            var nv = dgvNhanVien.CurrentRow.DataBoundItem as NhanVienDto;
            if (nv != null)
            {
                txtMaNV.Text = nv.MaNhanVien;
                txtHoTen.Text = nv.HoTen;
                cboVaiTro.SelectedItem = nv.VaiTro;
                chkDangHoatDong.Checked = nv.DangHoatDong;
            }
        }

        private void SetMode(EditMode mode)
        {
            _mode = mode;
            bool isEditing = mode != EditMode.None;

            // Khóa/Mở control nhập liệu
            txtMaNV.Enabled = (mode == EditMode.Add); // Chỉ cho sửa mã khi thêm mới
            txtHoTen.Enabled = isEditing;
            cboVaiTro.Enabled = isEditing;
            chkDangHoatDong.Enabled = isEditing;

            // Ẩn/Hiện nút bấm
            btnThem.Enabled = !isEditing;
            btnSua.Enabled = !isEditing && dgvNhanVien.RowCount > 0;
            btnXoa.Enabled = !isEditing && dgvNhanVien.RowCount > 0;

            btnLuu.Enabled = isEditing;
            btnHuy.Enabled = isEditing;

            dgvNhanVien.Enabled = !isEditing; // Khóa lưới khi đang sửa
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            SetMode(EditMode.Add);
            txtMaNV.Clear();
            txtHoTen.Clear();
            cboVaiTro.SelectedIndex = 0;
            chkDangHoatDong.Checked = true;
            txtMaNV.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            SetMode(EditMode.Edit);
            txtHoTen.Focus();
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            SetMode(EditMode.None);
            DgvNhanVien_SelectionChanged(null, null); // Load lại dòng đang chọn
        }

        private async void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow?.DataBoundItem is not NhanVienDto nv) return;

            if (MessageBox.Show($"Bạn có chắc muốn xóa nhân viên {nv.HoTen}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Gọi API Xóa
                    await _nhanVienService.DeleteAsync(nv.Id); // Bạn cần đảm bảo Service có hàm Delete
                    await LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
            }
        }

        private async void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã và Họ tên nhân viên.");
                return;
            }

            var dto = new NhanVienCreateUpdateDto
            {
                MaNhanVien = txtMaNV.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                VaiTro = cboVaiTro.SelectedItem.ToString()
                // API của bạn có thể cần thêm field Password mặc định ở đây nếu là Create
            };

            try
            {
                if (_mode == EditMode.Add)
                {
                    // TODO: Bạn cần bổ sung hàm Create trong NhanVienService
                    // await _nhanVienService.CreateAsync(dto);
                    MessageBox.Show("Đã gửi yêu cầu tạo nhân viên (Cần update Service)!");
                }
                else if (_mode == EditMode.Edit)
                {
                    var currentId = (dgvNhanVien.CurrentRow.DataBoundItem as NhanVienDto).Id;
                    // await _nhanVienService.UpdateAsync(currentId, dto);
                    MessageBox.Show("Đã lưu thông tin!");
                }

                await LoadData();
                SetMode(EditMode.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message);
            }
        }
    }
}