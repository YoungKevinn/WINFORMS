using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    partial class FormThanhToanQR
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTongTien;
        private PictureBox picQR;
        private Button btnDaNhanTien;
        private Button btnHuy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTongTien = new Label();
            picQR = new PictureBox();
            btnDaNhanTien = new Button();
            btnHuy = new Button();

            ((System.ComponentModel.ISupportInitialize)picQR).BeginInit();
            SuspendLayout();

            // lblTongTien
            lblTongTien.Dock = DockStyle.Top;
            lblTongTien.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblTongTien.Location = new Point(0, 0);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(420, 52);
            lblTongTien.TabIndex = 0;
            lblTongTien.Text = "Tổng tiền: 0 đ";
            lblTongTien.TextAlign = ContentAlignment.MiddleCenter;

            // btnHuy (dưới cùng)
            btnHuy.Dock = DockStyle.Bottom;
            btnHuy.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnHuy.Location = new Point(0, 518);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(420, 42);
            btnHuy.TabIndex = 3;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;

            // btnDaNhanTien (nằm trên nút hủy)
            btnDaNhanTien.Dock = DockStyle.Bottom;
            btnDaNhanTien.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnDaNhanTien.Location = new Point(0, 472);
            btnDaNhanTien.Name = "btnDaNhanTien";
            btnDaNhanTien.Size = new Size(420, 46);
            btnDaNhanTien.TabIndex = 2;
            btnDaNhanTien.Text = "Đã nhận tiền";
            btnDaNhanTien.UseVisualStyleBackColor = true;

            // picQR (ở giữa)
            picQR.Dock = DockStyle.Fill;
            picQR.Location = new Point(0, 52);
            picQR.Name = "picQR";
            picQR.Size = new Size(420, 420);
            picQR.SizeMode = PictureBoxSizeMode.Zoom;
            picQR.TabIndex = 1;
            picQR.TabStop = false;
            picQR.BackColor = System.Drawing.Color.White;

            // FormThanhToanQR
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 560);
            Controls.Add(picQR);
            Controls.Add(btnDaNhanTien);
            Controls.Add(btnHuy);
            Controls.Add(lblTongTien);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormThanhToanQR";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thanh toán QR";

            ((System.ComponentModel.ISupportInitialize)picQR).EndInit();
            ResumeLayout(false);
        }
    }
}
