namespace Client_DoMInhKhoa.Forms
{
    partial class FormThanhToan
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTongTienLabel = new System.Windows.Forms.Label();
            this.lblTongTienVal = new System.Windows.Forms.Label();
            this.lblDaTraLabel = new System.Windows.Forms.Label();
            this.lblDaTraVal = new System.Windows.Forms.Label();
            this.lblConLaiLabel = new System.Windows.Forms.Label();
            this.lblConLaiVal = new System.Windows.Forms.Label();
            this.lblKhachDua = new System.Windows.Forms.Label();
            this.txtKhachDua = new System.Windows.Forms.TextBox();
            this.lblTienThuaLabel = new System.Windows.Forms.Label();
            this.lblTienThuaVal = new System.Windows.Forms.Label();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnXacNhan = new System.Windows.Forms.Button(); // Đã đổi tên chuẩn thành btnXacNhan
            this.label1 = new System.Windows.Forms.Label();
            this.cboPhuongThuc = new System.Windows.Forms.ComboBox();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(25)))), ((int)(((byte)(74)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(420, 50);
            this.panelHeader.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(134, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "THANH TOÁN";

            // 
            // lblTongTienLabel
            // 
            this.lblTongTienLabel.AutoSize = true;
            this.lblTongTienLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTongTienLabel.Location = new System.Drawing.Point(30, 70);
            this.lblTongTienLabel.Name = "lblTongTienLabel";
            this.lblTongTienLabel.Size = new System.Drawing.Size(104, 20);
            this.lblTongTienLabel.TabIndex = 1;
            this.lblTongTienLabel.Text = "Tổng hóa đơn:";

            // 
            // lblTongTienVal
            // 
            this.lblTongTienVal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTongTienVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTongTienVal.Location = new System.Drawing.Point(200, 70);
            this.lblTongTienVal.Name = "lblTongTienVal";
            this.lblTongTienVal.Size = new System.Drawing.Size(190, 25);
            this.lblTongTienVal.TabIndex = 2;
            this.lblTongTienVal.Text = "0 đ";

            // 
            // lblDaTraLabel
            // 
            this.lblDaTraLabel.AutoSize = true;
            this.lblDaTraLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblDaTraLabel.ForeColor = System.Drawing.Color.DimGray;
            this.lblDaTraLabel.Location = new System.Drawing.Point(30, 100);
            this.lblDaTraLabel.Name = "lblDaTraLabel";
            this.lblDaTraLabel.Size = new System.Drawing.Size(106, 20);
            this.lblDaTraLabel.TabIndex = 3;
            this.lblDaTraLabel.Text = "Đã thanh toán:";

            // 
            // lblDaTraVal
            // 
            this.lblDaTraVal.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblDaTraVal.ForeColor = System.Drawing.Color.DimGray;
            this.lblDaTraVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDaTraVal.Location = new System.Drawing.Point(200, 100);
            this.lblDaTraVal.Name = "lblDaTraVal";
            this.lblDaTraVal.Size = new System.Drawing.Size(190, 25);
            this.lblDaTraVal.TabIndex = 4;
            this.lblDaTraVal.Text = "0 đ";

            // 
            // lblConLaiLabel
            // 
            this.lblConLaiLabel.AutoSize = true;
            this.lblConLaiLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblConLaiLabel.Location = new System.Drawing.Point(30, 130);
            this.lblConLaiLabel.Name = "lblConLaiLabel";
            this.lblConLaiLabel.Size = new System.Drawing.Size(99, 20);
            this.lblConLaiLabel.TabIndex = 5;
            this.lblConLaiLabel.Text = "Cần trả ngay:";

            // 
            // lblConLaiVal
            // 
            this.lblConLaiVal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblConLaiVal.ForeColor = System.Drawing.Color.Firebrick;
            this.lblConLaiVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblConLaiVal.Location = new System.Drawing.Point(200, 125);
            this.lblConLaiVal.Name = "lblConLaiVal";
            this.lblConLaiVal.Size = new System.Drawing.Size(190, 30);
            this.lblConLaiVal.TabIndex = 6;
            this.lblConLaiVal.Text = "0 đ";

            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Phương thức:";

            // 
            // cboPhuongThuc
            // 
            this.cboPhuongThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhuongThuc.Location = new System.Drawing.Point(200, 177);
            this.cboPhuongThuc.Name = "cboPhuongThuc";
            this.cboPhuongThuc.Size = new System.Drawing.Size(190, 25);
            this.cboPhuongThuc.TabIndex = 8;
            this.cboPhuongThuc.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ" });

            // 
            // lblKhachDua
            // 
            this.lblKhachDua.AutoSize = true;
            this.lblKhachDua.Location = new System.Drawing.Point(30, 220);
            this.lblKhachDua.Name = "lblKhachDua";
            this.lblKhachDua.Size = new System.Drawing.Size(73, 17);
            this.lblKhachDua.TabIndex = 9;
            this.lblKhachDua.Text = "Khách đưa:";

            // 
            // txtKhachDua
            // 
            this.txtKhachDua.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtKhachDua.Location = new System.Drawing.Point(200, 217);
            this.txtKhachDua.Name = "txtKhachDua";
            this.txtKhachDua.Size = new System.Drawing.Size(190, 29);
            this.txtKhachDua.TabIndex = 0; // Focus vào đây đầu tiên
            this.txtKhachDua.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // lblTienThuaLabel
            // 
            this.lblTienThuaLabel.AutoSize = true;
            this.lblTienThuaLabel.Location = new System.Drawing.Point(30, 260);
            this.lblTienThuaLabel.Name = "lblTienThuaLabel";
            this.lblTienThuaLabel.Size = new System.Drawing.Size(74, 17);
            this.lblTienThuaLabel.TabIndex = 10;
            this.lblTienThuaLabel.Text = "Tiền trả lại:";

            // 
            // lblTienThuaVal
            // 
            this.lblTienThuaVal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTienThuaVal.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblTienThuaVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTienThuaVal.Location = new System.Drawing.Point(200, 255);
            this.lblTienThuaVal.Name = "lblTienThuaVal";
            this.lblTienThuaVal.Size = new System.Drawing.Size(190, 30);
            this.lblTienThuaVal.TabIndex = 11;
            this.lblTienThuaVal.Text = "0 đ";

            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.Gray;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(34, 310);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(110, 40);
            this.btnHuy.TabIndex = 12;
            this.btnHuy.Text = "Hủy bỏ";
            this.btnHuy.UseVisualStyleBackColor = false;

            // 
            // btnXacNhan
            // 
            this.btnXacNhan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(45)))), ((int)(((byte)(104)))));
            this.btnXacNhan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXacNhan.ForeColor = System.Drawing.Color.White;
            this.btnXacNhan.Location = new System.Drawing.Point(180, 310);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(210, 40);
            this.btnXacNhan.TabIndex = 13;
            this.btnXacNhan.Text = "Thanh toán";
            this.btnXacNhan.UseVisualStyleBackColor = false;

            // 
            // FormThanhToan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(420, 380);
            this.Controls.Add(this.cboPhuongThuc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnXacNhan);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.lblTienThuaVal);
            this.Controls.Add(this.lblTienThuaLabel);
            this.Controls.Add(this.txtKhachDua);
            this.Controls.Add(this.lblKhachDua);
            this.Controls.Add(this.lblConLaiVal);
            this.Controls.Add(this.lblConLaiLabel);
            this.Controls.Add(this.lblDaTraVal);
            this.Controls.Add(this.lblDaTraLabel);
            this.Controls.Add(this.lblTongTienVal);
            this.Controls.Add(this.lblTongTienLabel);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormThanhToan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thanh toán";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTongTienLabel;
        private System.Windows.Forms.Label lblTongTienVal;
        private System.Windows.Forms.Label lblDaTraLabel;
        private System.Windows.Forms.Label lblDaTraVal;
        private System.Windows.Forms.Label lblConLaiLabel;
        private System.Windows.Forms.Label lblConLaiVal;
        private System.Windows.Forms.Label lblKhachDua;
        private System.Windows.Forms.TextBox txtKhachDua;
        private System.Windows.Forms.Label lblTienThuaLabel;
        private System.Windows.Forms.Label lblTienThuaVal;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnXacNhan; // Đã đổi tên
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboPhuongThuc;
    }
}