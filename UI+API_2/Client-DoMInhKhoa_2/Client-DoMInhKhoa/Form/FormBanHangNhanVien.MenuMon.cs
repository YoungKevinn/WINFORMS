using Client_DoMInhKhoa.Models;
using System;
using System.Windows.Forms;
using WinTimer = System.Windows.Forms.Timer;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien
    {
        // đặt tên khác để chắc chắn không đụng file khác
        private readonly WinTimer _menuSearchTimer__MINH = new WinTimer { Interval = 250 };
        private bool _menuSearchHooked__MINH = false;

        /// <summary>
        /// GỌI 1 LẦN trong constructor SAU InitializeComponent()
        /// </summary>
        private void InitMenuMonSearch()
        {
            if (_menuSearchHooked__MINH) return;
            _menuSearchHooked__MINH = true;

            // txtTimMon là textbox đã có sẵn trong FormBanHangNhanVien.Designer.cs
            if (txtTimMon == null) return;

            txtTimMon.TextChanged -= TxtTimMon_TextChanged__MINH;
            txtTimMon.TextChanged += TxtTimMon_TextChanged__MINH;

            _menuSearchTimer__MINH.Tick -= MenuSearchTimer_Tick__MINH;
            _menuSearchTimer__MINH.Tick += MenuSearchTimer_Tick__MINH;

            if (cboDanhMuc != null)
            {
                cboDanhMuc.SelectedIndexChanged -= CboDanhMuc_SelectedIndexChanged_MenuSearch__MINH;
                cboDanhMuc.SelectedIndexChanged += CboDanhMuc_SelectedIndexChanged_MenuSearch__MINH;
            }
        }

        private void TxtTimMon_TextChanged__MINH(object? sender, EventArgs e)
        {
            _menuSearchTimer__MINH.Stop();
            _menuSearchTimer__MINH.Start();
        }

        private void MenuSearchTimer_Tick__MINH(object? sender, EventArgs e)
        {
            _menuSearchTimer__MINH.Stop();

            string keyword = (txtTimMon?.Text ?? "").Trim();

            // Lọc món theo danh mục + keyword
            if (cboDanhMuc?.SelectedItem is DanhMucDto dm)
                LoadMonTheoDanhMuc(dm, keyword);

            // Render nút món nhanh theo keyword
            RenderMonButtons();
        }

        private void CboDanhMuc_SelectedIndexChanged_MenuSearch__MINH(object? sender, EventArgs e)
        {
            string keyword = (txtTimMon?.Text ?? "").Trim();

            if (cboDanhMuc?.SelectedItem is DanhMucDto dm)
            {
                LoadMonTheoDanhMuc(dm, keyword);
                RenderMonButtons(dm);
            }
        }
    }
}
