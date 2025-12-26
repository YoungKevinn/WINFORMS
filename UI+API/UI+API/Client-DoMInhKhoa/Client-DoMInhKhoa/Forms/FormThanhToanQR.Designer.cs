namespace Client_DoMInhKhoa.Forms
{
    partial class FormThanhToanQR
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.PictureBox picQR;
        private System.Windows.Forms.Button btnDaNhanTien;
        private System.Windows.Forms.Button btnHuy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTongTien = new System.Windows.Forms.Label();
            this.picQR = new System.Windows.Forms.PictureBox();
            this.btnDaNhanTien = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTongTien
            // 
            this.lblTongTien.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.Location = new System.Drawing.Point(0, 0);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(404, 48);
            this.lblTongTien.TabIndex = 0;
            this.lblTongTien.Text = "Tổng tiền: 0 đ";
            this.lblTongTien.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picQR
            // 
            this.picQR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picQR.Location = new System.Drawing.Point(0, 48);
            this.picQR.Name = "picQR";
            this.picQR.Size = new System.Drawing.Size(404, 414);
            this.picQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picQR.TabIndex = 1;
            this.picQR.TabStop = false;
            // 
            // btnDaNhanTien
            // 
            this.btnDaNhanTien.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDaNhanTien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDaNhanTien.Location = new System.Drawing.Point(0, 462);
            this.btnDaNhanTien.Name = "btnDaNhanTien";
            this.btnDaNhanTien.Size = new System.Drawing.Size(404, 44);
            this.btnDaNhanTien.TabIndex = 2;
            this.btnDaNhanTien.Text = "Đã nhận tiền";
            this.btnDaNhanTien.UseVisualStyleBackColor = true;
            // 
            // btnHuy
            // 
            this.btnHuy.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnHuy.Location = new System.Drawing.Point(0, 506);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(404, 40);
            this.btnHuy.TabIndex = 3;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            // 
            // FormThanhToanQR
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 546);
            this.Controls.Add(this.picQR);
            this.Controls.Add(this.btnDaNhanTien);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.lblTongTien);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormThanhToanQR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thanh toán QR";
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
