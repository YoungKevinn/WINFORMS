using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public class FormQuanLyMon : Form
    {
        // ====== UI controls ======
        private Panel panelTitle;
        private Label lblTitle;

        private Panel panelInput;     // chứa label + textbox + combobox
        private Label lblMaMon;
        private Label lblTenMon;
        private Label lblLoai;
        private Label lblDanhMuc;
        private Label lblDonGia;

        private TextBox txtMaMon;
        private TextBox txtTenMon;
        private ComboBox cboLoai;     // Thức ăn / Thức uống
        private ComboBox cboDanhMuc;
        private NumericUpDown nudDonGia;

        private Panel panelButtons;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnHuy;
        private Button btnSapXep;      // nút sắp xếp theo ĐƠN GIÁ
        private bool _sortGiaAsc = true; // true: bé -> lớn, false: lớn -> bé

        private DataGridView dgvMon;
        private readonly BindingSource _bs = new BindingSource();

        // ====== dữ liệu giả lập (sau này thay bằng API) ======
        private readonly List<MonViewModel> _monData = new();

        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;

        public FormQuanLyMon()
        {
            Text = "Quản lý món";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 600);
            Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            BackColor = Color.White;

            BuildLayout();
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

        // ================== BUILD UI ==================
        private void BuildLayout()
        {
            // --- TOP: tiêu đề ---
            panelTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
            };

            lblTitle = new Label
            {
                Text = "QUẢN LÝ MÓN",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panelTitle.Controls.Add(lblTitle);

            // --- panelInput: 2 hàng nhập liệu ---
            panelInput = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            int leftLabel1 = 40;
            int leftControl1 = 120;
            int leftLabel2 = 380;
            int leftControl2 = 460;
            int topRow1 = 20;
            int topRow2 = 60;
            int textboxWidth = 260;

            // Mã món
            lblMaMon = new Label
            {
                Text = "Mã món:",
                AutoSize = true,
                Location = new Point(leftLabel1, topRow1 + 5)
            };
            txtMaMon = new TextBox
            {
                Location = new Point(leftControl1, topRow1),
                Width = textboxWidth
            };

            // Tên món
            lblTenMon = new Label
            {
                Text = "Tên món:",
                AutoSize = true,
                Location = new Point(leftLabel2, topRow1 + 5)
            };
            txtTenMon = new TextBox
            {
                Location = new Point(leftControl2, topRow1),
                Width = textboxWidth + 80
            };

            // Loại
            lblLoai = new Label
            {
                Text = "Loại:",
                AutoSize = true,
                Location = new Point(leftLabel1, topRow2 + 5)
            };
            cboLoai = new ComboBox
            {
                Location = new Point(leftControl1, topRow2),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboLoai.Items.AddRange(new object[] { "Thức ăn", "Thức uống" });

            // Danh mục
            lblDanhMuc = new Label
            {
                Text = "Danh mục:",
                AutoSize = true,
                Location = new Point(leftLabel2, topRow2 + 5)
            };
            cboDanhMuc = new ComboBox
            {
                Location = new Point(leftControl2, topRow2),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // tạm: danh mục mẫu
            cboDanhMuc.Items.AddRange(new object[] { "Món chính", "Món phụ", "Cafe", "Nước ngọt" });

            // Đơn giá
            lblDonGia = new Label
            {
                Text = "Đơn giá:",
                AutoSize = true,
                Location = new Point(leftControl2 + 240, topRow2 + 5)
            };
            nudDonGia = new NumericUpDown
            {
                Location = new Point(leftControl2 + 300, topRow2),
                Width = 120,
                Minimum = 0,
                Maximum = 100000000,
                Increment = 1000,
                ThousandsSeparator = true
            };

            panelInput.Controls.AddRange(new Control[]
            {
                lblMaMon, txtMaMon,
                lblTenMon, txtTenMon,
                lblLoai, cboLoai,
                lblDanhMuc, cboDanhMuc,
                lblDonGia, nudDonGia
            });

            // --- panelButtons: Thêm / Sửa / Xóa / Lưu / Hủy / Sắp xếp ---
            panelButtons = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.WhiteSmoke
            };

            btnThem = new Button
            {
                Text = "Thêm",
                Width = 80,
                Height = 30,
                Location = new Point(40, 10)
            };
            btnSua = new Button
            {
                Text = "Sửa",
                Width = 80,
                Height = 30,
                Location = new Point(140, 10)
            };
            btnXoa = new Button
            {
                Text = "Xóa",
                Width = 80,
                Height = 30,
                Location = new Point(240, 10)
            };
            btnLuu = new Button
            {
                Text = "Lưu",
                Width = 80,
                Height = 30,
                Location = new Point(340, 10)
            };
            btnHuy = new Button
            {
                Text = "Hủy",
                Width = 80,
                Height = 30,
                Location = new Point(440, 10)
            };

            btnSapXep = new Button
            {
                Text = "Sắp xếp giá ↑",   // mặc định: bé -> lớn
                Width = 110,
                Height = 30,
                Location = new Point(540, 10)
            };

            panelButtons.Controls.AddRange(new Control[]
            {
                btnThem, btnSua, btnXoa, btnLuu, btnHuy, btnSapXep
            });

            // --- DataGridView món ---
            dgvMon = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };

            dgvMon.AutoGenerateColumns = false;
            dgvMon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(MonViewModel.MaMon),
                HeaderText = "Mã món",
                FillWeight = 15
            });
            dgvMon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(MonViewModel.TenMon),
                HeaderText = "Tên món",
                FillWeight = 35
            });
            dgvMon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(MonViewModel.Loai),
                HeaderText = "Loại",
                FillWeight = 15
            });
            dgvMon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(MonViewModel.DanhMuc),
                HeaderText = "Danh mục",
                FillWeight = 20
            });
            dgvMon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(MonViewModel.DonGia),
                HeaderText = "Đơn giá",
                FillWeight = 15,
                DefaultCellStyle = { Format = "N0" }
            });

            dgvMon.DataSource = _bs;

            // THỨ TỰ ADD: dưới → trên
            Controls.Add(dgvMon);
            Controls.Add(panelButtons);
            Controls.Add(panelInput);
            Controls.Add(panelTitle);
        }

        private void AttachEvents()
        {
            Load += FormQuanLyMon_Load;

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += BtnHuy_Click;
            btnSapXep.Click += BtnSapXep_Click;    // sự kiện sắp xếp

            dgvMon.SelectionChanged += DgvMon_SelectionChanged;
        }

        // ================== EVENT HANDLERS ==================
        private void FormQuanLyMon_Load(object sender, EventArgs e)
        {
            // dữ liệu mẫu cho dễ thấy
            _monData.AddRange(new[]
            {
                new MonViewModel{ MaMon="M01", TenMon="Phở bò", Loai="Thức ăn", DanhMuc="Món chính", DonGia=30000 },
                new MonViewModel{ MaMon="M02", TenMon="Cà phê sữa", Loai="Thức uống", DanhMuc="Cafe", DonGia=20000 }
            });

            _bs.DataSource = _monData.ToList(); // copy để binding
            if (cboLoai.Items.Count > 0) cboLoai.SelectedIndex = 0;
            if (cboDanhMuc.Items.Count > 0) cboDanhMuc.SelectedIndex = 0;

            SetEditMode(EditMode.None);
            LoadCurrentRowToInputs();
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            SetEditMode(EditMode.Add);
            ClearInputs();
            txtMaMon.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (_bs.Current == null) return;
            SetEditMode(EditMode.Edit);
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (_bs.Current == null) return;

            var current = (MonViewModel)_bs.Current;
            if (MessageBox.Show($"Xóa món '{current.TenMon}'?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                return;

            _monData.RemoveAll(m => m.MaMon == current.MaMon);
            RefreshBinding();
            SetEditMode(EditMode.None);
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (_mode == EditMode.None) return;

            // Validate đơn giản
            if (string.IsNullOrWhiteSpace(txtMaMon.Text))
            {
                MessageBox.Show("Mã món không được trống!");
                txtMaMon.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Tên món không được trống!");
                txtTenMon.Focus();
                return;
            }

            string ma = txtMaMon.Text.Trim();
            string ten = txtTenMon.Text.Trim();
            string loai = cboLoai.SelectedItem?.ToString() ?? "";
            string dm = cboDanhMuc.SelectedItem?.ToString() ?? "";
            decimal gia = nudDonGia.Value;

            if (_mode == EditMode.Add)
            {
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
            else if (_mode == EditMode.Edit && _bs.Current is MonViewModel cur)
            {
                var model = _monData.First(m => m.MaMon == cur.MaMon);
                model.TenMon = ten;
                model.Loai = loai;
                model.DanhMuc = dm;
                model.DonGia = gia;
            }

            RefreshBinding();
            SetEditMode(EditMode.None);
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            SetEditMode(EditMode.None);
            LoadCurrentRowToInputs();
        }

        private void DgvMon_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode != EditMode.None) return;
            LoadCurrentRowToInputs();
        }

        // ===== nút sắp xếp theo ĐƠN GIÁ (bé -> lớn <-> lớn -> bé) =====
        private void BtnSapXep_Click(object sender, EventArgs e)
        {
            if (_monData.Count == 0) return;

            if (_sortGiaAsc)
            {
                // bé -> lớn
                _monData.Sort((a, b) => a.DonGia.CompareTo(b.DonGia));
                btnSapXep.Text = "Sắp xếp giá ↓";
            }
            else
            {
                // lớn -> bé
                _monData.Sort((a, b) => b.DonGia.CompareTo(a.DonGia));
                btnSapXep.Text = "Sắp xếp giá ↑";
            }

            _sortGiaAsc = !_sortGiaAsc;
            RefreshBinding();
        }

        // ================== HELPER ==================
        private void SetEditMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.None;

            txtMaMon.ReadOnly = (mode == EditMode.Edit); // sửa không cho đổi mã
            txtTenMon.ReadOnly = !editing;
            cboLoai.Enabled = editing;
            cboDanhMuc.Enabled = editing;
            nudDonGia.Enabled = editing;

            btnThem.Enabled = !editing;
            btnSua.Enabled = !editing && _bs.Count > 0;
            btnXoa.Enabled = !editing && _bs.Count > 0;

            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
            btnSapXep.Enabled = !editing;   // đang sửa thì không cho sort
        }

        private void ClearInputs()
        {
            txtMaMon.Text = "";
            txtTenMon.Text = "";
            if (cboLoai.Items.Count > 0) cboLoai.SelectedIndex = 0;
            if (cboDanhMuc.Items.Count > 0) cboDanhMuc.SelectedIndex = 0;
            nudDonGia.Value = 0;
        }

        private void LoadCurrentRowToInputs()
        {
            if (_bs.Current is not MonViewModel cur) return;

            txtMaMon.Text = cur.MaMon;
            txtTenMon.Text = cur.TenMon;
            cboLoai.SelectedItem = cur.Loai;
            cboDanhMuc.SelectedItem = cur.DanhMuc;
            nudDonGia.Value = cur.DonGia;
        }

        private void RefreshBinding()
        {
            _bs.DataSource = _monData.ToList();  // refresh list lên grid
        }
    }
}
