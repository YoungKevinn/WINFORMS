namespace Client_DoMInhKhoa.Forms
{
    partial class FormNhatKyHeThong
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            panelFilter = new Panel();
            chkOnlyLogin = new CheckBox();
            btnSearch = new Button();
            txtKeyword = new TextBox();
            label3 = new Label();
            dtpTo = new DateTimePicker();
            label2 = new Label();
            dtpFrom = new DateTimePicker();
            label1 = new Label();
            dgvAuditLog = new DataGridView();
            panelFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuditLog).BeginInit();
            SuspendLayout();
            // 
            // panelFilter
            // 
            panelFilter.BackColor = Color.WhiteSmoke;
            panelFilter.Controls.Add(chkOnlyLogin);
            panelFilter.Controls.Add(btnSearch);
            panelFilter.Controls.Add(txtKeyword);
            panelFilter.Controls.Add(label3);
            panelFilter.Controls.Add(dtpTo);
            panelFilter.Controls.Add(label2);
            panelFilter.Controls.Add(dtpFrom);
            panelFilter.Controls.Add(label1);
            panelFilter.Dock = DockStyle.Top;
            panelFilter.Location = new Point(0, 0);
            panelFilter.Margin = new Padding(3, 4, 3, 4);
            panelFilter.Name = "panelFilter";
            panelFilter.Padding = new Padding(11, 13, 11, 13);
            panelFilter.Size = new Size(1125, 93);
            panelFilter.TabIndex = 0;
            // 
            // chkOnlyLogin
            // 
            chkOnlyLogin.AutoSize = true;
            chkOnlyLogin.Location = new Point(592, 16);
            chkOnlyLogin.Margin = new Padding(3, 4, 3, 4);
            chkOnlyLogin.Name = "chkOnlyLogin";
            chkOnlyLogin.Size = new Size(206, 24);
            chkOnlyLogin.TabIndex = 7;
            chkOnlyLogin.Text = "Chỉ hiển thị log đăng nhập";
            chkOnlyLogin.UseVisualStyleBackColor = true;
            chkOnlyLogin.CheckedChanged += chkOnlyLogin_CheckedChanged;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(460, 50);
            btnSearch.Margin = new Padding(3, 4, 3, 4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(103, 31);
            btnSearch.TabIndex = 6;
            btnSearch.Text = "Tìm";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(87, 52);
            txtKeyword.Margin = new Padding(3, 4, 3, 4);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new Size(354, 27);
            txtKeyword.TabIndex = 5;
            // 
            // label3
            // 
            label3.Location = new Point(12, 55);
            label3.Name = "label3";
            label3.Size = new Size(69, 20);
            label3.TabIndex = 4;
            label3.Text = "Từ khóa:";
            // 
            // dtpTo
            // 
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(400, 13);
            dtpTo.Margin = new Padding(3, 4, 3, 4);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(137, 27);
            dtpTo.TabIndex = 3;
            // 
            // label2
            // 
            label2.Location = new Point(320, 19);
            label2.Name = "label2";
            label2.Size = new Size(80, 20);
            label2.TabIndex = 2;
            label2.Text = "Đến ngày:";
            // 
            // dtpFrom
            // 
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(103, 13);
            dtpFrom.Margin = new Padding(3, 4, 3, 4);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(137, 27);
            dtpFrom.TabIndex = 1;
            // 
            // label1
            // 
            label1.Location = new Point(11, 19);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            label1.TabIndex = 0;
            label1.Text = "Từ ngày:";
            // 
            // dgvAuditLog
            // 
            dgvAuditLog.AllowUserToAddRows = false;
            dgvAuditLog.AllowUserToDeleteRows = false;
            dgvAuditLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAuditLog.BackgroundColor = Color.White;
            dgvAuditLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAuditLog.Dock = DockStyle.Fill;
            dgvAuditLog.Location = new Point(0, 93);
            dgvAuditLog.Margin = new Padding(3, 4, 3, 4);
            dgvAuditLog.MultiSelect = false;
            dgvAuditLog.Name = "dgvAuditLog";
            dgvAuditLog.ReadOnly = true;
            dgvAuditLog.RowHeadersVisible = false;
            dgvAuditLog.RowHeadersWidth = 51;
            dgvAuditLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAuditLog.Size = new Size(1125, 655);
            dgvAuditLog.TabIndex = 1;
            // 
            // FormNhatKyHeThong
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1125, 748);
            Controls.Add(dgvAuditLog);
            Controls.Add(panelFilter);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormNhatKyHeThong";
            Text = "Nhật ký hệ thống";
            Load += FormNhatKyHeThong_Load;
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuditLog).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.CheckBox chkOnlyLogin;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvAuditLog;
    }
}
