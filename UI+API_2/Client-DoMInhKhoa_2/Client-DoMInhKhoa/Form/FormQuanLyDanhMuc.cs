using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormQuanLyDanhMuc : Form
    {
        private readonly List<DanhMucViewModel> _data = new();

        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;

        private bool _loadingInputs = false;
        private string? _editingOriginalMa = null;

        public FormQuanLyDanhMuc()
        {
            InitializeComponent();
            AttachEvents();
        }

        private class DanhMucViewModel
        {
            public string MaDanhMuc { get; set; } = "";
            public string TenDanhMuc { get; set; } = "";
            public string Loai { get; set; } = "";
            public string GhiChu { get; set; } = "";
        }

        private void AttachEvents()
        {
            Load += FormQuanLyDanhMuc_Load;

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += BtnHuy_Click;

            dgvDanhMuc.SelectionChanged += DgvDanhMuc_SelectionChanged;

            // ✅ không xám nhưng mode None sẽ tự trả lại
            cboLoai.SelectionChangeCommitted += CboLoai_SelectionChangeCommitted;
        }

        private void FormQuanLyDanhMuc_Load(object? sender, EventArgs e)
        {
            _data.Clear();

            _data.AddRange(new[]
            {
                new DanhMucViewModel{ MaDanhMuc="DM01", TenDanhMuc="Món chính",      Loai="Thức ăn",   GhiChu="Các món no bụng" },
                new DanhMucViewModel{ MaDanhMuc="DM02", TenDanhMuc="Món phụ",        Loai="Thức ăn",   GhiChu="Ăn kèm" },
                new DanhMucViewModel{ MaDanhMuc="DM03", TenDanhMuc="Cà phê",         Loai="Thức uống", GhiChu="Cafe nóng / đá" },
                new DanhMucViewModel{ MaDanhMuc="DM04", TenDanhMuc="Nước giải khát", Loai="Thức uống", GhiChu="Nước đóng chai, nước ngọt" }
            });

            RefreshBinding();

            if (cboLoai.Items.Count > 0 && cboLoai.SelectedIndex < 0)
                cboLoai.SelectedIndex = 0;

            if (bsDanhMuc.Count > 0) bsDanhMuc.Position = 0;

            SetEditMode(EditMode.None);
            LoadCurrentToInputs();
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            _editingOriginalMa = null;
            SetEditMode(EditMode.Add);
            ClearInputs();
            txtMaDm.Focus();
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (bsDanhMuc.Current is not DanhMucViewModel cur) return;

            _editingOriginalMa = cur.MaDanhMuc;
            SetEditMode(EditMode.Edit);

            txtTenDm.Focus();
            txtTenDm.SelectionStart = txtTenDm.TextLength;
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (bsDanhMuc.Current is not DanhMucViewModel cur) return;

            if (MessageBox.Show($"Xóa danh mục '{cur.TenDanhMuc}'?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                return;

            _data.RemoveAll(d => d.MaDanhMuc.Equals(cur.MaDanhMuc, StringComparison.OrdinalIgnoreCase));
            RefreshBinding();

            if (bsDanhMuc.Count > 0) bsDanhMuc.Position = 0;

            SetEditMode(EditMode.None);
            LoadCurrentToInputs();
        }

        private void BtnLuu_Click(object? sender, EventArgs e)
        {
            if (_mode == EditMode.None) return;

            string ma = (txtMaDm.Text ?? "").Trim();
            string ten = (txtTenDm.Text ?? "").Trim();
            string loai = cboLoai.SelectedItem?.ToString() ?? "";
            string ghiChu = (txtGhiChu.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(ma))
            {
                MessageBox.Show("Mã danh mục không được trống!");
                txtMaDm.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Tên danh mục không được trống!");
                txtTenDm.Focus();
                return;
            }

            if (_mode == EditMode.Add)
            {
                if (_data.Any(d => d.MaDanhMuc.Equals(ma, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Mã danh mục đã tồn tại!");
                    txtMaDm.Focus();
                    return;
                }

                _data.Add(new DanhMucViewModel
                {
                    MaDanhMuc = ma,
                    TenDanhMuc = ten,
                    Loai = loai,
                    GhiChu = ghiChu
                });
            }
            else if (_mode == EditMode.Edit)
            {
                if (string.IsNullOrWhiteSpace(_editingOriginalMa))
                {
                    MessageBox.Show("Không xác định được danh mục đang sửa!");
                    return;
                }

                bool maChanged = !ma.Equals(_editingOriginalMa, StringComparison.OrdinalIgnoreCase);
                if (maChanged && _data.Any(d => d.MaDanhMuc.Equals(ma, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Mã danh mục đã tồn tại!");
                    txtMaDm.Focus();
                    return;
                }

                var model = _data.FirstOrDefault(d => d.MaDanhMuc.Equals(_editingOriginalMa, StringComparison.OrdinalIgnoreCase));
                if (model == null)
                {
                    MessageBox.Show("Danh mục cần sửa không còn tồn tại!");
                    return;
                }

                model.MaDanhMuc = ma;
                model.TenDanhMuc = ten;
                model.Loai = loai;
                model.GhiChu = ghiChu;

                _editingOriginalMa = ma;
            }

            RefreshBinding(keepMa: ma);

            SetEditMode(EditMode.None);
            LoadCurrentToInputs();
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            SetEditMode(EditMode.None);
            LoadCurrentToInputs();
        }

        private void DgvDanhMuc_SelectionChanged(object? sender, EventArgs e)
        {
            if (_mode != EditMode.None) return;
            LoadCurrentToInputs();
        }

        private void CboLoai_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            if (_loadingInputs) return;
            if (_mode == EditMode.None) LoadCurrentToInputs();
        }

        // ================== HELPER ==================
        private void SetEditMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.None;

            txtMaDm.ReadOnly = !editing;   // ✅ cho sửa mã luôn (Add/Edit)
            txtTenDm.ReadOnly = !editing;
            txtGhiChu.ReadOnly = !editing;

            // ComboBox luôn enabled để không bị xám, mode None thì tự trả lại
            cboLoai.Enabled = true;

            btnThem.Enabled = !editing;
            btnSua.Enabled = !editing && bsDanhMuc.Count > 0;
            btnXoa.Enabled = !editing && bsDanhMuc.Count > 0;

            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;

            dgvDanhMuc.Enabled = !editing;
        }

        private void ClearInputs()
        {
            _loadingInputs = true;
            try
            {
                txtMaDm.Text = "";
                txtTenDm.Text = "";
                txtGhiChu.Text = "";
                if (cboLoai.Items.Count > 0) cboLoai.SelectedIndex = 0;
            }
            finally
            {
                _loadingInputs = false;
            }
        }

        private void LoadCurrentToInputs()
        {
            _loadingInputs = true;
            try
            {
                if (bsDanhMuc.Current is not DanhMucViewModel cur)
                {
                    ClearInputs();
                    return;
                }

                txtMaDm.Text = cur.MaDanhMuc;
                txtTenDm.Text = cur.TenDanhMuc;

                cboLoai.SelectedItem = cur.Loai;
                if (cboLoai.SelectedIndex < 0 && cboLoai.Items.Count > 0)
                    cboLoai.SelectedIndex = 0;

                txtGhiChu.Text = cur.GhiChu;
            }
            finally
            {
                _loadingInputs = false;
            }
        }

        private void RefreshBinding(string? keepMa = null)
        {
            keepMa ??= (bsDanhMuc.Current as DanhMucViewModel)?.MaDanhMuc;

            bsDanhMuc.DataSource = null;
            bsDanhMuc.DataSource = _data;

            if (!string.IsNullOrWhiteSpace(keepMa))
            {
                int idx = _data.FindIndex(x => x.MaDanhMuc.Equals(keepMa, StringComparison.OrdinalIgnoreCase));
                if (idx >= 0) bsDanhMuc.Position = idx;
            }
        }
    }
}
