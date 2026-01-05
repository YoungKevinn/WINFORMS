namespace Client_DoMInhKhoa.Forms
{
    partial class FormTraCuuHoaDonAdmin
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Label lblTuNgay;
        private System.Windows.Forms.Label lblDenNgay;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Button btnLoc;
        private System.Windows.Forms.DataGridView dgvHoaDon;
        private System.Windows.Forms.Button btnXemIn;

        // Ô tìm kiếm mới
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            btnExportPdf = new Button();
            btnExportExcel = new Button();
            lblTuNgay = new Label();
            lblDenNgay = new Label();
            dtFrom = new DateTimePicker();
            dtTo = new DateTimePicker();
            btnLoc = new Button();
            dgvHoaDon = new DataGridView();
            btnXemIn = new Button();
            lblSearch = new Label();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).BeginInit();
            SuspendLayout();
            // 
            // btnExportPdf
            // 
            btnExportPdf.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExportPdf.Location = new Point(664, 667);
            btnExportPdf.Margin = new Padding(3, 4, 3, 4);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(103, 40);
            btnExportPdf.TabIndex = 6;
            btnExportPdf.Text = "Xuất PDF";
            btnExportPdf.UseVisualStyleBackColor = true;
            btnExportPdf.Click += btnExportPdf_Click;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExportExcel.Location = new Point(797, 667);
            btnExportExcel.Margin = new Padding(3, 4, 3, 4);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(103, 40);
            btnExportExcel.TabIndex = 7;
            btnExportExcel.Text = "Xuất Excel";
            btnExportExcel.UseVisualStyleBackColor = true;
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // lblTuNgay
            // 
            lblTuNgay.AutoSize = true;
            lblTuNgay.Location = new Point(14, 20);
            lblTuNgay.Name = "lblTuNgay";
            lblTuNgay.Size = new Size(65, 20);
            lblTuNgay.TabIndex = 0;
            lblTuNgay.Text = "Từ ngày:";
            // 
            // lblDenNgay
            // 
            lblDenNgay.AutoSize = true;
            lblDenNgay.Location = new Point(229, 20);
            lblDenNgay.Name = "lblDenNgay";
            lblDenNgay.Size = new Size(75, 20);
            lblDenNgay.TabIndex = 2;
            lblDenNgay.Text = "Đến ngày:";
            // 
            // dtFrom
            // 
            dtFrom.CustomFormat = "dd/MM/yyyy";
            dtFrom.Format = DateTimePickerFormat.Custom;
            dtFrom.Location = new Point(83, 16);
            dtFrom.Margin = new Padding(3, 4, 3, 4);
            dtFrom.Name = "dtFrom";
            dtFrom.Size = new Size(125, 27);
            dtFrom.TabIndex = 1;
            // 
            // dtTo
            // 
            dtTo.CustomFormat = "dd/MM/yyyy";
            dtTo.Format = DateTimePickerFormat.Custom;
            dtTo.Location = new Point(305, 16);
            dtTo.Margin = new Padding(3, 4, 3, 4);
            dtTo.Name = "dtTo";
            dtTo.Size = new Size(125, 27);
            dtTo.TabIndex = 3;
            // 
            // btnLoc
            // 
            btnLoc.Location = new Point(451, 15);
            btnLoc.Margin = new Padding(3, 4, 3, 4);
            btnLoc.Name = "btnLoc";
            btnLoc.Size = new Size(69, 33);
            btnLoc.TabIndex = 4;
            btnLoc.Text = "Lọc";
            btnLoc.UseVisualStyleBackColor = true;
            btnLoc.Click += btnLoc_Click;
            // 
            // dgvHoaDon
            // 
            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.AllowUserToDeleteRows = false;
            dgvHoaDon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHoaDon.Location = new Point(14, 67);
            dgvHoaDon.Margin = new Padding(3, 4, 3, 4);
            dgvHoaDon.Name = "dgvHoaDon";
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.RowHeadersWidth = 51;
            dgvHoaDon.RowTemplate.Height = 25;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.Size = new Size(1011, 573);
            dgvHoaDon.TabIndex = 5;
            dgvHoaDon.CellDoubleClick += dgvHoaDon_CellDoubleClick;
            // 
            // btnXemIn
            // 
            btnXemIn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnXemIn.Location = new Point(922, 667);
            btnXemIn.Margin = new Padding(3, 4, 3, 4);
            btnXemIn.Name = "btnXemIn";
            btnXemIn.Size = new Size(103, 40);
            btnXemIn.TabIndex = 8;
            btnXemIn.Text = "Xem / In";
            btnXemIn.UseVisualStyleBackColor = true;
            btnXemIn.Click += btnXemIn_Click;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(537, 20);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(73, 20);
            lblSearch.TabIndex = 9;
            lblSearch.Text = "Tìm kiếm:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(615, 16);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(285, 27);
            txtSearch.TabIndex = 10;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // FormTraCuuHoaDonAdmin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1039, 733);
            Controls.Add(txtSearch);
            Controls.Add(lblSearch);
            Controls.Add(btnXemIn);
            Controls.Add(btnExportExcel);
            Controls.Add(btnExportPdf);
            Controls.Add(dgvHoaDon);
            Controls.Add(btnLoc);
            Controls.Add(dtTo);
            Controls.Add(lblDenNgay);
            Controls.Add(dtFrom);
            Controls.Add(lblTuNgay);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormTraCuuHoaDonAdmin";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tra cứu hóa đơn";
            Load += FormTraCuuHoaDon_Load;
            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
