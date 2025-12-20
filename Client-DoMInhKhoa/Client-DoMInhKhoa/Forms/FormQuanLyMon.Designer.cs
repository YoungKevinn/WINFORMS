namespace Client_DoMInhKhoa.Forms
{
    partial class FormQuanLyMon
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

            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelInput = new System.Windows.Forms.Panel();
            this.nudDonGia = new System.Windows.Forms.NumericUpDown();
            this.cboDanhMuc = new System.Windows.Forms.ComboBox();
            this.cboLoai = new System.Windows.Forms.ComboBox();
            this.txtTenMon = new System.Windows.Forms.TextBox();
            this.txtMaMon = new System.Windows.Forms.TextBox();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.lblDanhMuc = new System.Windows.Forms.Label();
            this.lblLoai = new System.Windows.Forms.Label();
            this.lblTenMon = new System.Windows.Forms.Label();
            this.lblMaMon = new System.Windows.Forms.Label(); // ID ẩn hoặc hiển thị tùy ý
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.dgvMon = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDanhMuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGia = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.panelTitle.SuspendLayout();
            this.panelInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDonGia)).BeginInit();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMon)).BeginInit();
            this.SuspendLayout();

            // 
            // panelTitle
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
            this.lblTitle.Location = new System.Drawing.Point(20, 12);
            this.lblTitle.Text = "QUẢN LÝ THỰC ĐƠN";

            // 
            // panelInput
            // 
            this.panelInput.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelInput.Controls.Add(this.nudDonGia);
            this.panelInput.Controls.Add(this.cboDanhMuc);
            this.panelInput.Controls.Add(this.cboLoai);
            this.panelInput.Controls.Add(this.txtTenMon);
            this.panelInput.Controls.Add(this.txtMaMon);
            this.panelInput.Controls.Add(this.lblDonGia);
            this.panelInput.Controls.Add(this.lblDanhMuc);
            this.panelInput.Controls.Add(this.lblLoai);
            this.panelInput.Controls.Add(this.lblTenMon);
            this.panelInput.Controls.Add(this.lblMaMon);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInput.Height = 110;
            this.panelInput.TabIndex = 1;

            // Control vị trí
            int line1Y = 20;
            int line2Y = 60;

            // Tên món
            this.lblTenMon.Location = new System.Drawing.Point(30, line1Y);
            this.lblTenMon.Text = "Tên món:";
            this.lblTenMon.AutoSize = true;
            this.txtTenMon.Location = new System.Drawing.Point(110, line1Y - 3);
            this.txtTenMon.Size = new System.Drawing.Size(200, 27);

            // Loại (Thức ăn/Uống)
            this.lblLoai.Location = new System.Drawing.Point(350, line1Y);
            this.lblLoai.Text = "Loại:";
            this.lblLoai.AutoSize = true;
            this.cboLoai.Location = new System.Drawing.Point(430, line1Y - 3);
            this.cboLoai.Size = new System.Drawing.Size(150, 27);
            this.cboLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Danh mục
            this.lblDanhMuc.Location = new System.Drawing.Point(30, line2Y);
            this.lblDanhMuc.Text = "Danh mục:";
            this.lblDanhMuc.AutoSize = true;
            this.cboDanhMuc.Location = new System.Drawing.Point(110, line2Y - 3);
            this.cboDanhMuc.Size = new System.Drawing.Size(200, 27);
            this.cboDanhMuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Đơn giá
            this.lblDonGia.Location = new System.Drawing.Point(350, line2Y);
            this.lblDonGia.Text = "Đơn giá:";
            this.lblDonGia.AutoSize = true;
            this.nudDonGia.Location = new System.Drawing.Point(430, line2Y - 3);
            this.nudDonGia.Size = new System.Drawing.Size(150, 27);
            this.nudDonGia.Maximum = 100000000;
            this.nudDonGia.Increment = 1000;
            this.nudDonGia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // ID (Ẩn)
            this.txtMaMon.Visible = false;
            this.lblMaMon.Visible = false;

            // 
            // panelButtons
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

            // Buttons
            System.Drawing.Size btnSize = new System.Drawing.Size(90, 35);
            int btnY = 12;

            this.btnThem.Text = "Thêm";
            this.btnThem.Location = new System.Drawing.Point(30, btnY);
            this.btnThem.Size = btnSize;
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(13, 45, 104);
            this.btnThem.ForeColor = System.Drawing.Color.White;

            this.btnSua.Text = "Sửa";
            this.btnSua.Location = new System.Drawing.Point(130, btnY);
            this.btnSua.Size = btnSize;
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(13, 45, 104);
            this.btnSua.ForeColor = System.Drawing.Color.White;

            this.btnXoa.Text = "Xóa";
            this.btnXoa.Location = new System.Drawing.Point(230, btnY);
            this.btnXoa.Size = btnSize;
            this.btnXoa.BackColor = System.Drawing.Color.Firebrick;
            this.btnXoa.ForeColor = System.Drawing.Color.White;

            this.btnLuu.Text = "Lưu";
            this.btnLuu.Location = new System.Drawing.Point(450, btnY);
            this.btnLuu.Size = btnSize;
            this.btnLuu.BackColor = System.Drawing.Color.ForestGreen;
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Enabled = false;

            this.btnHuy.Text = "Hủy";
            this.btnHuy.Location = new System.Drawing.Point(550, btnY);
            this.btnHuy.Size = btnSize;
            this.btnHuy.BackColor = System.Drawing.Color.Gray;
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Enabled = false;

            // 
            // dgvMon
            // 
            this.dgvMon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMon.BackgroundColor = System.Drawing.Color.White;
            this.dgvMon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMon.AllowUserToAddRows = false;
            this.dgvMon.ReadOnly = true;
            this.dgvMon.RowHeadersVisible = false;

            // Header Style
            cellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            cellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvMon.ColumnHeadersDefaultCellStyle = cellStyle;

            this.dgvMon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colId, this.colTen, this.colLoai, this.colDanhMuc, this.colGia
            });

            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "ID";
            this.colId.Width = 50;

            this.colTen.DataPropertyName = "Ten";
            this.colTen.HeaderText = "Tên món";

            this.colLoai.DataPropertyName = "Loai";
            this.colLoai.HeaderText = "Loại";
            this.colLoai.Width = 100;

            this.colDanhMuc.DataPropertyName = "TenDanhMuc"; // Cần map field này trong code
            this.colDanhMuc.HeaderText = "Danh mục";

            this.colGia.DataPropertyName = "DonGia";
            this.colGia.HeaderText = "Đơn giá";
            this.colGia.DefaultCellStyle.Format = "N0";
            this.colGia.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            // 
            // FormQuanLyMon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.dgvMon);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelInput);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.Name = "FormQuanLyMon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý món";

            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDonGia)).EndInit();
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMon)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.DataGridView dgvMon;

        private System.Windows.Forms.TextBox txtTenMon;
        private System.Windows.Forms.Label lblTenMon;
        private System.Windows.Forms.TextBox txtMaMon;
        private System.Windows.Forms.Label lblMaMon;
        private System.Windows.Forms.ComboBox cboLoai;
        private System.Windows.Forms.Label lblLoai;
        private System.Windows.Forms.ComboBox cboDanhMuc;
        private System.Windows.Forms.Label lblDanhMuc;
        private System.Windows.Forms.NumericUpDown nudDonGia;
        private System.Windows.Forms.Label lblDonGia;

        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;

        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDanhMuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGia;
    }
}