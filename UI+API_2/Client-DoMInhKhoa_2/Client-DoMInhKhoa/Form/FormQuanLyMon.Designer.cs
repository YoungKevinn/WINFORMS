using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    partial class FormQuanLyMon
    {
        private IContainer components = null!;

        private Panel panelTitle = null!;
        private Label lblTitle = null!;

        private Panel panelInput = null!;
        private Label lblMaMon = null!;
        private Label lblTenMon = null!;
        private Label lblLoai = null!;
        private Label lblDanhMuc = null!;
        private Label lblDonGia = null!;

        private TextBox txtMaMon = null!;
        private TextBox txtTenMon = null!;
        private ComboBox cboLoai = null!;
        private ComboBox cboDanhMuc = null!;
        private NumericUpDown nudDonGia = null!;

        private Panel panelButtons = null!;
        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLuu = null!;
        private Button btnHuy = null!;
        private Button btnSapXep = null!;

        private DataGridView dgvMon = null!;
        private BindingSource bsMon = null!;

        private DataGridViewTextBoxColumn colMaMon = null!;
        private DataGridViewTextBoxColumn colTenMon = null!;
        private DataGridViewTextBoxColumn colLoai = null!;
        private DataGridViewTextBoxColumn colDanhMuc = null!;
        private DataGridViewTextBoxColumn colDonGia = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            bsMon = new BindingSource(components);

            panelTitle = new Panel();
            lblTitle = new Label();

            panelInput = new Panel();
            lblMaMon = new Label();
            txtMaMon = new TextBox();
            lblTenMon = new Label();
            txtTenMon = new TextBox();
            lblLoai = new Label();
            cboLoai = new ComboBox();
            lblDanhMuc = new Label();
            cboDanhMuc = new ComboBox();
            lblDonGia = new Label();
            nudDonGia = new NumericUpDown();

            panelButtons = new Panel();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLuu = new Button();
            btnHuy = new Button();
            btnSapXep = new Button();

            dgvMon = new DataGridView();
            colMaMon = new DataGridViewTextBoxColumn();
            colTenMon = new DataGridViewTextBoxColumn();
            colLoai = new DataGridViewTextBoxColumn();
            colDanhMuc = new DataGridViewTextBoxColumn();
            colDonGia = new DataGridViewTextBoxColumn();

            ((ISupportInitialize)bsMon).BeginInit();
            panelTitle.SuspendLayout();
            panelInput.SuspendLayout();
            ((ISupportInitialize)nudDonGia).BeginInit();
            panelButtons.SuspendLayout();
            ((ISupportInitialize)dgvMon).BeginInit();
            SuspendLayout();

            // panelTitle
            panelTitle.BackColor = Color.White;
            panelTitle.Controls.Add(lblTitle);
            panelTitle.Dock = DockStyle.Top;
            panelTitle.Location = new Point(0, 0);
            panelTitle.Name = "panelTitle";
            panelTitle.Size = new Size(1000, 60);
            panelTitle.TabIndex = 3;

            // lblTitle
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1000, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ MÓN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // panelInput
            panelInput.BackColor = Color.FromArgb(245, 247, 250);
            panelInput.Controls.Add(lblMaMon);
            panelInput.Controls.Add(txtMaMon);
            panelInput.Controls.Add(lblTenMon);
            panelInput.Controls.Add(txtTenMon);
            panelInput.Controls.Add(lblLoai);
            panelInput.Controls.Add(cboLoai);
            panelInput.Controls.Add(lblDanhMuc);
            panelInput.Controls.Add(cboDanhMuc);
            panelInput.Controls.Add(lblDonGia);
            panelInput.Controls.Add(nudDonGia);
            panelInput.Dock = DockStyle.Top;
            panelInput.Location = new Point(0, 60);
            panelInput.Name = "panelInput";
            panelInput.Size = new Size(1000, 120);
            panelInput.TabIndex = 2;

            // lblMaMon
            lblMaMon.AutoSize = true;
            lblMaMon.Location = new Point(40, 20);
            lblMaMon.Name = "lblMaMon";
            lblMaMon.Size = new Size(78, 23);
            lblMaMon.TabIndex = 0;
            lblMaMon.Text = "Mã món:";

            // txtMaMon
            txtMaMon.Location = new Point(120, 20);
            txtMaMon.Name = "txtMaMon";
            txtMaMon.Size = new Size(260, 30);
            txtMaMon.TabIndex = 1;

            // lblTenMon
            lblTenMon.AutoSize = true;
            lblTenMon.Location = new Point(380, 20);
            lblTenMon.Name = "lblTenMon";
            lblTenMon.Size = new Size(80, 23);
            lblTenMon.TabIndex = 2;
            lblTenMon.Text = "Tên món:";

            // txtTenMon
            txtTenMon.Location = new Point(460, 20);
            txtTenMon.Name = "txtTenMon";
            txtTenMon.Size = new Size(260, 30);
            txtTenMon.TabIndex = 3;

            // lblLoai
            lblLoai.AutoSize = true;
            lblLoai.Location = new Point(40, 60);
            lblLoai.Name = "lblLoai";
            lblLoai.Size = new Size(45, 23);
            lblLoai.TabIndex = 4;
            lblLoai.Text = "Loại:";

            // cboLoai
            cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoai.Items.AddRange(new object[] { "Thức ăn", "Thức uống" });
            cboLoai.Location = new Point(120, 60);
            cboLoai.Name = "cboLoai";
            cboLoai.Size = new Size(150, 31);
            cboLoai.TabIndex = 5;

            // lblDanhMuc
            lblDanhMuc.AutoSize = true;
            lblDanhMuc.Location = new Point(380, 60);
            lblDanhMuc.Name = "lblDanhMuc";
            lblDanhMuc.Size = new Size(93, 23);
            lblDanhMuc.TabIndex = 6;
            lblDanhMuc.Text = "Danh mục:";

            // cboDanhMuc
            cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDanhMuc.Items.AddRange(new object[] { "Món chính", "Món phụ", "Cafe", "Nước ngọt" });
            cboDanhMuc.Location = new Point(479, 57);
            cboDanhMuc.Name = "cboDanhMuc";
            cboDanhMuc.Size = new Size(220, 31);
            cboDanhMuc.TabIndex = 7;

            // lblDonGia
            lblDonGia.AutoSize = true;
            lblDonGia.Location = new Point(717, 63);
            lblDonGia.Name = "lblDonGia";
            lblDonGia.Size = new Size(74, 23);
            lblDonGia.TabIndex = 8;
            lblDonGia.Text = "Đơn giá:";

            // nudDonGia
            nudDonGia.DecimalPlaces = 0;
            nudDonGia.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            nudDonGia.Location = new Point(797, 61);
            nudDonGia.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            nudDonGia.Name = "nudDonGia";
            nudDonGia.Size = new Size(160, 30);
            nudDonGia.TabIndex = 9;
            nudDonGia.ThousandsSeparator = true;

            // panelButtons
            panelButtons.BackColor = Color.WhiteSmoke;
            panelButtons.Controls.Add(btnThem);
            panelButtons.Controls.Add(btnSua);
            panelButtons.Controls.Add(btnXoa);
            panelButtons.Controls.Add(btnLuu);
            panelButtons.Controls.Add(btnHuy);
            panelButtons.Controls.Add(btnSapXep);
            panelButtons.Dock = DockStyle.Top;
            panelButtons.Location = new Point(0, 180);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(1000, 50);
            panelButtons.TabIndex = 1;

            // btnThem
            btnThem.Location = new Point(40, 10);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(80, 30);
            btnThem.TabIndex = 0;
            btnThem.Text = "Thêm";

            // btnSua
            btnSua.Location = new Point(140, 10);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(80, 30);
            btnSua.TabIndex = 1;
            btnSua.Text = "Sửa";

            // btnXoa
            btnXoa.Location = new Point(240, 10);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(80, 30);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";

            // btnLuu
            btnLuu.Location = new Point(340, 10);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(80, 30);
            btnLuu.TabIndex = 3;
            btnLuu.Text = "Lưu";

            // btnHuy
            btnHuy.Location = new Point(440, 10);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(80, 30);
            btnHuy.TabIndex = 4;
            btnHuy.Text = "Hủy";

            // btnSapXep
            btnSapXep.Location = new Point(540, 10);
            btnSapXep.Name = "btnSapXep";
            btnSapXep.Size = new Size(140, 30);
            btnSapXep.TabIndex = 5;
            btnSapXep.Text = "Sắp xếp giá ↑";

            // dgvMon
            dgvMon.AllowUserToAddRows = false;
            dgvMon.AutoGenerateColumns = false;
            dgvMon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMon.BackgroundColor = Color.White;
            dgvMon.ColumnHeadersHeight = 29;
            dgvMon.Columns.AddRange(new DataGridViewColumn[]
            {
                colMaMon, colTenMon, colLoai, colDanhMuc, colDonGia
            });
            dgvMon.DataSource = bsMon;
            dgvMon.Dock = DockStyle.Fill;
            dgvMon.Location = new Point(0, 230);
            dgvMon.MultiSelect = false;
            dgvMon.Name = "dgvMon";
            dgvMon.ReadOnly = true;
            dgvMon.RowHeadersVisible = false;
            dgvMon.RowHeadersWidth = 51;
            dgvMon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMon.Size = new Size(1000, 370);
            dgvMon.TabIndex = 0;

            // columns
            colMaMon.Name = "colMaMon";
            colMaMon.HeaderText = "Mã món";
            colMaMon.DataPropertyName = "MaMon";
            colMaMon.ReadOnly = true;
            colMaMon.FillWeight = 14;

            colTenMon.Name = "colTenMon";
            colTenMon.HeaderText = "Tên món";
            colTenMon.DataPropertyName = "TenMon";
            colTenMon.ReadOnly = true;
            colTenMon.FillWeight = 30;

            colLoai.Name = "colLoai";
            colLoai.HeaderText = "Loại";
            colLoai.DataPropertyName = "Loai";
            colLoai.ReadOnly = true;
            colLoai.FillWeight = 16;

            colDanhMuc.Name = "colDanhMuc";
            colDanhMuc.HeaderText = "Danh mục";
            colDanhMuc.DataPropertyName = "DanhMuc";
            colDanhMuc.ReadOnly = true;
            colDanhMuc.FillWeight = 22;

            colDonGia.Name = "colDonGia";
            colDonGia.HeaderText = "Đơn giá";
            colDonGia.DataPropertyName = "DonGia";
            colDonGia.ReadOnly = true;
            colDonGia.FillWeight = 18;
            colDonGia.DefaultCellStyle.Format = "N0";
            colDonGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // FormQuanLyMon
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1000, 600);
            Controls.Add(dgvMon);
            Controls.Add(panelButtons);
            Controls.Add(panelInput);
            Controls.Add(panelTitle);
            Font = new Font("Segoe UI", 10F);
            Name = "FormQuanLyMon";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý món";

            ((ISupportInitialize)bsMon).EndInit();
            panelTitle.ResumeLayout(false);
            panelInput.ResumeLayout(false);
            panelInput.PerformLayout();
            ((ISupportInitialize)nudDonGia).EndInit();
            panelButtons.ResumeLayout(false);
            ((ISupportInitialize)dgvMon).EndInit();
            ResumeLayout(false);
        }
    }
}
