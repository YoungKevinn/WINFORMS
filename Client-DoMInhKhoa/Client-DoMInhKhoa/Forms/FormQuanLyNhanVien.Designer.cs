namespace Client_DoMInhKhoa.Forms
{
    partial class FormQuanLyNhanVien
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle cellStyle = new System.Windows.Forms.DataGridViewCellStyle();

            // Khởi tạo các Control
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelInput = new System.Windows.Forms.Panel();
            this.chkDangHoatDong = new System.Windows.Forms.CheckBox();
            this.cboVaiTro = new System.Windows.Forms.ComboBox();
            this.lblVaiTro = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.txtMaNV = new System.Windows.Forms.TextBox();
            this.lblMaNV = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.dgvNhanVien = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaNV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVaiTro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewCheckBoxColumn();

            this.panelTitle.SuspendLayout();
            this.panelInput.SuspendLayout();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).BeginInit();
            this.SuspendLayout();

            // 
            // panelTitle (Header xanh)
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(25)))), ((int)(((byte)(74)))));
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Height = 50;
            this.panelTitle.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 10);
            this.lblTitle.Text = "QUẢN LÝ NHÂN VIÊN";

            // 
            // panelInput (Vùng nhập liệu)
            // 
            this.panelInput.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelInput.Controls.Add(this.chkDangHoatDong);
            this.panelInput.Controls.Add(this.cboVaiTro);
            this.panelInput.Controls.Add(this.lblVaiTro);
            this.panelInput.Controls.Add(this.txtHoTen);
            this.panelInput.Controls.Add(this.lblHoTen);
            this.panelInput.Controls.Add(this.txtMaNV);
            this.panelInput.Controls.Add(this.lblMaNV);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInput.Height = 100;
            this.panelInput.TabIndex = 1;

            // Các Label và TextBox
            this.lblMaNV.Location = new System.Drawing.Point(30, 20);
            this.lblMaNV.Text = "Mã nhân viên:";
            this.lblMaNV.AutoSize = true;

            this.txtMaNV.Location = new System.Drawing.Point(30, 45);
            this.txtMaNV.Size = new System.Drawing.Size(150, 27);

            this.lblHoTen.Location = new System.Drawing.Point(200, 20);
            this.lblHoTen.Text = "Họ và tên:";
            this.lblHoTen.AutoSize = true;

            this.txtHoTen.Location = new System.Drawing.Point(200, 45);
            this.txtHoTen.Size = new System.Drawing.Size(250, 27);

            this.lblVaiTro.Location = new System.Drawing.Point(470, 20);
            this.lblVaiTro.Text = "Vai trò:";
            this.lblVaiTro.AutoSize = true;

            this.cboVaiTro.Location = new System.Drawing.Point(470, 45);
            this.cboVaiTro.Size = new System.Drawing.Size(150, 27);
            this.cboVaiTro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.chkDangHoatDong.Location = new System.Drawing.Point(650, 47);
            this.chkDangHoatDong.Text = "Đang hoạt động";
            this.chkDangHoatDong.AutoSize = true;
            this.chkDangHoatDong.Checked = true;

            // 
            // panelButtons (Thanh nút bấm)
            // 
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.Controls.Add(this.btnHuy);
            this.panelButtons.Controls.Add(this.btnLuu);
            this.panelButtons.Controls.Add(this.btnXoa);
            this.panelButtons.Controls.Add(this.btnSua);
            this.panelButtons.Controls.Add(this.btnThem);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Height = 60;
            this.panelButtons.TabIndex = 2;

            // Style chung cho Button
            System.Drawing.Size btnSize = new System.Drawing.Size(100, 35);
            int yBtn = 12;

            this.btnThem.Text = "Thêm";
            this.btnThem.Location = new System.Drawing.Point(30, yBtn);
            this.btnThem.Size = btnSize;
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(13, 45, 104);
            this.btnThem.ForeColor = System.Drawing.Color.White;

            this.btnSua.Text = "Sửa";
            this.btnSua.Location = new System.Drawing.Point(140, yBtn);
            this.btnSua.Size = btnSize;
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(13, 45, 104);
            this.btnSua.ForeColor = System.Drawing.Color.White;

            this.btnXoa.Text = "Xóa";
            this.btnXoa.Location = new System.Drawing.Point(250, yBtn);
            this.btnXoa.Size = btnSize;
            this.btnXoa.BackColor = System.Drawing.Color.Firebrick;
            this.btnXoa.ForeColor = System.Drawing.Color.White;

            this.btnLuu.Text = "Lưu";
            this.btnLuu.Location = new System.Drawing.Point(550, yBtn);
            this.btnLuu.Size = btnSize;
            this.btnLuu.BackColor = System.Drawing.Color.ForestGreen;
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Enabled = false;

            this.btnHuy.Text = "Hủy";
            this.btnHuy.Location = new System.Drawing.Point(660, yBtn);
            this.btnHuy.Size = btnSize;
            this.btnHuy.BackColor = System.Drawing.Color.Gray;
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Enabled = false;

            // 
            // dgvNhanVien (Lưới dữ liệu)
            // 
            this.dgvNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNhanVien.BackgroundColor = System.Drawing.Color.White;
            this.dgvNhanVien.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNhanVien.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNhanVien.ReadOnly = true;
            this.dgvNhanVien.RowHeadersVisible = false;
            this.dgvNhanVien.AllowUserToAddRows = false;

            // Header Style
            cellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            cellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvNhanVien.ColumnHeadersDefaultCellStyle = cellStyle;
            this.dgvNhanVien.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colId, this.colMaNV, this.colHoTen, this.colVaiTro, this.colTrangThai
            });

            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "ID";
            this.colId.Visible = false;

            this.colMaNV.DataPropertyName = "MaNhanVien";
            this.colMaNV.HeaderText = "Mã NV";

            this.colHoTen.DataPropertyName = "HoTen";
            this.colHoTen.HeaderText = "Họ tên";

            this.colVaiTro.DataPropertyName = "VaiTro";
            this.colVaiTro.HeaderText = "Vai trò";

            this.colTrangThai.DataPropertyName = "DangHoatDong";
            this.colTrangThai.HeaderText = "Hoạt động";

            // 
            // FormQuanLyNhanVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.dgvNhanVien);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelInput);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.Name = "FormQuanLyNhanVien";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý nhân viên";

            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.DataGridView dgvNhanVien;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.Label lblMaNV;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.ComboBox cboVaiTro;
        private System.Windows.Forms.Label lblVaiTro;
        private System.Windows.Forms.CheckBox chkDangHoatDong;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaNV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHoTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVaiTro;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colTrangThai;
    }
}