using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormQuanLyMon : Form
    {
        // Services
        private readonly ThucAnService _thucAnService = new();
        private readonly ThucUongService _thucUongService = new();
        private readonly DanhMucService _danhMucService = new();

        // Data Lists
        private List<DanhMucDto> _dsDanhMuc = new();
        private List<MonHienThiViewModel> _dsMonHienThi = new();

        // State
        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;
        private BindingSource _bs = new BindingSource();

        public FormQuanLyMon()
        {
            InitializeComponent();

            // Cấu hình Combobox Loại
            cboLoai.Items.Add("Thức ăn");
            cboLoai.Items.Add("Thức uống");
            cboLoai.SelectedIndex = 0;

            // Events
            Load += FormQuanLyMon_Load;
            btnThem.Click += (s, e) => SetMode(EditMode.Add);
            btnSua.Click += (s, e) => SetMode(EditMode.Edit);
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += (s, e) => SetMode(EditMode.None);

            dgvMon.SelectionChanged += DgvMon_SelectionChanged;
        }

        private async void FormQuanLyMon_Load(object sender, EventArgs e)
        {
            await LoadDanhMuc(); // Tải danh mục trước để fill combobox
            await LoadData();    // Tải món
            SetMode(EditMode.None);
        }

        private async Task LoadDanhMuc()
        {
            try
            {
                _dsDanhMuc = await _danhMucService.GetAllAsync();
                cboDanhMuc.DataSource = _dsDanhMuc;
                cboDanhMuc.DisplayMember = "Ten";
                cboDanhMuc.ValueMember = "Id";
            }
            catch { /* Bỏ qua lỗi danh mục nếu chưa có */ }
        }

        private async Task LoadData()
        {
            try
            {
                var thucAns = await _thucAnService.GetAllAsync();
                var thucUongs = await _thucUongService.GetAllAsync();

                _dsMonHienThi.Clear();

                // Gộp Thức ăn và Thức uống vào chung 1 list để hiển thị
                _dsMonHienThi.AddRange(thucAns.Select(x => new MonHienThiViewModel
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    DonGia = x.DonGia,
                    Loai = "Thức ăn",
                    DanhMucId = x.DanhMucId,
                    TenDanhMuc = GetTenDanhMuc(x.DanhMucId)
                }));

                _dsMonHienThi.AddRange(thucUongs.Select(x => new MonHienThiViewModel
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    DonGia = x.DonGia,
                    Loai = "Thức uống",
                    DanhMucId = x.DanhMucId,
                    TenDanhMuc = GetTenDanhMuc(x.DanhMucId)
                }));

                _bs.DataSource = _dsMonHienThi;
                dgvMon.DataSource = _bs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private string GetTenDanhMuc(int id)
        {
            return _dsDanhMuc.FirstOrDefault(d => d.Id == id)?.Ten ?? "Khác";
        }

        private void DgvMon_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode != EditMode.None) return;
            if (dgvMon.CurrentRow == null) return;

            var mon = dgvMon.CurrentRow.DataBoundItem as MonHienThiViewModel;
            if (mon != null)
            {
                txtTenMon.Text = mon.Ten;
                nudDonGia.Value = mon.DonGia;
                cboLoai.SelectedItem = mon.Loai;

                // Chọn danh mục tương ứng
                if (mon.DanhMucId > 0) cboDanhMuc.SelectedValue = mon.DanhMucId;
            }
        }

        private void SetMode(EditMode mode)
        {
            _mode = mode;
            bool isEditing = mode != EditMode.None;

            txtTenMon.Enabled = isEditing;
            nudDonGia.Enabled = isEditing;
            cboLoai.Enabled = isEditing;
            cboDanhMuc.Enabled = isEditing;

            // Logic khóa nút khi sửa: không được đổi Loại (vì khác bảng DB)
            if (mode == EditMode.Edit) cboLoai.Enabled = false;

            btnThem.Enabled = !isEditing;
            btnSua.Enabled = !isEditing;
            btnXoa.Enabled = !isEditing;
            btnLuu.Enabled = isEditing;
            btnHuy.Enabled = isEditing;
            dgvMon.Enabled = !isEditing;

            if (mode == EditMode.Add)
            {
                txtTenMon.Clear();
                nudDonGia.Value = 0;
                if (cboDanhMuc.Items.Count > 0) cboDanhMuc.SelectedIndex = 0;
                txtTenMon.Focus();
            }
        }

        private async void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Vui lòng nhập tên món.");
                return;
            }

            string loai = cboLoai.SelectedItem.ToString();
            int danhMucId = (int)cboDanhMuc.SelectedValue;

            try
            {
                if (loai == "Thức ăn")
                {
                    var dto = new ThucAnDto
                    {
                        Ten = txtTenMon.Text,
                        DonGia = nudDonGia.Value,
                        DanhMucId = danhMucId,
                        DangHoatDong = true
                    };

                    if (_mode == EditMode.Add)
                        await _thucAnService.CreateAsync(dto);
                    else
                    {
                        var currentId = (dgvMon.CurrentRow.DataBoundItem as MonHienThiViewModel).Id;
                        await _thucAnService.UpdateAsync(currentId, dto);
                    }
                }
                else // Thức uống
                {
                    var dto = new ThucUongDto
                    {
                        Ten = txtTenMon.Text,
                        DonGia = nudDonGia.Value,
                        DanhMucId = danhMucId,
                        DangHoatDong = true
                    };

                    if (_mode == EditMode.Add)
                        await _thucUongService.CreateAsync(dto);
                    else
                    {
                        var currentId = (dgvMon.CurrentRow.DataBoundItem as MonHienThiViewModel).Id;
                        await _thucUongService.UpdateAsync(currentId, dto);
                    }
                }

                MessageBox.Show("Lưu thành công!");
                await LoadData();
                SetMode(EditMode.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu: " + ex.Message);
            }
        }

        private async void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvMon.CurrentRow?.DataBoundItem is MonHienThiViewModel mon)
            {
                if (MessageBox.Show($"Xóa {mon.Ten}?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        if (mon.Loai == "Thức ăn")
                            await _thucAnService.DeleteAsync(mon.Id);
                        else
                            await _thucUongService.DeleteAsync(mon.Id);

                        await LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa: " + ex.Message);
                    }
                }
            }
        }

        // Class phụ để hiển thị lên Grid
        private class MonHienThiViewModel
        {
            public int Id { get; set; }
            public string Ten { get; set; }
            public decimal DonGia { get; set; }
            public string Loai { get; set; } // "Thức ăn" or "Thức uống"
            public int DanhMucId { get; set; }
            public string TenDanhMuc { get; set; }
        }
    }
}