using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormQuanLyMon : Form
    {
        // ====== dữ liệu giả lập (sau này thay bằng API) ======
        private readonly List<MonViewModel> _monData = new();

        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;

        private bool _sortGiaAsc = true; // true: bé->lớn, false: lớn->bé

        // chặn vòng lặp khi LoadCurrentRowToInputs() set lại giá trị
        private bool _loadingInputs = false;

        // lưu mã gốc khi sửa (cho phép đổi mã)
        private string? _editingOriginalMa = null;

        public FormQuanLyMon()
        {
            InitializeComponent();
            AttachEvents();
        }

        // ====== MODEL VIEW đơn giản ======
        private class MonViewModel
        {
            public string MaMon { get; set; } = "";
            public string TenMon { get; set; } = "";
            public string Loai { get; set; } = "";
            public string DanhMuc { get; set; } = "";
            public decimal DonGia { get; set; }
        }

        private void AttachEvents()
        {
            Load += FormQuanLyMon_Load;

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += BtnHuy_Click;
            btnSapXep.Click += BtnSapXep_Click;

            dgvMon.SelectionChanged += DgvMon_SelectionChanged;

            // ✅ “Không xám nhưng không cho đổi” ở mode None
            cboLoai.SelectionChangeCommitted += CboLoai_SelectionChangeCommitted;
            cboDanhMuc.SelectionChangeCommitted += CboDanhMuc_SelectionChangeCommitted;
            nudDonGia.ValueChanged += NudDonGia_ValueChanged;
        }

        private void FormQuanLyMon_Load(object? sender, EventArgs e)
        {
            _monData.Clear();
            _monData.AddRange(new[]
            {
                new MonViewModel{ MaMon="M02", TenMon="Cà phê sữa", Loai="Thức uống", DanhMuc="Cafe",     DonGia=20000 },
                new MonViewModel{ MaMon="M01", TenMon="Phở bò",     Loai="Thức ăn",   DanhMuc="Món chính", DonGia=30000 },
                new MonViewModel{ MaMon="M06", TenMon="Phở tái",    Loai="Thức ăn",   DanhMuc="Món chính", DonGia=100000 },
            });

            RefreshBinding();

            if (cboLoai.Items.Count > 0 && cboLoai.SelectedIndex < 0) cboLoai.SelectedIndex = 0;
            if (cboDanhMuc.Items.Count > 0 && cboDanhMuc.SelectedIndex < 0) cboDanhMuc.SelectedIndex = 0;

            if (bsMon.Count > 0) bsMon.Position = 0;

            SetEditMode(EditMode.None);
            LoadCurrentRowToInputs();
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            _editingOriginalMa = null;
            SetEditMode(EditMode.Add);
            ClearInputs();
            txtMaMon.Focus();
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (bsMon.Current is not MonViewModel cur) return;

            _editingOriginalMa = cur.MaMon;
            SetEditMode(EditMode.Edit);

            // focus vào tên món cho dễ sửa
            txtTenMon.Focus();
            txtTenMon.SelectionStart = txtTenMon.TextLength;
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (bsMon.Current is not MonViewModel current) return;

            if (MessageBox.Show($"Xóa món '{current.TenMon}'?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                return;

            _monData.RemoveAll(m => m.MaMon.Equals(current.MaMon, StringComparison.OrdinalIgnoreCase));
            RefreshBinding();

            if (bsMon.Count > 0) bsMon.Position = 0;

            SetEditMode(EditMode.None);
            LoadCurrentRowToInputs();
        }

        private void BtnLuu_Click(object? sender, EventArgs e)
        {
            if (_mode == EditMode.None) return;

            string ma = (txtMaMon.Text ?? "").Trim();
            string ten = (txtTenMon.Text ?? "").Trim();
            string loai = cboLoai.SelectedItem?.ToString() ?? "";
            string dm = cboDanhMuc.SelectedItem?.ToString() ?? "";
            decimal gia = nudDonGia.Value;

            if (string.IsNullOrWhiteSpace(ma))
            {
                MessageBox.Show("Mã món không được trống!");
                txtMaMon.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Tên món không được trống!");
                txtTenMon.Focus();
                return;
            }

            if (_mode == EditMode.Add)
            {
                // check trùng mã
                if (_monData.Any(m => m.MaMon.Equals(ma, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Mã món đã tồn tại!");
                    txtMaMon.Focus();
                    return;
                }

                _monData.Add(new MonViewModel
                {
                    MaMon = ma,
                    TenMon = ten,
                    Loai = loai,
                    DanhMuc = dm,
                    DonGia = gia
                });
            }
            else if (_mode == EditMode.Edit)
            {
                if (string.IsNullOrWhiteSpace(_editingOriginalMa))
                {
                    MessageBox.Show("Không xác định được món đang sửa!");
                    return;
                }

                // nếu đổi mã -> check trùng (ngoại trừ chính nó)
                bool maChanged = !ma.Equals(_editingOriginalMa, StringComparison.OrdinalIgnoreCase);
                if (maChanged && _monData.Any(m => m.MaMon.Equals(ma, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Mã món đã tồn tại!");
                    txtMaMon.Focus();
                    return;
                }

                var model = _monData.FirstOrDefault(m => m.MaMon.Equals(_editingOriginalMa, StringComparison.OrdinalIgnoreCase));
                if (model == null)
                {
                    MessageBox.Show("Món cần sửa không còn tồn tại!");
                    return;
                }

                model.MaMon = ma;
                model.TenMon = ten;
                model.Loai = loai;
                model.DanhMuc = dm;
                model.DonGia = gia;

                _editingOriginalMa = ma; // cập nhật lại mã gốc
            }

            RefreshBinding(keepMa: ma);

            SetEditMode(EditMode.None);
            LoadCurrentRowToInputs();
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            SetEditMode(EditMode.None);
            LoadCurrentRowToInputs();
        }

        private void DgvMon_SelectionChanged(object? sender, EventArgs e)
        {
            if (_mode != EditMode.None) return;
            LoadCurrentRowToInputs();
        }

        private void BtnSapXep_Click(object? sender, EventArgs e)
        {
            if (_monData.Count == 0) return;

            if (_sortGiaAsc)
            {
                _monData.Sort((a, b) => a.DonGia.CompareTo(b.DonGia));
                btnSapXep.Text = "Sắp xếp giá ↓";
            }
            else
            {
                _monData.Sort((a, b) => b.DonGia.CompareTo(a.DonGia));
                btnSapXep.Text = "Sắp xếp giá ↑";
            }

            _sortGiaAsc = !_sortGiaAsc;
            RefreshBinding();
        }

        // ====== ✅ “Không xám nhưng không cho đổi” ở mode None ======
        private void CboLoai_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            if (_loadingInputs) return;
            if (_mode == EditMode.None) LoadCurrentRowToInputs();
        }

        private void CboDanhMuc_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            if (_loadingInputs) return;
            if (_mode == EditMode.None) LoadCurrentRowToInputs();
        }

        private void NudDonGia_ValueChanged(object? sender, EventArgs e)
        {
            if (_loadingInputs) return;
            if (_mode == EditMode.None) LoadCurrentRowToInputs();
        }

        // ================== HELPER ==================
        private void SetEditMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.None;

            // TextBox: dùng ReadOnly để không bị xám
            txtMaMon.ReadOnly = !editing;   // ✅ cho phép sửa mã luôn (Add/Edit)
            txtTenMon.ReadOnly = !editing;

            // ComboBox & Numeric: luôn enabled để không bị xám
            // nhưng mode None đổi sẽ bị trả lại (SelectionChangeCommitted / ValueChanged)
            cboLoai.Enabled = true;
            cboDanhMuc.Enabled = true;

            nudDonGia.Enabled = true;
            nudDonGia.ReadOnly = !editing; // chặn gõ trực tiếp khi xem

            btnThem.Enabled = !editing;
            btnSua.Enabled = !editing && bsMon.Count > 0;
            btnXoa.Enabled = !editing && bsMon.Count > 0;

            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            btnSapXep.Enabled = !editing;

            // khi đang sửa, khóa selection grid để khỏi “nhảy dòng”
            dgvMon.Enabled = !editing;
        }

        private void ClearInputs()
        {
            _loadingInputs = true;
            try
            {
                txtMaMon.Text = "";
                txtTenMon.Text = "";

                if (cboLoai.Items.Count > 0) cboLoai.SelectedIndex = 0;
                if (cboDanhMuc.Items.Count > 0) cboDanhMuc.SelectedIndex = 0;

                nudDonGia.Value = 0;
            }
            finally
            {
                _loadingInputs = false;
            }
        }

        private void LoadCurrentRowToInputs()
        {
            _loadingInputs = true;
            try
            {
                if (bsMon.Current is not MonViewModel cur)
                {
                    ClearInputs();
                    return;
                }

                txtMaMon.Text = cur.MaMon;
                txtTenMon.Text = cur.TenMon;

                cboLoai.SelectedItem = cur.Loai;
                if (cboLoai.SelectedIndex < 0 && cboLoai.Items.Count > 0)
                    cboLoai.SelectedIndex = 0;

                cboDanhMuc.SelectedItem = cur.DanhMuc;
                if (cboDanhMuc.SelectedIndex < 0 && cboDanhMuc.Items.Count > 0)
                    cboDanhMuc.SelectedIndex = 0;

                decimal gia = cur.DonGia;
                if (gia < nudDonGia.Minimum) gia = nudDonGia.Minimum;
                if (gia > nudDonGia.Maximum) gia = nudDonGia.Maximum;
                nudDonGia.Value = gia;
            }
            finally
            {
                _loadingInputs = false;
            }
        }

        private void RefreshBinding(string? keepMa = null)
        {
            keepMa ??= (bsMon.Current as MonViewModel)?.MaMon;

            bsMon.DataSource = null;
            bsMon.DataSource = _monData;

            if (!string.IsNullOrWhiteSpace(keepMa))
            {
                int idx = _monData.FindIndex(x => x.MaMon.Equals(keepMa, StringComparison.OrdinalIgnoreCase));
                if (idx >= 0) bsMon.Position = idx;
            }
        }
    }
}
