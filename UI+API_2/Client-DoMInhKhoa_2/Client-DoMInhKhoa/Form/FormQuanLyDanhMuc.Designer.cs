using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    partial class FormQuanLyDanhMuc
    {
        private IContainer components = null!;

        private Panel panelTitle = null!;
        private Label lblTitle = null!;

        private Panel panelInput = null!;
        private Label lblMaDm = null!;
        private Label lblTenDm = null!;
        private Label lblLoai = null!;
        private Label lblGhiChu = null!;

        private TextBox txtMaDm = null!;
        private TextBox txtTenDm = null!;
        private ComboBox cboLoai = null!;
        private TextBox txtGhiChu = null!;

        private Panel panelButtons = null!;
        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLuu = null!;
        private Button btnHuy = null!;

        private DataGridView dgvDanhMuc = null!;
        private BindingSource bsDanhMuc = null!;

        private DataGridViewTextBoxColumn colMaDanhMuc = null!;
        private DataGridViewTextBoxColumn colTenDanhMuc = null!;
        private DataGridViewTextBoxColumn colLoai = null!;
        private DataGridViewTextBoxColumn colGhiChu = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            bsDanhMuc = new BindingSource(components);

            panelTitle = new Panel();
            lblTitle = new Label();

            panelInput = new Panel();
            lblMaDm = new Label();
            txtMaDm = new TextBox();
            lblTenDm = new Label();
            txtTenDm = new TextBox();
            lblLoai = new Label();
            cboLoai = new ComboBox();
            lblGhiChu = new Label();
            txtGhiChu = new TextBox();

            panelButtons = new Panel();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLuu = new Button();
            btnHuy = new Button();

            dgvDanhMuc = new DataGridView();
            colMaDanhMuc = new DataGridViewTextBoxColumn();
            colTenDanhMuc = new DataGridViewTextBoxColumn();
            colLoai = new DataGridViewTextBoxColumn();
            colGhiChu = new DataGridViewTextBoxColumn();

            ((ISupportInitialize)bsDanhMuc).BeginInit();
            panelTitle.SuspendLayout();
            panelInput.SuspendLayout();
            panelButtons.SuspendLayout();
            ((ISupportInitialize)dgvDanhMuc).BeginInit();
            SuspendLayout();

            // panelTitle
            panelTitle.BackColor = Color.White;
            panelTitle.Controls.Add(lblTitle);
            panelTitle.Dock = DockStyle.Top;
            panelTitle.Location = new Point(0, 0);
            panelTitle.Name = "panelTitle";
            panelTitle.Size = new Size(900, 60);
            panelTitle.TabIndex = 3;

            // lblTitle
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(900, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ DANH MỤC";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // panelInput
            panelInput.BackColor = Color.FromArgb(245, 247, 250);
            panelInput.Controls.Add(lblMaDm);
            panelInput.Controls.Add(txtMaDm);
            panelInput.Controls.Add(lblTenDm);
            panelInput.Controls.Add(txtTenDm);
            panelInput.Controls.Add(lblLoai);
            panelInput.Controls.Add(cboLoai);
            panelInput.Controls.Add(lblGhiChu);
            panelInput.Controls.Add(txtGhiChu);
            panelInput.Dock = DockStyle.Top;
            panelInput.Location = new Point(0, 60);
            panelInput.Name = "panelInput";
            panelInput.Size = new Size(900, 150);
            panelInput.TabIndex = 2;

            // lblMaDm
            lblMaDm.AutoSize = true;
            lblMaDm.Location = new Point(30, 20);
            lblMaDm.Name = "lblMaDm";
            lblMaDm.Size = new Size(120, 23);
            lblMaDm.TabIndex = 0;
            lblMaDm.Text = "Mã danh mục:";

            // txtMaDm
            txtMaDm.Location = new Point(156, 20);
            txtMaDm.Name = "txtMaDm";
            txtMaDm.Size = new Size(200, 30);
            txtMaDm.TabIndex = 1;

            // lblTenDm
            lblTenDm.AutoSize = true;
            lblTenDm.Location = new Point(412, 20);
            lblTenDm.Name = "lblTenDm";
            lblTenDm.Size = new Size(122, 23);
            lblTenDm.TabIndex = 2;
            lblTenDm.Text = "Tên danh mục:";

            // txtTenDm
            txtTenDm.Location = new Point(540, 20);
            txtTenDm.Name = "txtTenDm";
            txtTenDm.Size = new Size(200, 30);
            txtTenDm.TabIndex = 3;

            // lblLoai
            lblLoai.AutoSize = true;
            lblLoai.Location = new Point(30, 60);
            lblLoai.Name = "lblLoai";
            lblLoai.Size = new Size(45, 23);
            lblLoai.TabIndex = 4;
            lblLoai.Text = "Loại:";

            // cboLoai
            cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoai.Items.AddRange(new object[] { "Thức ăn", "Thức uống" });
            cboLoai.Location = new Point(156, 60);
            cboLoai.Name = "cboLoai";
            cboLoai.Size = new Size(200, 31);
            cboLoai.TabIndex = 5;

            // lblGhiChu
            lblGhiChu.AutoSize = true;
            lblGhiChu.Location = new Point(461, 60);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(73, 23);
            lblGhiChu.TabIndex = 6;
            lblGhiChu.Text = "Ghi chú:";

            // txtGhiChu
            txtGhiChu.Location = new Point(540, 60);
            txtGhiChu.Multiline = true;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(200, 50);
            txtGhiChu.TabIndex = 7;

            // panelButtons
            panelButtons.BackColor = Color.WhiteSmoke;
            panelButtons.Controls.Add(btnThem);
            panelButtons.Controls.Add(btnSua);
            panelButtons.Controls.Add(btnXoa);
            panelButtons.Controls.Add(btnLuu);
            panelButtons.Controls.Add(btnHuy);
            panelButtons.Dock = DockStyle.Top;
            panelButtons.Location = new Point(0, 210);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(900, 50);
            panelButtons.TabIndex = 1;

            // btnThem
            btnThem.Location = new Point(30, 10);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(80, 30);
            btnThem.TabIndex = 0;
            btnThem.Text = "Thêm";

            // btnSua
            btnSua.Location = new Point(120, 10);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(80, 30);
            btnSua.TabIndex = 1;
            btnSua.Text = "Sửa";

            // btnXoa
            btnXoa.Location = new Point(210, 10);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(80, 30);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";

            // btnLuu
            btnLuu.Location = new Point(300, 10);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(80, 30);
            btnLuu.TabIndex = 3;
            btnLuu.Text = "Lưu";

            // btnHuy
            btnHuy.Location = new Point(390, 10);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(80, 30);
            btnHuy.TabIndex = 4;
            btnHuy.Text = "Hủy";

            // dgvDanhMuc
            dgvDanhMuc.AllowUserToAddRows = false;
            dgvDanhMuc.AutoGenerateColumns = false;
            dgvDanhMuc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhMuc.BackgroundColor = Color.White;
            dgvDanhMuc.ColumnHeadersHeight = 29;
            dgvDanhMuc.Columns.AddRange(new DataGridViewColumn[]
            {
                colMaDanhMuc, colTenDanhMuc, colLoai, colGhiChu
            });
            dgvDanhMuc.DataSource = bsDanhMuc;
            dgvDanhMuc.Dock = DockStyle.Fill;
            dgvDanhMuc.Location = new Point(0, 260);
            dgvDanhMuc.MultiSelect = false;
            dgvDanhMuc.Name = "dgvDanhMuc";
            dgvDanhMuc.ReadOnly = true;
            dgvDanhMuc.RowHeadersVisible = false;
            dgvDanhMuc.RowHeadersWidth = 51;
            dgvDanhMuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhMuc.Size = new Size(900, 290);
            dgvDanhMuc.TabIndex = 0;

            // columns
            colMaDanhMuc.Name = "colMaDanhMuc";
            colMaDanhMuc.HeaderText = "Mã danh mục";
            colMaDanhMuc.DataPropertyName = "MaDanhMuc";
            colMaDanhMuc.ReadOnly = true;
            colMaDanhMuc.FillWeight = 18;

            colTenDanhMuc.Name = "colTenDanhMuc";
            colTenDanhMuc.HeaderText = "Tên danh mục";
            colTenDanhMuc.DataPropertyName = "TenDanhMuc";
            colTenDanhMuc.ReadOnly = true;
            colTenDanhMuc.FillWeight = 32;

            colLoai.Name = "colLoai";
            colLoai.HeaderText = "Loại";
            colLoai.DataPropertyName = "Loai";
            colLoai.ReadOnly = true;
            colLoai.FillWeight = 18;

            colGhiChu.Name = "colGhiChu";
            colGhiChu.HeaderText = "Ghi chú";
            colGhiChu.DataPropertyName = "GhiChu";
            colGhiChu.ReadOnly = true;
            colGhiChu.FillWeight = 32;

            // Form
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(900, 550);
            Controls.Add(dgvDanhMuc);
            Controls.Add(panelButtons);
            Controls.Add(panelInput);
            Controls.Add(panelTitle);
            Font = new Font("Segoe UI", 10F);
            Name = "FormQuanLyDanhMuc";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý danh mục";

            ((ISupportInitialize)bsDanhMuc).EndInit();
            panelTitle.ResumeLayout(false);
            panelInput.ResumeLayout(false);
            panelInput.PerformLayout();
            panelButtons.ResumeLayout(false);
            ((ISupportInitialize)dgvDanhMuc).EndInit();
            ResumeLayout(false);
        }
    }
}
