using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien : Form
    {
        // ====== Services dùng static ApiClient ======
        private readonly BanService _banService = new();
        private readonly DanhMucService _danhMucService = new();
        private readonly ThucAnService _thucAnService = new();
        private readonly ThucUongService _thucUongService = new();
        private readonly DonGoiService _donGoiService = new();
        private readonly ChiTietDonGoiService _ctDonGoiService = new();

        private readonly NhanVienDto _nhanVienDangNhap;

        // Cache dữ liệu
        private List<BanDto> _dsBan = new();
        private List<DanhMucDto> _dsDanhMuc = new();
        private List<ThucAnDto> _dsThucAn = new();
        private List<ThucUongDto> _dsThucUong = new();

        private BanDto? _banDangChon;
        private DonGoiDto? _donGoiHienTai;

        // Dòng hiển thị trên DataGridView
        private class ChiTietRow
        {
            public int Id { get; set; }
            public string TenMon { get; set; } = string.Empty;
            public decimal DonGia { get; set; }
            public int SoLuong { get; set; }
            public decimal ThanhTien { get; set; }
        }

        public FormBanHangNhanVien(NhanVienDto nhanVien)
        {
            InitializeComponent();
            _nhanVienDangNhap = nhanVien;

            // Gắn event
            this.Load += FormBanHangNhanVien_Load;
            btnThemMon.Click += btnThemMon_Click;
            btnXoaMon.Click += btnXoaMon_Click;
            btnThanhToan.Click += btnThanhToan_Click;
            btnDangXuat.Click += btnDangXuat_Click;
            // (btnChuyenBan, btnGopBan hiện để trống, sau này cần sẽ xử lý thêm)

            InitGrid();
        }

        // ====== Cấu hình DataGridView ======
        private void InitGrid()
        {
            dgvChiTiet.AutoGenerateColumns = false;
            dgvChiTiet.Columns.Clear();

            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                DataPropertyName = "Id",
                Visible = false
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTenMon",
                HeaderText = "Tên món",
                DataPropertyName = "TenMon",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDonGia",
                HeaderText = "Đơn giá",
                DataPropertyName = "DonGia"
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSoLuong",
                HeaderText = "SL",
                DataPropertyName = "SoLuong"
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colThanhTien",
                HeaderText = "Thành tiền",
                DataPropertyName = "ThanhTien"
            });
            // ====== Format tiền Việt Nam ======
            var vi = new CultureInfo("vi-VN");

            dgvChiTiet.Columns["colDonGia"].DefaultCellStyle.FormatProvider = vi;
            dgvChiTiet.Columns["colDonGia"].DefaultCellStyle.Format = "N0";        // 25.000
            dgvChiTiet.Columns["colDonGia"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;

            dgvChiTiet.Columns["colThanhTien"].DefaultCellStyle.FormatProvider = vi;
            dgvChiTiet.Columns["colThanhTien"].DefaultCellStyle.Format = "N0";     // 245.000
            dgvChiTiet.Columns["colThanhTien"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;
        }

        // ====== Load form ======
        private async void FormBanHangNhanVien_Load(object? sender, EventArgs e)
        {
            try
            {
                lblXinChao.Text = $"Xin chào, {_nhanVienDangNhap.HoTen}";
            }
            catch
            {
                // nếu DTO không có HoTen thì kệ, dùng text mặc định
            }

            await LoadDanhMucVaMonAsync();
            await LoadDanhSachBanAsync();
        }

        // ====== Load danh mục + món ======
        private async Task LoadDanhMucVaMonAsync()
        {
            _dsDanhMuc = await _danhMucService.GetAllAsync();
            _dsThucAn = await _thucAnService.GetAllAsync();
            _dsThucUong = await _thucUongService.GetAllAsync();

            var dsDmActive = _dsDanhMuc
                .Where(x => x.DangHoatDong)
                .OrderBy(x => x.Ten)
                .ToList();

            cboDanhMuc.DataSource = dsDmActive;
            cboDanhMuc.DisplayMember = "Ten";
            cboDanhMuc.ValueMember = "Id";

            cboDanhMuc.SelectedIndexChanged -= cboDanhMuc_SelectedIndexChanged;
            cboDanhMuc.SelectedIndexChanged += cboDanhMuc_SelectedIndexChanged;

            if (dsDmActive.Any())
                cboDanhMuc.SelectedIndex = 0;
        }

        private void cboDanhMuc_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboDanhMuc.SelectedItem is not DanhMucDto dm) return;

            var dsMon = new List<MonViewModel>();

            dsMon.AddRange(_dsThucAn
                .Where(m => m.DanhMucId == dm.Id && m.DangHoatDong)
                .Select(m => new MonViewModel
                {
                    Id = m.Id,
                    Ten = m.Ten,
                    DonGia = m.DonGia,
                    Loai = "ThucAn"
                }));

            dsMon.AddRange(_dsThucUong
                .Where(m => m.DanhMucId == dm.Id && m.DangHoatDong)
                .Select(m => new MonViewModel
                {
                    Id = m.Id,
                    Ten = m.Ten,
                    DonGia = m.DonGia,
                    Loai = "ThucUong"
                }));

            cboMon.DataSource = dsMon;
            cboMon.DisplayMember = "Ten";
            cboMon.ValueMember = "Id";
        }

        // ====== Load danh sách bàn bên trái ======
        private async Task LoadDanhSachBanAsync()
        {
            _dsBan = await _banService.GetAllAsync();

            flpBan.Controls.Clear();

            foreach (var ban in _dsBan)
            {
                var btn = new Button
                {
                    Width = 90,
                    Height = 70,
                    Tag = ban,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Margin = new Padding(8),
                    FlatStyle = FlatStyle.Flat
                };

                bool banTrong = !ban.TrangThai;

                btn.Text = banTrong ? $"{ban.TenBan}\nTrống" : $"{ban.TenBan}\nCó khách";
                btn.BackColor = banTrong ? System.Drawing.Color.WhiteSmoke : System.Drawing.Color.LightGreen;
                btn.Click += BanButton_Click;
                flpBan.Controls.Add(btn);
            }

            // Combobox dưới
            cboBanFrom.DataSource = _dsBan.ToList();
            cboBanFrom.DisplayMember = "TenBan";
            cboBanFrom.ValueMember = "Id";

            cboBanTo.DataSource = _dsBan.ToList();
            cboBanTo.DisplayMember = "TenBan";
            cboBanTo.ValueMember = "Id";
        }

        private async void BanButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not BanDto ban) return;

            _banDangChon = ban;
            cboBanFrom.SelectedValue = ban.Id;

            await LoadDonGoiChoBanAsync(ban.Id);
        }

        // ====== Load đơn gọi + chi tiết cho bàn ======
        private async Task LoadDonGoiChoBanAsync(int banId)
        {
            var tatCaDonGoi = await _donGoiService.GetAllAsync();

            // Lấy đơn mới nhất của bàn (kể cả đã thanh toán)
            var donMoiNhat = tatCaDonGoi
                .Where(x => x.BanId == banId)
                .OrderByDescending(x => x.MoLuc)
                .FirstOrDefault();

            _donGoiHienTai = donMoiNhat;   // dùng cho thêm món (sẽ kiểm tra trạng thái ở dưới)

            string tenBan = _banDangChon?.TenBan
                            ?? _dsBan.FirstOrDefault(b => b.Id == banId)?.TenBan
                            ?? $"Bàn {banId}";

            if (donMoiNhat == null)
            {
                dgvChiTiet.DataSource = null;
                lblTongTien.Text = "0 đ";
                lblBanHienTai.Text = $"Bàn hiện tại: {tenBan} (chưa có đơn gọi)";
                return;
            }

            string trangThaiText = donMoiNhat.TrangThai == 1 ? "đã thanh toán" : "chưa thanh toán";
            lblBanHienTai.Text = $"Bàn hiện tại: {tenBan} ({trangThaiText})";

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(donMoiNhat.Id);
            BindChiTietToGrid(dsChiTiet);
        }


        private void BindChiTietToGrid(List<ChiTietDonGoiDto> dsChiTiet)
        {
            var data = new List<ChiTietRow>();
            decimal tong = 0;

            foreach (var ct in dsChiTiet)
            {
                string tenMon = "";
                if (ct.ThucAnId.HasValue)
                {
                    tenMon = _dsThucAn.FirstOrDefault(x => x.Id == ct.ThucAnId.Value)?.Ten ?? "Món ăn";
                }
                else if (ct.ThucUongId.HasValue)
                {
                    tenMon = _dsThucUong.FirstOrDefault(x => x.Id == ct.ThucUongId.Value)?.Ten ?? "Thức uống";
                }

                var thanhTien = ct.DonGia * ct.SoLuong * (1 - ct.ChietKhau / 100m);
                tong += thanhTien;

                data.Add(new ChiTietRow
                {
                    Id = ct.Id,
                    TenMon = tenMon,
                    DonGia = ct.DonGia,
                    SoLuong = ct.SoLuong,
                    ThanhTien = thanhTien
                });
            }

            dgvChiTiet.DataSource = data;
            var vi = new CultureInfo("vi-VN");
            lblTongTien.Text = string.Format(vi, "{0:N0} đ", tong);
        }

        // ====== Thêm món ======
        private async void btnThemMon_Click(object? sender, EventArgs e)
        {
            if (_banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trong danh sách bên trái.", "Thông báo");
                return;
            }

            if (cboMon.SelectedItem is not MonViewModel monVm)
            {
                MessageBox.Show("Vui lòng chọn món.", "Thông báo");
                return;
            }

            int soLuong = (int)nudSoLuong.Value;
            if (soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải > 0.", "Thông báo");
                return;
            }

            // Nếu chưa có đơn gọi -> tạo mới
            if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
            {
                var reqDonGoi = new
                {
                    NhanVienId = _nhanVienDangNhap.Id,
                    BanId = _banDangChon.Id,
                    MoLuc = DateTime.Now,
                    TrangThai = 0,          // 0 = chưa thanh toán
                    GhiChu = (string?)null
                };
                _donGoiHienTai = await _donGoiService.CreateAsync(reqDonGoi);

                await LoadDanhSachBanAsync(); // cập nhật màu bàn
        }

            var reqChiTiet = new
            {
                DonGoiId = _donGoiHienTai.Id,
                ThucAnId = monVm.Loai == "ThucAn" ? monVm.Id : (int?)null,
                ThucUongId = monVm.Loai == "ThucUong" ? monVm.Id : (int?)null,
                SoLuong = soLuong,
                DonGia = monVm.DonGia,
                ChietKhau = 0m,
                GhiChu = (string?)null
            };

            await _ctDonGoiService.CreateAsync(reqChiTiet);
            await LoadDonGoiChoBanAsync(_banDangChon!.Id);
        }

        // ====== Xóa món ======
        private async void btnXoaMon_Click(object? sender, EventArgs e)
        {
            if (dgvChiTiet.CurrentRow == null)
                return;

            var cellValue = dgvChiTiet.CurrentRow.Cells["colId"].Value;
            if (cellValue == null) return;

            int ctId = Convert.ToInt32(cellValue);

            if (MessageBox.Show("Xóa món đang chọn?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            await _ctDonGoiService.DeleteAsync(ctId);
            await LoadDonGoiChoBanAsync(_banDangChon!.Id);
        }

        // ====== Thanh toán (skeleton – để đơn giản, chưa gọi ThanhToan/HoaDon) ======
        private async void btnThanhToan_Click(object? sender, EventArgs e)
        {
            if (_donGoiHienTai == null)
            {
                MessageBox.Show("Bàn này chưa có đơn gọi.", "Thông báo");
                return;
            }

            if (_donGoiHienTai.TrangThai != 0)
            {
                MessageBox.Show("Đơn gọi đã thanh toán rồi.", "Thông báo");
                return;
            }

            // Tính tổng tiền từ grid
            decimal tongTien = 0;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.Cells["colThanhTien"].Value == null) continue;
                if (decimal.TryParse(row.Cells["colThanhTien"].Value.ToString(), out var tt))
                    tongTien += tt;
            }

            // TODO: nối thêm API ThanhToan + HoaDon nếu muốn
            MessageBox.Show($"Thanh toán {tongTien:N0} đ (bạn nối thêm API ThanhToan/HoaDon).");

            // Sau thanh toán: reset đơn gọi hiện tại, reload bàn
            _donGoiHienTai = null;
            await LoadDanhSachBanAsync();
            await LoadDonGoiChoBanAsync(_banDangChon!.Id);
        }

        // ====== Đăng xuất ======
        private void btnDangXuat_Click(object? sender, EventArgs e)
        {
            this.Hide();
            using (var f = new FormNhanVienDangNhap())
            {
                f.ShowDialog();
            }
            this.Close();
        }
    }
}
