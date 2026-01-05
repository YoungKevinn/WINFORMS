#nullable disable
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormThanhToanTungPhan
    {
        private IContainer components = null;

        private FlowLayoutPanel flp;
        private Panel pnlBottom;

        private Label lblTong;
        private Label lblPhuongThuc;
        private ComboBox cboPhuongThuc;

        private Label lblKhachDua;
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
            flp = new FlowLayoutPanel();
            pnlBottom = new Panel();
            lblTong = new Label();
            lblPhuongThuc = new Label();
            cboPhuongThuc = new ComboBox();
            lblKhachDua = new Label();
            txtKhachDua = new TextBox();
            lblTienThoiCaption = new Label();
            lblTienThoi = new Label();
            btnCancel = new Button();
            btnOK = new Button();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // flp
            // 
            flp.AutoScroll = true;
            flp.BackColor = Color.White;
            flp.Dock = DockStyle.Fill;
            flp.FlowDirection = FlowDirection.TopDown;
            flp.Location = new Point(0, 0);
            flp.Name = "flp";
            flp.Padding = new Padding(10);
            flp.Size = new Size(980, 440);
            flp.TabIndex = 0;
            flp.WrapContents = false;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.WhiteSmoke;
            pnlBottom.Controls.Add(lblTong);
            pnlBottom.Controls.Add(lblPhuongThuc);
            pnlBottom.Controls.Add(cboPhuongThuc);
            pnlBottom.Controls.Add(lblKhachDua);
            pnlBottom.Controls.Add(txtKhachDua);
            pnlBottom.Controls.Add(lblTienThoiCaption);
            pnlBottom.Controls.Add(lblTienThoi);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 440);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Padding = new Padding(12, 10, 12, 10);
            pnlBottom.Size = new Size(980, 120);
            pnlBottom.TabIndex = 1;
            // 
            // lblTong
            // 
            lblTong.AutoSize = true;
            lblTong.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTong.Location = new Point(14, 16);
            lblTong.Name = "lblTong";
            lblTong.Size = new Size(174, 28);
            lblTong.TabIndex = 0;
            lblTong.Text = "Tổng cần trả: 0 đ";
            // 
            // lblPhuongThuc
            // 
            lblPhuongThuc.AutoSize = true;
            lblPhuongThuc.Location = new Point(14, 54);
            lblPhuongThuc.Name = "lblPhuongThuc";
            lblPhuongThuc.Size = new Size(113, 23);
            lblPhuongThuc.TabIndex = 1;
            lblPhuongThuc.Text = "Phương thức:";
            // 
            // cboPhuongThuc
            // 
            cboPhuongThuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhuongThuc.FormattingEnabled = true;
            cboPhuongThuc.Location = new Point(130, 50);
            cboPhuongThuc.Name = "cboPhuongThuc";
            cboPhuongThuc.Size = new Size(240, 31);
            cboPhuongThuc.TabIndex = 2;
            // 
            // lblKhachDua
            // 
            lblKhachDua.AutoSize = true;
            lblKhachDua.Location = new Point(390, 50);
            lblKhachDua.Name = "lblKhachDua";
            lblKhachDua.Size = new Size(95, 23);
            lblKhachDua.TabIndex = 3;
            lblKhachDua.Text = "Khách đưa:";
            // 
            // txtKhachDua
            // 
            txtKhachDua.Location = new Point(491, 43);
            txtKhachDua.Name = "txtKhachDua";
            txtKhachDua.Size = new Size(200, 30);
            txtKhachDua.TabIndex = 4;
            txtKhachDua.Text = "0";
            // 
            // lblTienThoiCaption
            // 
            lblTienThoiCaption.AutoSize = true;
            lblTienThoiCaption.Location = new Point(700, 50);
            lblTienThoiCaption.Name = "lblTienThoiCaption";
            lblTienThoiCaption.Size = new Size(81, 23);
            lblTienThoiCaption.TabIndex = 5;
            lblTienThoiCaption.Text = "Tiền thối:";
            // 
            // lblTienThoi
            // 
            lblTienThoi.AutoSize = true;
            lblTienThoi.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTienThoi.Location = new Point(787, 46);
            lblTienThoi.Name = "lblTienThoi";
            lblTienThoi.Size = new Size(36, 23);
            lblTienThoi.TabIndex = 6;
            lblTienThoi.Text = "0 đ";
            lblTienThoi.Click += lblTienThoi_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(710, 72);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(110, 36);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.Location = new Point(840, 72);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(130, 36);
            btnOK.TabIndex = 8;
            btnOK.Text = "Xác nhận";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // FormThanhToanTungPhan
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(980, 560);
            Controls.Add(flp);
            Controls.Add(pnlBottom);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(980, 560);
            Name = "FormThanhToanTungPhan";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thanh toán từng phần";
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
        }
    }
}
