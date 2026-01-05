using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    partial class FormQuanLyBan
    {
        private IContainer components = null!;

        private Panel panelTitle = null!;
        private Label lblTitle = null!;

        private Panel panelFilter = null!;
        private TableLayoutPanel tlpFilter = null!;
        private Button btnFilterAll = null!;
        private Button btnFilterTrong = null!;
        private Button btnFilterDangPhucVu = null!;
        private Label lblThongKe = null!;

        // ✅ để đúng với logic: FlowLayoutPanel
        private FlowLayoutPanel panelTables = null!;

        private Panel panelBottom = null!;
        private Label lblBanFrom = null!;
        private ComboBox cboBanFrom = null!;
        private Label lblBanTo = null!;
        private ComboBox cboBanTo = null!;
        private Button btnChuyenBan = null!;
        private Button btnGopBan = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTitle = new Panel();
            lblTitle = new Label();

            panelFilter = new Panel();
            tlpFilter = new TableLayoutPanel();
            btnFilterAll = new Button();
            btnFilterTrong = new Button();
            btnFilterDangPhucVu = new Button();
            lblThongKe = new Label();

            panelTables = new FlowLayoutPanel();

            panelBottom = new Panel();
            lblBanFrom = new Label();
            cboBanFrom = new ComboBox();
            lblBanTo = new Label();
            cboBanTo = new ComboBox();
            btnChuyenBan = new Button();
            btnGopBan = new Button();

            panelTitle.SuspendLayout();
            panelFilter.SuspendLayout();
            tlpFilter.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();

            // 
            // panelTitle
            // 
            panelTitle.BackColor = Color.White;
            panelTitle.Controls.Add(lblTitle);
            panelTitle.Dock = DockStyle.Top;
            panelTitle.Location = new Point(0, 0);
            panelTitle.Name = "panelTitle";
            panelTitle.Size = new Size(1181, 60);
            panelTitle.TabIndex = 0;

            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(20, 0, 0, 0);
            lblTitle.Size = new Size(1181, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "QUẢN LÝ BÀN";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // panelFilter
            // 
            panelFilter.BackColor = Color.FromArgb(245, 247, 250);
            panelFilter.Dock = DockStyle.Top;
            panelFilter.Location = new Point(0, 60);
            panelFilter.Name = "panelFilter";
            panelFilter.Padding = new Padding(20, 9, 20, 9);
            panelFilter.Size = new Size(1181, 50);
            panelFilter.TabIndex = 1;
            panelFilter.Controls.Add(tlpFilter);

            // 
            // tlpFilter
            // 
            tlpFilter.ColumnCount = 4;
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));   // Tất cả
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));  // Còn trống
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));  // Đang dùng
            tlpFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));   // Thống kê (ăn phần còn lại)
            tlpFilter.RowCount = 1;
            tlpFilter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFilter.Dock = DockStyle.Fill;
            tlpFilter.Location = new Point(20, 9);
            tlpFilter.Name = "tlpFilter";
            tlpFilter.Size = new Size(1141, 32);
            tlpFilter.TabIndex = 0;

            tlpFilter.Controls.Add(btnFilterAll, 0, 0);
            tlpFilter.Controls.Add(btnFilterTrong, 1, 0);
            tlpFilter.Controls.Add(btnFilterDangPhucVu, 2, 0);
            tlpFilter.Controls.Add(lblThongKe, 3, 0);

            // 
            // btnFilterAll
            // 
            btnFilterAll.Dock = DockStyle.Fill;
            btnFilterAll.FlatStyle = FlatStyle.Flat;
            btnFilterAll.Margin = new Padding(0);
            btnFilterAll.Name = "btnFilterAll";
            btnFilterAll.TabIndex = 0;
            btnFilterAll.Text = "Tất cả";
            btnFilterAll.UseVisualStyleBackColor = true;

            // 
            // btnFilterTrong
            // 
            btnFilterTrong.Dock = DockStyle.Fill;
            btnFilterTrong.FlatStyle = FlatStyle.Flat;
            btnFilterTrong.Margin = new Padding(10, 0, 0, 0);
            btnFilterTrong.Name = "btnFilterTrong";
            btnFilterTrong.TabIndex = 1;
            btnFilterTrong.Text = "Còn trống";
            btnFilterTrong.UseVisualStyleBackColor = true;

            // 
            // btnFilterDangPhucVu
            // 
            btnFilterDangPhucVu.Dock = DockStyle.Fill;
            btnFilterDangPhucVu.FlatStyle = FlatStyle.Flat;
            btnFilterDangPhucVu.Margin = new Padding(10, 0, 0, 0);
            btnFilterDangPhucVu.Name = "btnFilterDangPhucVu";
            btnFilterDangPhucVu.TabIndex = 2;
            btnFilterDangPhucVu.Text = "Đang dùng";
            btnFilterDangPhucVu.UseVisualStyleBackColor = true;

            // 
            // lblThongKe
            // 
            lblThongKe.Dock = DockStyle.Fill;
            lblThongKe.AutoSize = false;
            lblThongKe.AutoEllipsis = true;
            lblThongKe.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblThongKe.ForeColor = Color.Gray;
            lblThongKe.Margin = new Padding(12, 0, 0, 0);
            lblThongKe.Name = "lblThongKe";
            lblThongKe.TabIndex = 3;
            lblThongKe.TextAlign = ContentAlignment.MiddleLeft;
            lblThongKe.Text = ""; // set trong code-behind

            // 
            // panelTables
            // 
            panelTables.AutoScroll = true;
            panelTables.BackColor = Color.White;
            panelTables.Dock = DockStyle.Fill;
            panelTables.Location = new Point(0, 110);
            panelTables.Name = "panelTables";
            panelTables.Padding = new Padding(40);
            panelTables.Size = new Size(1181, 430);
            panelTables.TabIndex = 2;

            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.White;
            panelBottom.Controls.Add(lblBanFrom);
            panelBottom.Controls.Add(cboBanFrom);
            panelBottom.Controls.Add(lblBanTo);
            panelBottom.Controls.Add(cboBanTo);
            panelBottom.Controls.Add(btnChuyenBan);
            panelBottom.Controls.Add(btnGopBan);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 540);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1181, 60);
            panelBottom.TabIndex = 3;

            // 
            // lblBanFrom
            // 
            lblBanFrom.AutoSize = true;
            lblBanFrom.Location = new Point(36, 20);
            lblBanFrom.Name = "lblBanFrom";
            lblBanFrom.Size = new Size(98, 23);
            lblBanFrom.TabIndex = 0;
            lblBanFrom.Text = "Bàn nguồn:";

            // 
            // cboBanFrom
            // 
            cboBanFrom.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBanFrom.Location = new Point(140, 15);
            cboBanFrom.Name = "cboBanFrom";
            cboBanFrom.Size = new Size(130, 31);
            cboBanFrom.TabIndex = 1;

            // 
            // lblBanTo
            // 
            lblBanTo.AutoSize = true;
            lblBanTo.Location = new Point(307, 20);
            lblBanTo.Name = "lblBanTo";
            lblBanTo.Size = new Size(80, 23);
            lblBanTo.TabIndex = 2;
            lblBanTo.Text = "Bàn đích:";

            // 
            // cboBanTo
            // 
            cboBanTo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBanTo.Location = new Point(405, 16);
            cboBanTo.Name = "cboBanTo";
            cboBanTo.Size = new Size(130, 31);
            cboBanTo.TabIndex = 3;

            // 
            // btnChuyenBan
            // 
            btnChuyenBan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnChuyenBan.Location = new Point(749, 15);
            btnChuyenBan.Name = "btnChuyenBan";
            btnChuyenBan.Size = new Size(110, 30);
            btnChuyenBan.TabIndex = 4;
            btnChuyenBan.Text = "Chuyển bàn";
            btnChuyenBan.UseVisualStyleBackColor = true;

            // 
            // btnGopBan
            // 
            btnGopBan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGopBan.Location = new Point(870, 15);
            btnGopBan.Name = "btnGopBan";
            btnGopBan.Size = new Size(110, 30);
            btnGopBan.TabIndex = 5;
            btnGopBan.Text = "Gộp bàn";
            btnGopBan.UseVisualStyleBackColor = true;

            // 
            // FormQuanLyBan
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1181, 600);

            Controls.Add(panelTables);
            Controls.Add(panelBottom);
            Controls.Add(panelFilter);
            Controls.Add(panelTitle);

            Font = new Font("Segoe UI", 10F);
            Name = "FormQuanLyBan";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý bàn";

            panelTitle.ResumeLayout(false);
            panelFilter.ResumeLayout(false);
            tlpFilter.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }
    }
}
