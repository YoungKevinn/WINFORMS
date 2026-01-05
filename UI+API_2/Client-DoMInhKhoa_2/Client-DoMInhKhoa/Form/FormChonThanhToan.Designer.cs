using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    partial class FormChonThanhToan
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblTongCaption;
        private Label lblTong;

        private GroupBox grpPhuongThuc;
        private RadioButton rdoTienMat;
        private RadioButton rdoChuyenKhoan;

        private Label lblKhachDuaCaption;
        private TextBox txtKhachDua;

        private Label lblTienThoiCaption;
        private Label lblTienThoi;

        private Button btnOK;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblTongCaption = new Label();
            lblTong = new Label();

            grpPhuongThuc = new GroupBox();
            rdoTienMat = new RadioButton();
            rdoChuyenKhoan = new RadioButton();

            lblKhachDuaCaption = new Label();
            txtKhachDua = new TextBox();

            lblTienThoiCaption = new Label();
            lblTienThoi = new Label();

            btnOK = new Button();
            btnCancel = new Button();

            SuspendLayout();

            // Form
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 300);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Text = "Chọn thanh toán";

            // Title
            lblTitle.AutoSize = false;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 46;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.Padding = new Padding(12, 0, 12, 0);
            lblTitle.Text = "Thanh toán";

            // Tổng
            lblTongCaption.AutoSize = true;
            lblTongCaption.Location = new Point(18, 60);
            lblTongCaption.Text = "Tổng tiền:";

            lblTong.AutoSize = false;
            lblTong.Location = new Point(120, 56);
            lblTong.Size = new Size(320, 28);
            lblTong.TextAlign = ContentAlignment.MiddleRight;
            lblTong.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTong.ForeColor = Color.DarkRed;
            lblTong.Text = "0 đ";

            // Group phương thức
            grpPhuongThuc.Location = new Point(18, 92);
            grpPhuongThuc.Size = new Size(422, 74);
            grpPhuongThuc.Text = "Phương thức";

            rdoTienMat.Location = new Point(14, 30);
            rdoTienMat.Size = new Size(120, 28);
            rdoTienMat.Text = "Tiền mặt";
            rdoTienMat.Checked = true;

            rdoChuyenKhoan.Location = new Point(160, 30);
            rdoChuyenKhoan.Size = new Size(160, 28);
            rdoChuyenKhoan.Text = "Chuyển khoản";

            grpPhuongThuc.Controls.Add(rdoTienMat);
            grpPhuongThuc.Controls.Add(rdoChuyenKhoan);

            // Khách đưa
            lblKhachDuaCaption.AutoSize = true;
            lblKhachDuaCaption.Location = new Point(18, 178);
            lblKhachDuaCaption.Text = "Khách đưa:";

            txtKhachDua.Location = new Point(120, 174);
            txtKhachDua.Size = new Size(220, 30);
            txtKhachDua.TextAlign = HorizontalAlignment.Left; // muốn canh phải thì đổi Right

            // Tiền thối
            lblTienThoiCaption.AutoSize = true;
            lblTienThoiCaption.Location = new Point(18, 214);
            lblTienThoiCaption.Text = "Tiền thối:";

            lblTienThoi.AutoSize = false;
            lblTienThoi.Location = new Point(120, 210);
            lblTienThoi.Size = new Size(320, 28);
            lblTienThoi.TextAlign = ContentAlignment.MiddleRight;
            lblTienThoi.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTienThoi.Text = "0 đ";

            // Buttons
            btnOK.Text = "Xác nhận";
            btnOK.Size = new Size(120, 34);
            btnOK.Location = new Point(190, 252);

            btnCancel.Text = "Hủy";
            btnCancel.Size = new Size(90, 34);
            btnCancel.Location = new Point(320, 252);

            AcceptButton = btnOK;
            CancelButton = btnCancel;

            // Add controls
            Controls.Add(lblTitle);
            Controls.Add(lblTongCaption);
            Controls.Add(lblTong);
            Controls.Add(grpPhuongThuc);
            Controls.Add(lblKhachDuaCaption);
            Controls.Add(txtKhachDua);
            Controls.Add(lblTienThoiCaption);
            Controls.Add(lblTienThoi);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
