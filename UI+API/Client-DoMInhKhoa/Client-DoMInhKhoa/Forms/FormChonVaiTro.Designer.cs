namespace Client_DoMInhKhoa.Forms
{
    partial class FormChonVaiTro
    {
        private System.ComponentModel.IContainer components = null;

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
            panelLeft = new Panel();
            lblSlogan = new Label();
            lblAppName = new Label();
            panelRight = new Panel();
            btnAdmin = new Button();
            btnNhanVien = new Button();
            lblTitle = new Label();
            panelLeft.SuspendLayout();
            panelRight.SuspendLayout();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.FromArgb(10, 25, 74);
            panelLeft.Controls.Add(lblSlogan);
            panelLeft.Controls.Add(lblAppName);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Margin = new Padding(3, 4, 3, 4);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(297, 533);
            panelLeft.TabIndex = 0;
            // 
            // lblSlogan
            // 
            lblSlogan.AutoSize = true;
            lblSlogan.Font = new Font("Segoe UI", 9F);
            lblSlogan.ForeColor = Color.Gainsboro;
            lblSlogan.Location = new Point(40, 120);
            lblSlogan.Name = "lblSlogan";
            lblSlogan.Size = new Size(230, 60);
            lblSlogan.TabIndex = 1;
            lblSlogan.Text = "Quản lý quán cà phê thông minh,\r\nnhanh và trực quan cho chủ quán.\r\n\r\n";
            // 
            // lblAppName
            // 
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblAppName.ForeColor = Color.White;
            lblAppName.Location = new Point(37, 53);
            lblAppName.Name = "lblAppName";
            lblAppName.Size = new Size(187, 37);
            lblAppName.TabIndex = 0;
            lblAppName.Text = "Cafe Manager";
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.White;
            panelRight.Controls.Add(btnAdmin);
            panelRight.Controls.Add(btnNhanVien);
            panelRight.Controls.Add(lblTitle);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(297, 0);
            panelRight.Margin = new Padding(3, 4, 3, 4);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(503, 533);
            panelRight.TabIndex = 1;
            // 
            // btnAdmin
            // 
            btnAdmin.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnAdmin.Location = new Point(73, 280);
            btnAdmin.Margin = new Padding(3, 4, 3, 4);
            btnAdmin.Name = "btnAdmin";
            btnAdmin.Size = new Size(354, 80);
            btnAdmin.TabIndex = 2;
            btnAdmin.Text = "Quản trị Admin";
            btnAdmin.UseVisualStyleBackColor = true;
            btnAdmin.Click += btnAdmin_Click;
            // 
            // btnNhanVien
            // 
            btnNhanVien.BackColor = Color.White;
            btnNhanVien.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnNhanVien.Location = new Point(73, 173);
            btnNhanVien.Margin = new Padding(3, 4, 3, 4);
            btnNhanVien.Name = "btnNhanVien";
            btnNhanVien.Size = new Size(354, 80);
            btnNhanVien.TabIndex = 1;
            btnNhanVien.Text = "Nhân viên bán hàng";
            btnNhanVien.UseVisualStyleBackColor = false;
            btnNhanVien.Click += btnNhanVien_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(10, 25, 74);
            lblTitle.Location = new Point(69, 80);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(353, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Chọn chế độ sử dụng hệ thống";
            // 
            // FormChonVaiTro
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 533);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "FormChonVaiTro";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cafe Manager - Chọn chế độ";
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            panelRight.ResumeLayout(false);
            panelRight.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblSlogan;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.Button btnNhanVien;
        private System.Windows.Forms.Label lblTitle;
    }
}