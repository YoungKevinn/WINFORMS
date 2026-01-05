using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    partial class FormHoaDon
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dgvChiTiet;
        private Label lblMaHoaDon;
        private Label lblNgayGio;
        private Label lblTongTien;
        private Button btnIn;
        private Button btnDong;
        private System.Drawing.Printing.PrintDocument printDocument1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dgvChiTiet = new DataGridView();
            lblMaHoaDon = new Label();
            lblNgayGio = new Label();
            lblTongTien = new Label();
            btnIn = new Button();
            btnDong = new Button();
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            SuspendLayout();
            // 
            // dgvChiTiet
            // 
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.ColumnHeadersHeight = 29;
            dgvChiTiet.Location = new Point(23, 107);
            dgvChiTiet.Margin = new Padding(3, 4, 3, 4);
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersWidth = 51;
            dgvChiTiet.RowTemplate.Height = 28;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.Size = new Size(983, 573);
            dgvChiTiet.TabIndex = 0;
            // 
            // lblMaHoaDon
            // 
            lblMaHoaDon.AutoSize = true;
            lblMaHoaDon.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblMaHoaDon.Location = new Point(23, 27);
            lblMaHoaDon.Name = "lblMaHoaDon";
            lblMaHoaDon.Size = new Size(169, 32);
            lblMaHoaDon.TabIndex = 1;
            lblMaHoaDon.Text = "Đơn gọi #000";
            // 
            // lblNgayGio
            // 
            lblNgayGio.AutoSize = true;
            lblNgayGio.Font = new Font("Segoe UI", 10F);
            lblNgayGio.Location = new Point(23, 67);
            lblNgayGio.Name = "lblNgayGio";
            lblNgayGio.Size = new Size(106, 23);
            lblNgayGio.TabIndex = 2;
            lblNgayGio.Text = "dd/MM/yyyy";
            // 
            // lblTongTien
            // 
            lblTongTien.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTongTien.Location = new Point(846, 61);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(43, 28);
            lblTongTien.TabIndex = 3;
            lblTongTien.Text = "0 đ";
            // 
            // btnIn
            // 
            btnIn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnIn.Location = new Point(777, 707);
            btnIn.Margin = new Padding(3, 4, 3, 4);
            btnIn.Name = "btnIn";
            btnIn.Size = new Size(114, 47);
            btnIn.TabIndex = 4;
            btnIn.Text = "In hóa đơn";
            btnIn.UseVisualStyleBackColor = true;
            btnIn.Click += btnIn_Click;
            // 
            // btnDong
            // 
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Location = new Point(903, 707);
            btnDong.Margin = new Padding(3, 4, 3, 4);
            btnDong.Name = "btnDong";
            btnDong.Size = new Size(114, 47);
            btnDong.TabIndex = 5;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = true;
            btnDong.Click += btnDong_Click;
            // 
            // printDocument1
            // 
            printDocument1.PrintPage += PrintDocument1_PrintPage;
            // 
            // FormHoaDon
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1029, 800);
            Controls.Add(btnDong);
            Controls.Add(btnIn);
            Controls.Add(lblTongTien);
            Controls.Add(lblNgayGio);
            Controls.Add(lblMaHoaDon);
            Controls.Add(dgvChiTiet);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormHoaDon";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Hóa đơn";
            Load += FormHoaDon_Load;
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
