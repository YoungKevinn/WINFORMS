namespace Client_DoMInhKhoa.Forms
{
    partial class FormBaoCaoDoanhThu
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            panelTop = new Panel();
            label1 = new Label();
            dtpFrom = new DateTimePicker();
            label2 = new Label();
            dtpTo = new DateTimePicker();
            btnXem = new Button();
            dgvBaoCao = new DataGridView();
            colMaNV = new DataGridViewTextBoxColumn();
            colTenNV = new DataGridViewTextBoxColumn();
            colSoDon = new DataGridViewTextBoxColumn();
            colTongTien = new DataGridViewTextBoxColumn();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBaoCao).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.WhiteSmoke;
            panelTop.Controls.Add(label1);
            panelTop.Controls.Add(dtpFrom);
            panelTop.Controls.Add(label2);
            panelTop.Controls.Add(dtpTo);
            panelTop.Controls.Add(btnXem);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(800, 60);
            panelTop.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 22);
            label1.Name = "label1";
            label1.Size = new Size(83, 25);
            label1.TabIndex = 0;
            label1.Text = "Từ ngày:";
            // 
            // dtpFrom
            // 
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(119, 16);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(120, 32);
            dtpFrom.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(307, 22);
            label2.Name = "label2";
            label2.Size = new Size(96, 25);
            label2.TabIndex = 2;
            label2.Text = "Đến ngày:";
            // 
            // dtpTo
            // 
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(424, 15);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(120, 32);
            dtpTo.TabIndex = 3;
            // 
            // btnXem
            // 
            btnXem.BackColor = Color.FromArgb(13, 45, 104);
            btnXem.FlatStyle = FlatStyle.Flat;
            btnXem.ForeColor = Color.White;
            btnXem.Location = new Point(636, 16);
            btnXem.Name = "btnXem";
            btnXem.Size = new Size(120, 35);
            btnXem.TabIndex = 4;
            btnXem.Text = "Xem Báo Cáo";
            btnXem.UseVisualStyleBackColor = false;
            // 
            // dgvBaoCao
            // 
            dgvBaoCao.AllowUserToAddRows = false;
            dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBaoCao.BackgroundColor = Color.White;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(10, 25, 74);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dgvBaoCao.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvBaoCao.ColumnHeadersHeight = 29;
            dgvBaoCao.Columns.AddRange(new DataGridViewColumn[] { colMaNV, colTenNV, colSoDon, colTongTien });
            dgvBaoCao.Dock = DockStyle.Fill;
            dgvBaoCao.EnableHeadersVisualStyles = false;
            dgvBaoCao.Location = new Point(0, 60);
            dgvBaoCao.Name = "dgvBaoCao";
            dgvBaoCao.ReadOnly = true;
            dgvBaoCao.RowHeadersVisible = false;
            dgvBaoCao.RowHeadersWidth = 51;
            dgvBaoCao.Size = new Size(800, 440);
            dgvBaoCao.TabIndex = 0;
            // 
            // colMaNV
            // 
            colMaNV.DataPropertyName = "MaNhanVien";
            colMaNV.HeaderText = "Mã NV";
            colMaNV.MinimumWidth = 6;
            colMaNV.Name = "colMaNV";
            colMaNV.ReadOnly = true;
            // 
            // colTenNV
            // 
            colTenNV.DataPropertyName = "HoTen";
            colTenNV.HeaderText = "Họ Tên";
            colTenNV.MinimumWidth = 6;
            colTenNV.Name = "colTenNV";
            colTenNV.ReadOnly = true;
            // 
            // colSoDon
            // 
            colSoDon.DataPropertyName = "SoHoaDon";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSoDon.DefaultCellStyle = dataGridViewCellStyle2;
            colSoDon.HeaderText = "Số Đơn";
            colSoDon.MinimumWidth = 6;
            colSoDon.Name = "colSoDon";
            colSoDon.ReadOnly = true;
            // 
            // colTongTien
            // 
            colTongTien.DataPropertyName = "TongTien";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            colTongTien.DefaultCellStyle = dataGridViewCellStyle3;
            colTongTien.HeaderText = "Doanh Thu";
            colTongTien.MinimumWidth = 6;
            colTongTien.Name = "colTongTien";
            colTongTien.ReadOnly = true;
            // 
            // FormBaoCaoDoanhThu
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 500);
            Controls.Add(dgvBaoCao);
            Controls.Add(panelTop);
            Font = new Font("Segoe UI", 11F);
            Name = "FormBaoCaoDoanhThu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thống kê doanh thu nhân viên";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBaoCao).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnXem;
        private System.Windows.Forms.DataGridView dgvBaoCao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaNV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenNV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongTien;
    }
}