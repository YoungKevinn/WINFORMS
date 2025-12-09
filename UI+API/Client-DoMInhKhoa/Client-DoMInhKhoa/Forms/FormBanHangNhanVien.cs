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
        private Panel _panelHeader;
        private Panel _panelBan;
        private Panel _panelMenu;
        private Panel _panelDon;

        private Label lblXinChao;
        private Button btnDangXuat;

        private FlowLayoutPanel flpBan;

        private ComboBox cboDanhMuc;
        private ComboBox cboMon;
        private NumericUpDown nudSoLuong;
        private FlowLayoutPanel flpMonNhanh;
        private Panel _panelMenuTop;
        private TextBox? _txtTimMon;

        private Label lblBanHienTai;
        private FlowLayoutPanel flpChiTiet;
        private Label lblTongTien;
        private Button btnThemMon;
        private Button btnXoaMon;
        private Button btnThanhToan;
        private ComboBox cboBanFrom;
        private ComboBox cboBanTo;
        private Button btnChuyenBan;
        private Button btnGopBan;

        private class MonViewModel
        {
            public int Id { get; set; }
            public string Ten { get; set; } = string.Empty;
            public decimal DonGia { get; set; }
            public string Loai { get; set; } = string.Empty;
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
            btnXoaMon.Click += btnXoaMon_Click;
            btnThemMon.Click += btnThemMon_Click;
            btnChuyenBan.Click += btnChuyenBan_Click;
            btnGopBan.Click += btnGopBan_Click;

            cboMon.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private async void FormBanHangNhanVien_Load(object? sender, EventArgs e)
        {
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
