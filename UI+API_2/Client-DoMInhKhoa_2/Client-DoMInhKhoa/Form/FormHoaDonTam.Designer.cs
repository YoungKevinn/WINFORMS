using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    partial class FormHoaDonTam
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblSub;
        private DataGridView dgv;

        private Panel pnlBottom;
        private Label lblTong;
        private Button btnIn;
        private Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblSub = new Label();
            dgv = new DataGridView();

            pnlBottom = new Panel();
            lblTong = new Label();
            btnIn = new Button();
            btnDong = new Button();

            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();

            // ===== Form =====
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(760, 560);
            MinimumSize = new Size(720, 520);
            Text = "Hóa đơn tạm";
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10F);

            // ===== lblTitle =====
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 44;
            lblTitle.Padding = new Padding(10, 0, 10, 0);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Text = "HÓA ĐƠN TẠM";

            // ===== lblSub =====
            lblSub.Dock = DockStyle.Top;
            lblSub.Height = 28;
            lblSub.Padding = new Padding(10, 0, 10, 0);
            lblSub.TextAlign = ContentAlignment.MiddleLeft;
            lblSub.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblSub.ForeColor = Color.DimGray;
            lblSub.Text = "Đơn gọi: #0";

            // ===== dgv =====
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.ColumnHeadersHeight = 32;
            dgv.RowTemplate.Height = 28;
            dgv.EnableHeadersVisualStyles = true;

            dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "STT",
                Name = "colSTT",
                FillWeight = 10,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Món",
                Name = "colTen",
                FillWeight = 46,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "SL",
                Name = "colSL",
                FillWeight = 12,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Đơn giá",
                Name = "colDG",
                FillWeight = 16,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Thành tiền",
                Name = "colTT",
                FillWeight = 16,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            // ===== pnlBottom =====
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Height = 64;
            pnlBottom.Padding = new Padding(10);
            pnlBottom.BackColor = Color.WhiteSmoke;

            // btnIn
            btnIn.Text = "In";
            btnIn.Size = new Size(90, 34);
            btnIn.Location = new Point(10, 15);
            btnIn.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

            // btnDong
            btnDong.Text = "Đóng";
            btnDong.Size = new Size(90, 34);
            btnDong.Location = new Point(110, 15);
            btnDong.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

            // lblTong
            lblTong.Size = new Size(320, 34);
            lblTong.Location = new Point(760 - 10 - 320, 15); // default theo size form
            lblTong.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            lblTong.TextAlign = ContentAlignment.MiddleRight;
            lblTong.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTong.ForeColor = Color.DarkRed;
            lblTong.Text = "Tổng: 0 đ";

            pnlBottom.Controls.Add(btnIn);
            pnlBottom.Controls.Add(btnDong);
            pnlBottom.Controls.Add(lblTong);

            // ===== add controls =====
            Controls.Add(dgv);
            Controls.Add(pnlBottom);
            Controls.Add(lblSub);
            Controls.Add(lblTitle);

            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            ResumeLayout(false);
        }
    }
}
