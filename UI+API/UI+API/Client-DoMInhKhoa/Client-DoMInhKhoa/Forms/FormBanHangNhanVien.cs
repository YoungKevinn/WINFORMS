using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien : Form
    {
        private const int STATUS_TRONG = 0;
        private const int STATUS_DANG_DUNG = 1;

        private readonly DanhMucService _danhMucService = new();
        private readonly ThucAnService _thucAnService = new();
        private readonly ThucUongService _thucUongService = new();
        private readonly DonGoiService _donGoiService = new();
        private readonly ChiTietDonGoiService _ctDonGoiService = new();

        private readonly NhanVienDto _nhanVienDangNhap;

        private List<BanDto> _dsBan = new();
        private List<DanhMucDto> _dsDanhMuc = new();
        private List<ThucAnDto> _dsThucAn = new();
        private List<ThucUongDto> _dsThucUong = new();

        private BanDto? _banDangChon;
        private DonGoiDto? _donGoiHienTai;
        private int? _chiTietDangChonId;

        // ==== Controls ====
        private Panel _panelHeader = null!;
        private Panel _panelBan = null!;
        private Panel _panelMenu = null!;
        private Panel _panelDon = null!;

        private Label lblXinChao = null!;
        private Button btnDangXuat = null!;

        private FlowLayoutPanel flpBan = null!;

        private ComboBox cboDanhMuc = null!;
        private ComboBox cboMon = null!;
        private NumericUpDown nudSoLuong = null!;
        private FlowLayoutPanel flpMonNhanh = null!;
        private Panel _panelMenuTop = null!;
        private TextBox? _txtTimMon;

        private Label lblBanHienTai = null!;
        private FlowLayoutPanel flpChiTiet = null!;
        private Label lblTongTien = null!;
        private Button btnThemMon = null!;
        private Button btnXoaMon = null!;

        // ✅ NÚT IN TẠM
        private Button btnInTam = null!;

        // ✅ PHẢI CÓ CẢ 2 NÚT NÀY (BẠN ĐANG THIẾU btnThanhToanTungPhan)
        private Button btnThanhToan = null!;
        private Button btnThanhToanTungPhan = null!;

        private ComboBox cboBanFrom = null!;
        private ComboBox cboBanTo = null!;
        private Button btnChuyenBan = null!;
        private Button btnGopBan = null!;

        // ✅ ViewModel dùng chung MenuMon.cs / ChiTietDon.cs
        private class MonViewModel
        {
            public int Id { get; set; }
            public string Ten { get; set; } = string.Empty;
            public decimal DonGia { get; set; }
            public string Loai { get; set; } = string.Empty; // "ThucAn" | "ThucUong"
            public override string ToString() => Ten;
        }

        public FormBanHangNhanVien(NhanVienDto nhanVien)
        {
            _nhanVienDangNhap = nhanVien;

            BuildLayout();
            BuildMenuSearch();

            Load += FormBanHangNhanVien_Load;

            btnDangXuat.Click += btnDangXuat_Click;

            btnThanhToan.Click += btnThanhToan_Click;
            btnThanhToanTungPhan.Click += btnThanhToanTungPhan_Click;

            btnXoaMon.Click += btnXoaMon_Click;
            btnThemMon.Click += btnThemMon_Click;

            btnChuyenBan.Click += btnChuyenBan_Click;
            btnGopBan.Click += btnGopBan_Click;

            // ✅ HOOK NÚT IN TẠM
            btnInTam.Click += btnInTam_Click;

            cboMon.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private async void FormBanHangNhanVien_Load(object? sender, EventArgs e)
        {
            // lblXinChao đang ẩn nên dòng này không ảnh hưởng, giữ cho logic
            lblXinChao.Text = $"Xin chào, {_nhanVienDangNhap.HoTen}";

            await LoadDanhMucVaMonAsync();
            await LoadDanhSachBanAsync();
        }

        private void btnDangXuat_Click(object? sender, EventArgs e)
        {
            Hide();
            using (var f = new FormNhanVienDangNhap())
            {
                f.ShowDialog();
            }
            Close();
        }
    }
}
