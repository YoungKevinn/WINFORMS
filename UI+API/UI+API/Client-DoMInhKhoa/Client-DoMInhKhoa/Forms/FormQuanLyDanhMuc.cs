using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public class FormQuanLyDanhMuc : Form
    {
        // ====== UI controls ======
        private Panel panelTitle;
        private Label lblTitle;

        private Panel panelInput;
        private Label lblMaDm;
        private Label lblTenDm;
        private Label lblLoai;
        private Label lblGhiChu;

        private TextBox txtMaDm;
        private TextBox txtTenDm;
        private ComboBox cboLoai;      // Thức ăn / Thức uống
        private TextBox txtGhiChu;     // multiline

        private Panel panelButtons;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLuu;
        private Button btnHuy;

        private DataGridView dgvDanhMuc;
        private readonly BindingSource _bs = new BindingSource();

        // ====== dữ liệu giả lập (sau này thay bằng API) ======
        private readonly List<DanhMucViewModel> _data = new();

        private enum EditMode { None, Add, Edit }
        private EditMode _mode = EditMode.None;

        public FormQuanLyDanhMuc()
        {
            Text = "Quản lý danh mục";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(900, 550);
            Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            BackColor = Color.White;

            BuildLayout();
            AttachEvents();
        }

        // ====== ViewModel đơn giản ======
        private class DanhMucViewModel
        {
            public string MaDanhMuc { get; set; } = "";
            public string TenDanhMuc { get; set; } = "";
            public string Loai { get; set; } = "";
            public string GhiChu { get; set; } = "";
        }

        // ================== BUILD UI ==================
        private void BuildLayout()
        {
            // ----- TITLE -----
            panelTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };

            lblTitle = new Label
            {
                Text = "QUẢN LÝ DANH MỤC",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            panelTitle.Controls.Add(lblTitle);

            // ----- INPUT -----
            panelInput = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            // Tăng khoảng cách giữa label và textbox để không bị đè chữ
            int leftLabel1 = 30;
            int leftControl1 = 140;      // trước là 120
            int leftLabel2 = 360;        // trước là 340
            int leftControl2 = 540;      // trước là 430 -> 540 cho rộng
            int topRow1 = 20;
            int topRow2 = 60;
            int textWidth = 200;

            // Mã danh mục
            lblMaDm = new Label
            {
                Text = "Mã danh mục:",
                AutoSize = true,
                Location = new Point(leftLabel1, topRow1 + 5)
            };
            txtMaDm = new TextBox
            {
                Location = new Point(leftControl1, topRow1),
                Width = textWidth
            };

            // Tên danh mục
            lblTenDm = new Label
            {
                Text = "Tên danh mục:",
                AutoSize = true,
                Location = new Point(leftLabel2, topRow1 + 5)
            };
            txtTenDm = new TextBox
            {
                Location = new Point(leftControl2, topRow1),
                Width = textWidth + 60
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
                Width = textWidth,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboLoai.Items.AddRange(new object[] { "Thức ăn", "Thức uống" });

            // Ghi chú
            lblGhiChu = new Label
            {
                Text = "Ghi chú:",
                AutoSize = true,
                Location = new Point(leftLabel2, topRow2 + 5)
            };
            txtGhiChu = new TextBox
            {
                Location = new Point(leftControl2, topRow2),
                Width = textWidth + 60,
                Height = 50,
                Multiline = true
            };

            panelInput.Controls.AddRange(new Control[]
            {
                lblMaDm, txtMaDm,
                lblTenDm, txtTenDm,
                lblLoai, cboLoai,
                lblGhiChu, txtGhiChu
            });

            // ----- BUTTONS -----
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
                Location = new Point(30, 10)
            };
            btnSua = new Button
            {
                Text = "Sửa",
                Width = 80,
                Height = 30,
                Location = new Point(120, 10)
            };
            btnXoa = new Button
            {
                Text = "Xóa",
                Width = 80,
                Height = 30,
                Location = new Point(210, 10)
            };
            btnLuu = new Button
            {
                Text = "Lưu",
                Width = 80,
                Height = 30,
                Location = new Point(300, 10)
            };
            btnHuy = new Button
            {
                Text = "Hủy",
                Width = 80,
                Height = 30,
                Location = new Point(390, 10)
            };

            panelButtons.Controls.AddRange(new Control[]
            {
                btnThem, btnSua, btnXoa, btnLuu, btnHuy
            });

            // ----- GRID -----
            dgvDanhMuc = new DataGridView
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

            dgvDanhMuc.AutoGenerateColumns = false;
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(DanhMucViewModel.MaDanhMuc),
                HeaderText = "Mã",
                FillWeight = 15
            });
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(DanhMucViewModel.TenDanhMuc),
                HeaderText = "Tên danh mục",
                FillWeight = 35
            });
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(DanhMucViewModel.Loai),
                HeaderText = "Loại",
                FillWeight = 20
            });
            dgvDanhMuc.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(DanhMucViewModel.GhiChu),
                HeaderText = "Ghi chú",
                FillWeight = 30
            });

            dgvDanhMuc.DataSource = _bs;

            // Thứ tự add: dưới → trên
            Controls.Add(dgvDanhMuc);
            Controls.Add(panelButtons);
            Controls.Add(panelInput);
            Controls.Add(panelTitle);
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
        }

        // ================== EVENT HANDLERS ==================
        private void FormQuanLyDanhMuc_Load(object sender, EventArgs e)
        {
            // dữ liệu mẫu cho dễ nhìn
            _data.AddRange(new[]
            {
                new DanhMucViewModel{ MaDanhMuc="DM01", TenDanhMuc="Món chính",      Loai="Thức ăn",   GhiChu="Các món no bụng" },
                new DanhMucViewModel{ MaDanhMuc="DM02", TenDanhMuc="Món phụ",        Loai="Thức ăn",   GhiChu="Ăn kèm" },
                new DanhMucViewModel{ MaDanhMuc="DM03", TenDanhMuc="Cà phê",         Loai="Thức uống", GhiChu="Cafe nóng / đá" },
                new DanhMucViewModel{ MaDanhMuc="DM04", TenDanhMuc="Nước giải khát", Loai="Thức uống", GhiChu="Nước đóng chai, nước ngọt" }
            });

            RefreshBinding();

            if (cboLoai.Items.Count > 0)
                cboLoai.SelectedIndex = 0;

            SetEditMode(EditMode.None);
            LoadCurrentToInputs();
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            SetEditMode(EditMode.Add);
            ClearInputs();
            txtMaDm.Focus();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (_bs.Current == null) return;
            SetEditMode(EditMode.Edit);
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (_bs.Current is not DanhMucViewModel cur) return;

            if (MessageBox.Show($"Xóa danh mục '{cur.TenDanhMuc}'?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                return;

            _data.RemoveAll(d => d.MaDanhMuc == cur.MaDanhMuc);
            RefreshBinding();
            SetEditMode(EditMode.None);
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (_mode == EditMode.None) return;

            // validate
            if (string.IsNullOrWhiteSpace(txtMaDm.Text))
            {
                MessageBox.Show("Mã danh mục không được trống!");
                txtMaDm.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenDm.Text))
            {
                MessageBox.Show("Tên danh mục không được trống!");
                txtTenDm.Focus();
                return;
            }

            string ma = txtMaDm.Text.Trim();
            string ten = txtTenDm.Text.Trim();
            string loai = cboLoai.SelectedItem?.ToString() ?? "";
            string ghiChu = txtGhiChu.Text.Trim();

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
            else if (_mode == EditMode.Edit && _bs.Current is DanhMucViewModel cur)
            {
                var dm = _data.First(d => d.MaDanhMuc == cur.MaDanhMuc);
                dm.TenDanhMuc = ten;
                dm.Loai = loai;
                dm.GhiChu = ghiChu;
            }

            RefreshBinding();
            SetEditMode(EditMode.None);
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            SetEditMode(EditMode.None);
            LoadCurrentToInputs();
        }

        private void DgvDanhMuc_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode != EditMode.None) return;
            LoadCurrentToInputs();
        }

        // ================== HELPER ==================
        private void SetEditMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.None;

            txtMaDm.ReadOnly = (mode == EditMode.Edit);
            txtTenDm.ReadOnly = !editing;
            cboLoai.Enabled = editing;
            txtGhiChu.ReadOnly = !editing;

            btnThem.Enabled = !editing;
            btnSua.Enabled = !editing && _bs.Count > 0;
            btnXoa.Enabled = !editing && _bs.Count > 0;

            btnLuu.Enabled = editing;
            btnHuy.Enabled = editing;
        }

        private void ClearInputs()
        {
            txtMaDm.Text = "";
            txtTenDm.Text = "";
            txtGhiChu.Text = "";
            if (cboLoai.Items.Count > 0)
                cboLoai.SelectedIndex = 0;
        }

        private void LoadCurrentToInputs()
        {
            if (_bs.Current is not DanhMucViewModel cur) return;

            txtMaDm.Text = cur.MaDanhMuc;
            txtTenDm.Text = cur.TenDanhMuc;
            cboLoai.SelectedItem = cur.Loai;
            txtGhiChu.Text = cur.GhiChu;
        }

        private void RefreshBinding()
        {
            _bs.DataSource = _data.ToList();
        }
    }
}
