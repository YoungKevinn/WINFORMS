using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    partial class FormBaoCaoDoanhThu
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelFilter;
        private Label lblFrom;
        private Label lblTo;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Label lblKieu;
        private ComboBox cboKieuThongKe;
        private Button btnXemBaoCao;
        private Button btnXemBieuDo;
        private TabControl tabControl;
        private TabPage tabTongQuan;
        private TabPage tabNhanVien;
        private DataGridView dgvTongQuan;
        private DataGridView dgvNhanVien;
        private Panel panelSummaryTongQuan;
        private Label lblTongThu;
        private Label lblSoHoaDon;
        private Panel panelSummaryNhanVien;
        private Label lblTongThuNhanVien;
        private Label lblSoNhanVien;

        // Ô tìm kiếm mới
        private Label lblSearch;
        private TextBox txtSearch;

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
            panelFilter = new Panel();
            lblFrom = new Label();
            dtpFrom = new DateTimePicker();
            lblTo = new Label();
            dtpTo = new DateTimePicker();
            lblKieu = new Label();
            cboKieuThongKe = new ComboBox();
            btnXemBaoCao = new Button();
            btnXemBieuDo = new Button();
            lblSearch = new Label();
            txtSearch = new TextBox();
            tabControl = new TabControl();
            tabTongQuan = new TabPage();
            dgvTongQuan = new DataGridView();
            panelSummaryTongQuan = new Panel();
            btnXuatExcel = new Button();
            lblTongThu = new Label();
            lblSoHoaDon = new Label();
            tabNhanVien = new TabPage();
            dgvNhanVien = new DataGridView();
            panelSummaryNhanVien = new Panel();
            lblTongThuNhanVien = new Label();
            lblSoNhanVien = new Label();
            btnXuatPDF = new Button();
            panelFilter.SuspendLayout();
            tabControl.SuspendLayout();
            tabTongQuan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTongQuan).BeginInit();
            panelSummaryTongQuan.SuspendLayout();
            tabNhanVien.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNhanVien).BeginInit();
            panelSummaryNhanVien.SuspendLayout();
            SuspendLayout();
            // 
            // panelFilter
            // 
            panelFilter.BackColor = Color.WhiteSmoke;
            panelFilter.Controls.Add(lblFrom);
            panelFilter.Controls.Add(dtpFrom);
            panelFilter.Controls.Add(lblTo);
            panelFilter.Controls.Add(dtpTo);
            panelFilter.Controls.Add(lblKieu);
            panelFilter.Controls.Add(cboKieuThongKe);
            panelFilter.Controls.Add(btnXemBaoCao);
            panelFilter.Controls.Add(btnXemBieuDo);
            panelFilter.Controls.Add(lblSearch);
            panelFilter.Controls.Add(txtSearch);
            panelFilter.Dock = DockStyle.Top;
            panelFilter.Location = new Point(0, 0);
            panelFilter.Name = "panelFilter";
            panelFilter.Padding = new Padding(10);
            panelFilter.Size = new Size(800, 80);
            panelFilter.TabIndex = 1;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Location = new Point(15, 18);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(62, 20);
            lblFrom.TabIndex = 0;
            lblFrom.Text = "Từ ngày";
            // 
            // dtpFrom
            // 
            dtpFrom.CustomFormat = "dd/MM/yyyy";
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.Location = new Point(83, 14);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(110, 27);
            dtpFrom.TabIndex = 1;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Location = new Point(205, 18);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(72, 20);
            lblTo.TabIndex = 2;
            lblTo.Text = "Đến ngày";
            // 
            // dtpTo
            // 
            dtpTo.CustomFormat = "dd/MM/yyyy";
            dtpTo.Format = DateTimePickerFormat.Custom;
            dtpTo.Location = new Point(283, 14);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(110, 27);
            dtpTo.TabIndex = 3;
            // 
            // lblKieu
            // 
            lblKieu.AutoSize = true;
            lblKieu.Location = new Point(405, 18);
            lblKieu.Name = "lblKieu";
            lblKieu.Size = new Size(103, 20);
            lblKieu.TabIndex = 4;
            lblKieu.Text = "Kiểu thống kê:";
            // 
            // cboKieuThongKe
            // 
            cboKieuThongKe.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKieuThongKe.Location = new Point(514, 14);
            cboKieuThongKe.Name = "cboKieuThongKe";
            cboKieuThongKe.Size = new Size(140, 28);
            cboKieuThongKe.TabIndex = 5;
            // 
            // btnXemBaoCao
            // 
            btnXemBaoCao.Location = new Point(665, 14);
            btnXemBaoCao.Name = "btnXemBaoCao";
            btnXemBaoCao.Size = new Size(110, 28);
            btnXemBaoCao.TabIndex = 6;
            btnXemBaoCao.Text = "Xem báo cáo";
            btnXemBaoCao.UseVisualStyleBackColor = true;
            btnXemBaoCao.Click += btnXemBaoCao_Click;
            // 
            // btnXemBieuDo
            // 
            btnXemBieuDo.Location = new Point(665, 44);
            btnXemBieuDo.Name = "btnXemBieuDo";
            btnXemBieuDo.Size = new Size(110, 28);
            btnXemBieuDo.TabIndex = 9;
            btnXemBieuDo.Text = "Xem biểu đồ";
            btnXemBieuDo.UseVisualStyleBackColor = true;
            btnXemBieuDo.Click += btnXemBieuDo_Click;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(15, 48);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(73, 20);
            lblSearch.TabIndex = 7;
            lblSearch.Text = "Tìm kiếm:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(93, 45);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(230, 27);
            txtSearch.TabIndex = 8;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabTongQuan);
            tabControl.Controls.Add(tabNhanVien);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 80);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(800, 420);
            tabControl.TabIndex = 0;
            // 
            // tabTongQuan
            // 
            tabTongQuan.Controls.Add(dgvTongQuan);
            tabTongQuan.Controls.Add(panelSummaryTongQuan);
            tabTongQuan.Location = new Point(4, 29);
            tabTongQuan.Name = "tabTongQuan";
            tabTongQuan.Padding = new Padding(3);
            tabTongQuan.Size = new Size(792, 387);
            tabTongQuan.TabIndex = 0;
            tabTongQuan.Text = "Doanh thu tổng";
            tabTongQuan.UseVisualStyleBackColor = true;
            // 
            // dgvTongQuan
            // 
            dgvTongQuan.AllowUserToAddRows = false;
            dgvTongQuan.AllowUserToDeleteRows = false;
            dgvTongQuan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTongQuan.ColumnHeadersHeight = 29;
            dgvTongQuan.Dock = DockStyle.Fill;
            dgvTongQuan.Location = new Point(3, 3);
            dgvTongQuan.Name = "dgvTongQuan";
            dgvTongQuan.ReadOnly = true;
            dgvTongQuan.RowHeadersWidth = 51;
            dgvTongQuan.RowTemplate.Height = 25;
            dgvTongQuan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTongQuan.Size = new Size(786, 341);
            dgvTongQuan.TabIndex = 0;
            // 
            // panelSummaryTongQuan
            // 
            panelSummaryTongQuan.BackColor = Color.WhiteSmoke;
            panelSummaryTongQuan.Controls.Add(btnXuatPDF);
            panelSummaryTongQuan.Controls.Add(btnXuatExcel);
            panelSummaryTongQuan.Controls.Add(lblTongThu);
            panelSummaryTongQuan.Controls.Add(lblSoHoaDon);
            panelSummaryTongQuan.Dock = DockStyle.Bottom;
            panelSummaryTongQuan.Location = new Point(3, 344);
            panelSummaryTongQuan.Name = "panelSummaryTongQuan";
            panelSummaryTongQuan.Size = new Size(786, 40);
            panelSummaryTongQuan.TabIndex = 1;
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.Location = new Point(522, 8);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(108, 29);
            btnXuatExcel.TabIndex = 2;
            btnXuatExcel.Text = "Xuất Excel";
            btnXuatExcel.UseVisualStyleBackColor = true;
            btnXuatExcel.Click += btnXuatExcel_Click;
            // 
            // lblTongThu
            // 
            lblTongThu.AutoSize = true;
            lblTongThu.Location = new Point(10, 12);
            lblTongThu.Name = "lblTongThu";
            lblTongThu.Size = new Size(127, 20);
            lblTongThu.TabIndex = 0;
            lblTongThu.Text = "Tổng doanh thu: -";
            // 
            // lblSoHoaDon
            // 
            lblSoHoaDon.AutoSize = true;
            lblSoHoaDon.Location = new Point(300, 12);
            lblSoHoaDon.Name = "lblSoHoaDon";
            lblSoHoaDon.Size = new Size(98, 20);
            lblSoHoaDon.TabIndex = 1;
            lblSoHoaDon.Text = "Số hóa đơn: -";
            // 
            // tabNhanVien
            // 
            tabNhanVien.Controls.Add(dgvNhanVien);
            tabNhanVien.Controls.Add(panelSummaryNhanVien);
            tabNhanVien.Location = new Point(4, 29);
            tabNhanVien.Name = "tabNhanVien";
            tabNhanVien.Padding = new Padding(3);
            tabNhanVien.Size = new Size(792, 387);
            tabNhanVien.TabIndex = 1;
            tabNhanVien.Text = "Theo nhân viên";
            tabNhanVien.UseVisualStyleBackColor = true;
            // 
            // dgvNhanVien
            // 
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.AllowUserToDeleteRows = false;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.ColumnHeadersHeight = 29;
            dgvNhanVien.Dock = DockStyle.Fill;
            dgvNhanVien.Location = new Point(3, 3);
            dgvNhanVien.Name = "dgvNhanVien";
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.RowHeadersWidth = 51;
            dgvNhanVien.RowTemplate.Height = 25;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.Size = new Size(786, 341);
            dgvNhanVien.TabIndex = 0;
            // 
            // panelSummaryNhanVien
            // 
            panelSummaryNhanVien.BackColor = Color.WhiteSmoke;
            panelSummaryNhanVien.Controls.Add(lblTongThuNhanVien);
            panelSummaryNhanVien.Controls.Add(lblSoNhanVien);
            panelSummaryNhanVien.Dock = DockStyle.Bottom;
            panelSummaryNhanVien.Location = new Point(3, 344);
            panelSummaryNhanVien.Name = "panelSummaryNhanVien";
            panelSummaryNhanVien.Size = new Size(786, 40);
            panelSummaryNhanVien.TabIndex = 1;
            // 
            // lblTongThuNhanVien
            // 
            lblTongThuNhanVien.AutoSize = true;
            lblTongThuNhanVien.Location = new Point(10, 12);
            lblTongThuNhanVien.Name = "lblTongThuNhanVien";
            lblTongThuNhanVien.Size = new Size(127, 20);
            lblTongThuNhanVien.TabIndex = 0;
            lblTongThuNhanVien.Text = "Tổng doanh thu: -";
            // 
            // lblSoNhanVien
            // 
            lblSoNhanVien.AutoSize = true;
            lblSoNhanVien.Location = new Point(300, 12);
            lblSoNhanVien.Name = "lblSoNhanVien";
            lblSoNhanVien.Size = new Size(152, 20);
            lblSoNhanVien.TabIndex = 1;
            lblSoNhanVien.Text = "Số nhân viên có HĐ: -";
            // 
            // btnXuatPDF
            // 
            btnXuatPDF.Location = new Point(658, 6);
            btnXuatPDF.Name = "btnXuatPDF";
            btnXuatPDF.Size = new Size(108, 29);
            btnXuatPDF.TabIndex = 3;
            btnXuatPDF.Text = "Xuất PDF";
            btnXuatPDF.UseVisualStyleBackColor = true;
            btnXuatPDF.Click += btnXuatPDF_Click;
            // 
            // FormBaoCaoDoanhThu
            // 
            ClientSize = new Size(800, 500);
            Controls.Add(tabControl);
            Controls.Add(panelFilter);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormBaoCaoDoanhThu";
            Text = "Báo cáo doanh thu";
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            tabControl.ResumeLayout(false);
            tabTongQuan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTongQuan).EndInit();
            panelSummaryTongQuan.ResumeLayout(false);
            panelSummaryTongQuan.PerformLayout();
            tabNhanVien.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvNhanVien).EndInit();
            panelSummaryNhanVien.ResumeLayout(false);
            panelSummaryNhanVien.PerformLayout();
            ResumeLayout(false);
        }
        private Button btnXuatExcel;
        private Button btnXuatPDF;
    }
}
