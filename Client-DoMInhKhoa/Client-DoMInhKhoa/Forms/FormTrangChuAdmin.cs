using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Client_DoMInhKhoa.Session;
using Client_DoMInhKhoa.Forms;
namespace Client_DoMInhKhoa.Forms
{
    public partial class FormTrangChuAdmin : Form
    {
        private Button _currentMenuButton;
        private readonly Color _menuNormalColor = Color.FromArgb(13, 45, 104);
        private readonly Color _menuActiveColor = Color.FromArgb(24, 119, 242);

        private Form _activeChildForm;
        private readonly Dictionary<string, Form> _childForms = new();

        public FormTrangChuAdmin()
        {
            InitializeComponent();

            // Hiển thị thông tin admin đang đăng nhập (nếu có lưu Session)
            if (!string.IsNullOrEmpty(SessionHienTai.TenDangNhap))
            {
                lblXinChao.Text =
                    $"Xin chào, {SessionHienTai.TenDangNhap} ({SessionHienTai.VaiTro})";
            }
            else
            {
                lblXinChao.Text = "Xin chào, Admin";
            }

            // Áp dụng style cho tất cả button trong panel menu
            foreach (var btn in panelMenu.Controls.OfType<Button>())
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = _menuNormalColor;
                btn.ForeColor = Color.White;
            }

            // (Optional) chọn sẵn tab đầu tiên
            //SetActiveMenuButton(btnQuanLyNhanVien);
            //OpenChildForm<FormQuanLyNhanVien>();
        }

        private void SetActiveMenuButton(Button btn)
        {
            if (_currentMenuButton != null)
            {
                _currentMenuButton.BackColor = _menuNormalColor;
                _currentMenuButton.ForeColor = Color.White;
            }

            _currentMenuButton = btn;
            _currentMenuButton.BackColor = _menuActiveColor;
            _currentMenuButton.ForeColor = Color.White;

            // ép UI vẽ ngay, tránh cảm giác delay
            _currentMenuButton.Refresh();
        }

        /// <summary>
        /// Mở form con kiểu T trong panelContent, có cache để không phải new nhiều lần.
        /// </summary>
        private void OpenChildForm<T>() where T : Form, new()
        {
            // Nếu form hiện tại đã là T thì thôi
            if (_activeChildForm is T)
                return;

            string key = typeof(T).Name;

            // Ẩn form đang hiển thị (nếu có)
            if (_activeChildForm != null)
            {
                _activeChildForm.Hide();
            }

            // Lấy form từ cache nếu đã tạo
            if (!_childForms.TryGetValue(key, out var form))
            {
                form = new T
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };

                _childForms[key] = form;
                panelContent.Controls.Add(form);
            }

            _activeChildForm = form;
            form.BringToFront();
            form.Show();
        }

        // ====== MENU EVENTS ======

        private void btnQuanLyNhanVien_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (_currentMenuButton == btn) return;

            SetActiveMenuButton(btn);
            OpenChildForm<FormQuanLyNhanVien>();
        }
        private void btnBaoCaoDoanhThu_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (_currentMenuButton == btn) return;

            SetActiveMenuButton(btn);
            OpenChildForm<FormBaoCaoDoanhThu>();
        }
        private void btnQuanLyBan_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (_currentMenuButton == btn) return;

            SetActiveMenuButton(btn);
            OpenChildForm<FormQuanLyBan>();
        }

        private void btnQuanLyMon_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (_currentMenuButton == btn) return;

            SetActiveMenuButton(btn);
            OpenChildForm<FormQuanLyMon>();
        }
        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (_currentMenuButton == btn) return;

            SetActiveMenuButton(btn);
            OpenChildForm<FormTraCuuHoaDonAdmin>();
        }

        private void btnQuanLyDanhMuc_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (_currentMenuButton == btn) return;

            SetActiveMenuButton(btn);
            OpenChildForm<FormQuanLyDanhMuc>();
        }

        private void btnAuditLog_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (_currentMenuButton == btn) return;

            SetActiveMenuButton(btn);
            OpenChildForm<FormNhatKyHeThong>();
        }

        // ====== TOP BAR ======

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            using (var f = new FormDoiMatKhau())
            {
                f.ShowDialog();
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SessionHienTai.Clear();
                Close();
                Application.Restart(); // quay lại form chọn vai trò / login ban đầu
            }
        }
    }
}
