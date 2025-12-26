namespace Client_DoMInhKhoa.Forms
{
    partial class FormDangNhapAdmin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelLeft = new Panel();
            lblSlogan = new Label();
            lblAppName = new Label();
            panelLogo = new Panel();
            lblTitle = new Label();
            lblSubTitle = new Label();
            lblTenDangNhap = new Label();
            txtTenDangNhap = new TextBox();
            panelUnderlineUser = new Panel();
            lblMatKhau = new Label();
            txtMatKhau = new TextBox();
            panelUnderlinePass = new Panel();
            chkNhoTaiKhoan = new CheckBox();
            btnDangNhap = new Button();
            btnThoat = new Button();
            btnToggleMatKhau = new Button();
            panelLeft.SuspendLayout();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.FromArgb(10, 25, 74);
            panelLeft.Controls.Add(lblSlogan);
            panelLeft.Controls.Add(lblAppName);
            panelLeft.Controls.Add(panelLogo);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Margin = new Padding(3, 4, 3, 4);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(297, 560);
            panelLeft.TabIndex = 0;
            // 
            // lblSlogan
            // 
            lblSlogan.AutoSize = true;
            lblSlogan.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblSlogan.ForeColor = Color.FromArgb(176, 196, 222);
            lblSlogan.Location = new Point(37, 360);
            lblSlogan.MaximumSize = new Size(240, 0);
            lblSlogan.Name = "lblSlogan";
            lblSlogan.Size = new Size(234, 80);
            lblSlogan.TabIndex = 2;
            lblSlogan.Text = "Nếu em là đường, anh nguyện làm cà phê.\r\n Để mỗi ngày chúng ta hòa quyện vào nhau.";
            lblSlogan.Click += lblSlogan_Click;
            // 
            // lblAppName
            // 
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblAppName.ForeColor = Color.White;
            lblAppName.Location = new Point(37, 280);
            lblAppName.Name = "lblAppName";
            lblAppName.Size = new Size(196, 37);
            lblAppName.TabIndex = 1;
            lblAppName.Text = "Cafe Manager";
            // 
            // panelLogo
            // 
            panelLogo.BackColor = Color.FromArgb(24, 119, 242);
            panelLogo.Location = new Point(51, 75);
            panelLogo.Margin = new Padding(3, 4, 3, 4);
            panelLogo.Name = "panelLogo";
            panelLogo.Size = new Size(169, 168);
            panelLogo.TabIndex = 0;
            panelLogo.Paint += panelLogo_Paint;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(10, 25, 74);
            lblTitle.Location = new Point(331, 80);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(248, 37);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Đăng nhập Admin";
            // 
            // lblSubTitle
            // 
            lblSubTitle.AutoSize = true;
            lblSubTitle.Font = new Font("Segoe UI", 9F);
            lblSubTitle.ForeColor = Color.Gray;
            lblSubTitle.Location = new Point(334, 127);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(109, 20);
            lblSubTitle.TabIndex = 2;
            lblSubTitle.Text = "Admin logging";
            // 
            // lblTenDangNhap
            // 
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Font = new Font("Segoe UI", 9F);
            lblTenDangNhap.ForeColor = Color.DimGray;
            lblTenDangNhap.Location = new Point(334, 193);
            lblTenDangNhap.Name = "lblTenDangNhap";
            lblTenDangNhap.Size = new Size(107, 20);
            lblTenDangNhap.TabIndex = 3;
            lblTenDangNhap.Text = "Tên đăng nhập";
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.BorderStyle = BorderStyle.None;
            txtTenDangNhap.Font = new Font("Segoe UI", 10F);
            txtTenDangNhap.Location = new Point(334, 220);
            txtTenDangNhap.Margin = new Padding(3, 4, 3, 4);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(297, 23);
            txtTenDangNhap.TabIndex = 0;
            txtTenDangNhap.TextChanged += txtTenDangNhap_TextChanged;
            // 
            // panelUnderlineUser
            // 
            panelUnderlineUser.BackColor = Color.FromArgb(24, 119, 242);
            panelUnderlineUser.Location = new Point(334, 248);
            panelUnderlineUser.Margin = new Padding(3, 4, 3, 4);
            panelUnderlineUser.Name = "panelUnderlineUser";
            panelUnderlineUser.Size = new Size(297, 3);
            panelUnderlineUser.TabIndex = 5;
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Font = new Font("Segoe UI", 9F);
            lblMatKhau.ForeColor = Color.DimGray;
            lblMatKhau.Location = new Point(334, 273);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(70, 20);
            lblMatKhau.TabIndex = 6;
            lblMatKhau.Text = "Mật khẩu";
            // 
            // txtMatKhau
            // 
            txtMatKhau.BorderStyle = BorderStyle.None;
            txtMatKhau.Font = new Font("Segoe UI", 10F);
            txtMatKhau.Location = new Point(334, 300);
            txtMatKhau.Margin = new Padding(3, 4, 3, 4);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.PasswordChar = '●';
            txtMatKhau.Size = new Size(297, 23);
            txtMatKhau.TabIndex = 1;
            // 
            // panelUnderlinePass
            // 
            panelUnderlinePass.BackColor = Color.FromArgb(24, 119, 242);
            panelUnderlinePass.Location = new Point(334, 328);
            panelUnderlinePass.Margin = new Padding(3, 4, 3, 4);
            panelUnderlinePass.Name = "panelUnderlinePass";
            panelUnderlinePass.Size = new Size(297, 3);
            panelUnderlinePass.TabIndex = 8;
            // 
            // chkNhoTaiKhoan
            // 
            chkNhoTaiKhoan.AutoSize = true;
            chkNhoTaiKhoan.Checked = true;
            chkNhoTaiKhoan.CheckState = CheckState.Checked;
            chkNhoTaiKhoan.Font = new Font("Segoe UI", 9F);
            chkNhoTaiKhoan.ForeColor = Color.DimGray;
            chkNhoTaiKhoan.Location = new Point(334, 347);
            chkNhoTaiKhoan.Margin = new Padding(3, 4, 3, 4);
            chkNhoTaiKhoan.Name = "chkNhoTaiKhoan";
            chkNhoTaiKhoan.Size = new Size(124, 24);
            chkNhoTaiKhoan.TabIndex = 2;
            chkNhoTaiKhoan.Text = "Nhớ tài khoản";
            chkNhoTaiKhoan.UseVisualStyleBackColor = true;
            // 
            // btnDangNhap
            // 
            btnDangNhap.BackColor = Color.FromArgb(24, 119, 242);
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Location = new Point(334, 400);
            btnDangNhap.Margin = new Padding(3, 4, 3, 4);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Size = new Size(297, 48);
            btnDangNhap.TabIndex = 3;
            btnDangNhap.Text = "ĐĂNG NHẬP";
            btnDangNhap.UseVisualStyleBackColor = false;
            btnDangNhap.Click += btnDangNhap_Click;
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.White;
            btnThoat.FlatAppearance.BorderColor = Color.LightGray;
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.Font = new Font("Segoe UI", 9F);
            btnThoat.ForeColor = Color.DimGray;
            btnThoat.Location = new Point(528, 467);
            btnThoat.Margin = new Padding(3, 4, 3, 4);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(103, 37);
            btnThoat.TabIndex = 4;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = false;
            btnThoat.Click += btnThoat_Click;
            // 
            // btnToggleMatKhau
            // 
            btnToggleMatKhau.Cursor = Cursors.Hand;
            btnToggleMatKhau.FlatAppearance.BorderSize = 0;
            btnToggleMatKhau.FlatStyle = FlatStyle.Flat;
            btnToggleMatKhau.Location = new Point(587, 287);
            btnToggleMatKhau.Name = "btnToggleMatKhau";
            btnToggleMatKhau.Size = new Size(57, 36);
            btnToggleMatKhau.TabIndex = 11;
            btnToggleMatKhau.Text = "Hiện";
            btnToggleMatKhau.UseVisualStyleBackColor = true;
            btnToggleMatKhau.Click += btnToggleMatKhau_Click;
            // 
            // FormDangNhap
            // 
            AcceptButton = btnDangNhap;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(686, 560);
            Controls.Add(btnToggleMatKhau);
            Controls.Add(btnThoat);
            Controls.Add(btnDangNhap);
            Controls.Add(chkNhoTaiKhoan);
            Controls.Add(panelUnderlinePass);
            Controls.Add(txtMatKhau);
            Controls.Add(lblMatKhau);
            Controls.Add(panelUnderlineUser);
            Controls.Add(txtTenDangNhap);
            Controls.Add(lblTenDangNhap);
            Controls.Add(lblSubTitle);
            Controls.Add(lblTitle);
            Controls.Add(panelLeft);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "FormDangNhap";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập hệ thống";
            Load += FormDangNhap_Load;
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Label lblSlogan;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTenDangNhap;
        private System.Windows.Forms.TextBox txtTenDangNhap;
        private System.Windows.Forms.Panel panelUnderlineUser;
        private System.Windows.Forms.Label lblMatKhau;
        private System.Windows.Forms.TextBox txtMatKhau;
        private System.Windows.Forms.Panel panelUnderlinePass;
        private System.Windows.Forms.CheckBox chkNhoTaiKhoan;
        private System.Windows.Forms.Button btnDangNhap;
        private System.Windows.Forms.Button btnThoat;
        private Button btnToggleMatKhau;
    }
}