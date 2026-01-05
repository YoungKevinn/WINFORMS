using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using Client_DoMInhKhoa.Session;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// Alias để khỏi ambiguous với System.Threading.Timer
using WinTimer = System.Windows.Forms.Timer;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien : Form
    {
        // ====== CONST / FORMAT ======
        private const int STATUS_TRONG = 0;
        private const int STATUS_DANG_DUNG = 1;

        private const int BAN_BTN_W = 120;
        private const int BAN_BTN_H = 90;

        private readonly CultureInfo _vi = new CultureInfo("vi-VN");

        // ✅ NOTE BOX: đẩy "Ghi chú" sang phải
        private const int NOTEBOX_X = 82;   // (trước là 70)
        private const int NOTEBOX_RIGHT_GAP = 18;
        private const int NOTEBOX_MIN_W = 140;

        // ====== SERVICES ======
        private readonly DanhMucService _danhMucService = new();
        private readonly ThucAnService _thucAnService = new();
        private readonly ThucUongService _thucUongService = new();
        private readonly DonGoiService _donGoiService = new();
        private readonly ChiTietDonGoiService _ctDonGoiService = new();

        // ====== STATE ======
        private List<BanDto> _dsBan = new();
        private BanDto? _banDangChon;
        private DonGoiDto? _donGoiHienTai;
        private int? _chiTietDangChonId;

        private List<DanhMucDto> _dsDanhMuc = new();
        private List<ThucAnDto> _dsThucAn = new();
        private List<ThucUongDto> _dsThucUong = new();

        private readonly NhanVienDto _nhanVienDangNhap;

        // ====== SEARCH (DEBOUNCE) ======
        private readonly WinTimer _searchTimer = new WinTimer { Interval = 250 };
        private bool _hookedSearchTimer = false;

        private bool _syncingCombo = false;
        private bool _hookResizeChiTiet = false;

        // ====== VM cho combobox món ======
        private class MonViewModel
        {
            public int Id { get; set; }
            public string Ten { get; set; } = "";
            public decimal DonGia { get; set; }
            public string Loai { get; set; } = ""; // "ThucAn" | "ThucUong"
            public override string ToString() => Ten;
        }

        // ====== CTOR ======
        public FormBanHangNhanVien()
            : this(new NhanVienDto { Id = 1, HoTen = "NV" })
        { }

        public FormBanHangNhanVien(NhanVienDto nhanVienDangNhap)
        {
            _nhanVienDangNhap = nhanVienDangNhap;

            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            Load += FormBanHangNhanVien_Load;

            btnThemMon.Click += btnThemMon_Click;
            btnXoaMon.Click += btnXoaMon_Click;

            btnChuyenBan.Click += btnChuyenBan_Click;
            btnGopBan.Click += btnGopBan_Click;

            cboBanFrom.SelectedIndexChanged += cboBanFrom_SelectedIndexChanged;
            cboDanhMuc.SelectedIndexChanged += cboDanhMuc_SelectedIndexChanged;
            txtTimMon.TextChanged += txtTimMon_TextChanged;

            btnDangXuat.Click += btnDangXuat_Click;

            btnInTam.Click += btnInTam_Click;

            btnThanhToan.Click += btnThanhToan_Click;
            btnThanhToanTungPhan.Click += btnThanhToanTungPhan_Click;

            HookSearchTimerOnce();
        }

        private async void FormBanHangNhanVien_Load(object? sender, EventArgs e)
        {
            lblXinChao.Text = $"Xin chào: {_nhanVienDangNhap.HoTen}";
            await LoadDanhMucVaMonAsync();
            await LoadDanhSachBanAsync();
        }

        // =========================
        //  ĐĂNG XUẤT
        // =========================
        private void btnDangXuat_Click(object? sender, EventArgs e)
        {
            var ok = MessageBox.Show("Bạn có chắc muốn đăng xuất?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ok != DialogResult.Yes) return;

            SessionHienTai.Clear();

            var fChon = Application.OpenForms.OfType<FormChonVaiTro>().FirstOrDefault();

            if (fChon == null || fChon.IsDisposed)
            {
                fChon = new FormChonVaiTro();
                fChon.Show();
            }
            else
            {
                fChon.Show();
                if (fChon.WindowState == FormWindowState.Minimized)
                    fChon.WindowState = FormWindowState.Normal;
                fChon.BringToFront();
                fChon.Activate();
            }

            Close();
        }

        // =========================
        //  LOAD DANH MỤC + MÓN
        // =========================
        private void HookSearchTimerOnce()
        {
            if (_hookedSearchTimer) return;
            _hookedSearchTimer = true;

            _searchTimer.Tick += (s, e) =>
            {
                _searchTimer.Stop();

                if (cboDanhMuc.SelectedItem is DanhMucDto dm)
                    LoadMonTheoDanhMuc(dm, txtTimMon.Text);

                RenderMonButtons();
            };
        }

        private void txtTimMon_TextChanged(object? sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private async Task LoadDanhMucVaMonAsync()
        {
            try
            {
                _dsDanhMuc = await _danhMucService.GetAllAsync();
                _dsThucAn = await _thucAnService.GetAllAsync();
                _dsThucUong = await _thucUongService.GetAllAsync();

                var dsDmActive = _dsDanhMuc
                    .Where(x => x.DangHoatDong)
                    .OrderBy(x => x.Ten)
                    .ToList();

                cboDanhMuc.SelectedIndexChanged -= cboDanhMuc_SelectedIndexChanged;

                cboDanhMuc.DataSource = dsDmActive;
                cboDanhMuc.DisplayMember = "Ten";
                cboDanhMuc.ValueMember = "Id";

                cboDanhMuc.SelectedIndexChanged += cboDanhMuc_SelectedIndexChanged;

                if (dsDmActive.Any())
                {
                    cboDanhMuc.SelectedIndex = 0;
                    var dm = (DanhMucDto)cboDanhMuc.SelectedItem;
                    LoadMonTheoDanhMuc(dm, txtTimMon.Text);
                }
                else
                {
                    cboMon.DataSource = null;
                    cboMon.Text = string.Empty;
                }

                RenderMonButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh mục / món: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboDanhMuc_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboDanhMuc.SelectedItem is DanhMucDto dm)
            {
                LoadMonTheoDanhMuc(dm, txtTimMon.Text);
                RenderMonButtons(dm);
            }
            else
            {
                cboMon.DataSource = null;
                cboMon.Text = string.Empty;
            }
        }

        private List<MonViewModel> BuildDsMonTheoDanhMuc(DanhMucDto? dm)
        {
            var list = new List<MonViewModel>();

            var thucAn = _dsThucAn.AsEnumerable();
            var thucUong = _dsThucUong.AsEnumerable();

            if (dm != null)
            {
                thucAn = thucAn.Where(m => m.DanhMucId == dm.Id && m.DangHoatDong);
                thucUong = thucUong.Where(m => m.DanhMucId == dm.Id && m.DangHoatDong);
            }
            else
            {
                thucAn = thucAn.Where(m => m.DangHoatDong);
                thucUong = thucUong.Where(m => m.DangHoatDong);
            }

            list.AddRange(thucAn.Select(m => new MonViewModel
            {
                Id = m.Id,
                Ten = m.Ten,
                DonGia = m.DonGia,
                Loai = "ThucAn"
            }));

            list.AddRange(thucUong.Select(m => new MonViewModel
            {
                Id = m.Id,
                Ten = m.Ten,
                DonGia = m.DonGia,
                Loai = "ThucUong"
            }));

            return list.OrderBy(x => x.Ten).ToList();
        }

        private void LoadMonTheoDanhMuc(DanhMucDto dm, string? keyword = null)
        {
            var dsMon = BuildDsMonTheoDanhMuc(dm);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var key = keyword.Trim().ToLowerInvariant();
                dsMon = dsMon.Where(m => m.Ten.ToLowerInvariant().Contains(key)).ToList();
            }

            if (dsMon.Count == 0)
            {
                cboMon.DataSource = null;
                cboMon.Text = string.Empty;
                return;
            }

            cboMon.DataSource = dsMon;
            cboMon.DisplayMember = "Ten";
            cboMon.ValueMember = "Id";
            cboMon.SelectedIndex = 0;
        }

        private void RenderMonButtons(DanhMucDto? dm = null)
        {
            if (flpMonNhanh == null) return;

            flpMonNhanh.SuspendLayout();
            flpMonNhanh.Controls.Clear();

            var dsMon = BuildDsMonTheoDanhMuc(dm);

            string keyword = (txtTimMon.Text ?? "").Trim().ToLowerInvariant();
            if (!string.IsNullOrEmpty(keyword))
                dsMon = dsMon.Where(m => m.Ten.ToLowerInvariant().Contains(keyword)).ToList();

            foreach (var mon in dsMon)
            {
                var btn = new Button
                {
                    Width = 120,
                    Height = 70,
                    Margin = new Padding(6),
                    Tag = mon,
                    Text = $"{mon.Ten}\r\n{mon.DonGia:N0} đ",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    BackColor = Color.FromArgb(240, 248, 255),
                    FlatStyle = FlatStyle.Flat,
                    UseCompatibleTextRendering = true
                };
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.RoyalBlue;

                btn.Click += BtnMonNhanh_Click;
                flpMonNhanh.Controls.Add(btn);
            }

            flpMonNhanh.ResumeLayout();
        }

        private void RenderMonButtons()
        {
            DanhMucDto? dm = cboDanhMuc.SelectedItem as DanhMucDto;
            RenderMonButtons(dm);
        }

        private void BtnMonNhanh_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not MonViewModel monVm) return;

            int? dmId = null;

            if (string.Equals(monVm.Loai, "ThucAn", StringComparison.OrdinalIgnoreCase))
                dmId = _dsThucAn.FirstOrDefault(x => x.Id == monVm.Id)?.DanhMucId;
            else if (string.Equals(monVm.Loai, "ThucUong", StringComparison.OrdinalIgnoreCase))
                dmId = _dsThucUong.FirstOrDefault(x => x.Id == monVm.Id)?.DanhMucId;

            if (dmId.HasValue && cboDanhMuc.DataSource != null)
                cboDanhMuc.SelectedValue = dmId.Value;

            if (cboMon.DataSource is IEnumerable<MonViewModel> list)
            {
                var item = list.FirstOrDefault(x =>
                    x.Id == monVm.Id &&
                    string.Equals(x.Loai, monVm.Loai, StringComparison.OrdinalIgnoreCase));

                if (item != null)
                    cboMon.SelectedItem = item;
            }

            btnThemMon_Click(null, EventArgs.Empty);
        }

        // =========================================================
        // ✅ FIX CỨNG: sync trạng thái bàn theo đơn mở + auto đóng đơn rỗng
        // =========================================================
        private async Task SyncBanTrangThaiTheoDonMoAsync()
        {
            List<DonGoiDto> tatCaDon;
            try
            {
                tatCaDon = await _donGoiService.GetAllAsync();
            }
            catch
            {
                return;
            }

            var donMo = tatCaDon.Where(d => d.TrangThai == 0).ToList();

            var banDangMoThucSu = new HashSet<int>();

            foreach (var d in donMo)
            {
                List<ChiTietDonGoiDto>? dsct = null;
                try { dsct = await _ctDonGoiService.GetByDonGoiIdAsync(d.Id); } catch { }

                if (dsct == null || dsct.Count == 0)
                {
                    await CloseDonGoiRongAsync(d);
                }
                else
                {
                    banDangMoThucSu.Add(d.BanId);
                }
            }

            foreach (var ban in _dsBan)
            {
                int expected = banDangMoThucSu.Contains(ban.Id) ? STATUS_DANG_DUNG : STATUS_TRONG;

                if (ban.TrangThai != expected)
                {
                    ban.TrangThai = expected;
                    try
                    {
                        await ApiClient.PutAsync<string>($"api/Ban/{ban.Id}", ban, includeAuth: true);
                    }
                    catch { }
                }
            }

            if (_banDangChon != null)
            {
                var fresh = _dsBan.FirstOrDefault(x => x.Id == _banDangChon.Id);
                if (fresh != null) _banDangChon = fresh;
            }
        }

        private async Task CloseDonGoiRongAsync(DonGoiDto don)
        {
            var req = new
            {
                NhanVienId = don.NhanVienId,
                BanId = don.BanId,
                MoLuc = don.MoLuc,
                DongLuc = DateTime.Now,
                TrangThai = 2,
                GhiChu = don.GhiChu
            };

            try
            {
                await ApiClient.PutAsync<string>($"api/DonGoi/{don.Id}", req, includeAuth: true);
            }
            catch { }
        }

        private async Task UpdateBanTrangThaiAsync(int banId, int trangThai)
        {
            var ban = _dsBan.FirstOrDefault(b => b.Id == banId);
            if (ban != null) ban.TrangThai = trangThai;

            if (_banDangChon != null && _banDangChon.Id == banId)
                _banDangChon.TrangThai = trangThai;

            try
            {
                if (ban != null)
                    await ApiClient.PutAsync<string>($"api/Ban/{ban.Id}", ban, includeAuth: true);
                else if (_banDangChon != null && _banDangChon.Id == banId)
                    await ApiClient.PutAsync<string>($"api/Ban/{_banDangChon.Id}", _banDangChon, includeAuth: true);
            }
            catch { }

            foreach (var btn in flpBan.Controls.OfType<Button>())
            {
                if (btn.Tag is BanDto b && b.Id == banId)
                {
                    b.TrangThai = trangThai;
                    bool isSelected = _banDangChon != null && _banDangChon.Id == banId;
                    StyleButtonBan(btn, trangThai, isSelected);
                    break;
                }
            }
        }

        // =========================
        //  LOAD BÀN + ĐƠN
        // =========================
        private async Task LoadDanhSachBanAsync(int? selectBanId = null)
        {
            _dsBan = await ApiClient.GetAsync<List<BanDto>>("api/Ban", includeAuth: true) ?? new List<BanDto>();

            await SyncBanTrangThaiTheoDonMoAsync();

            flpBan.SuspendLayout();
            flpBan.Controls.Clear();

            foreach (var ban in _dsBan.OrderBy(b => b.Id))
            {
                var btn = new Button
                {
                    Width = BAN_BTN_W,
                    Height = BAN_BTN_H,
                    Tag = ban,
                    Margin = new Padding(8)
                };

                StyleButtonBan(btn, ban.TrangThai, false);
                btn.Click += BanButton_Click;

                flpBan.Controls.Add(btn);
            }

            flpBan.ResumeLayout();

            _syncingCombo = true;

            cboBanFrom.DataSource = _dsBan.ToList();
            cboBanFrom.DisplayMember = "TenBan";
            cboBanFrom.ValueMember = "Id";

            cboBanTo.DataSource = _dsBan.ToList();
            cboBanTo.DisplayMember = "TenBan";
            cboBanTo.ValueMember = "Id";

            _syncingCombo = false;

            if (selectBanId.HasValue)
                _banDangChon = _dsBan.FirstOrDefault(b => b.Id == selectBanId.Value);
            else if (_banDangChon != null)
                _banDangChon = _dsBan.FirstOrDefault(b => b.Id == _banDangChon.Id);

            if (_banDangChon == null && _dsBan.Count > 0)
                _banDangChon = _dsBan[0];

            if (_banDangChon != null)
            {
                _syncingCombo = true;
                cboBanFrom.SelectedValue = _banDangChon.Id;
                _syncingCombo = false;

                HighlightSelectedBan(_banDangChon);
                await LoadDonGoiChoBanAsync(_banDangChon.Id);
            }
            else
            {
                ClearBillUi(0);
            }
        }

        private async void BanButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not BanDto ban) return;

            _banDangChon = ban;

            _syncingCombo = true;
            cboBanFrom.SelectedValue = ban.Id;
            _syncingCombo = false;

            HighlightSelectedBan(_banDangChon);
            await LoadDonGoiChoBanAsync(ban.Id);
        }

        private async void cboBanFrom_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_syncingCombo) return;
            if (cboBanFrom.SelectedItem is not BanDto ban) return;

            _banDangChon = ban;
            HighlightSelectedBan(_banDangChon);
            await LoadDonGoiChoBanAsync(ban.Id);
        }

        private void StyleButtonBan(Button btn, int trangThai, bool isSelected)
        {
            if (btn.Tag is BanDto ban)
            {
                string trangThaiText = trangThai == STATUS_DANG_DUNG ? "ĐANG DÙNG" : "CÒN TRỐNG";
                btn.Text = $"{ban.TenBan}\r\n{trangThaiText}";
            }

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = isSelected ? 3 : 2;
            btn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.UseCompatibleTextRendering = true;
            btn.AutoSize = false;
            btn.Padding = new Padding(2);

            if (trangThai == STATUS_DANG_DUNG)
            {
                btn.BackColor = Color.FromArgb(0, 120, 215);
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 215);
            }
            else
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.RoyalBlue;
                btn.FlatAppearance.BorderColor = Color.RoyalBlue;
            }
        }

        private void HighlightSelectedBan(BanDto? selected)
        {
            foreach (var control in flpBan.Controls.OfType<Button>())
            {
                if (control.Tag is not BanDto ban) continue;
                bool isSelected = (selected != null && ban.Id == selected.Id);
                StyleButtonBan(control, ban.TrangThai, isSelected);
            }
        }

        private void ClearBillUi(int banId)
        {
            _donGoiHienTai = null;
            _chiTietDangChonId = null;

            flpChiTiet.Controls.Clear();
            lblTongTien.Text = "0 đ";

            string tenBan = _banDangChon?.TenBan
                            ?? _dsBan.FirstOrDefault(b => b.Id == banId)?.TenBan
                            ?? $"Bàn {banId}";

            lblBanHienTai.Text = $"Bàn hiện tại: {tenBan} (chưa có đơn gọi)";
        }

        private async Task LoadDonGoiChoBanAsync(int banId)
        {
            var tatCaDonGoi = await _donGoiService.GetAllAsync();

            var donDangMo = tatCaDonGoi
                .Where(x => x.BanId == banId && x.TrangThai == 0)
                .OrderByDescending(x => x.MoLuc)
                .FirstOrDefault();

            if (donDangMo == null)
            {
                await UpdateBanTrangThaiAsync(banId, STATUS_TRONG);
                ClearBillUi(banId);
                return;
            }

            await UpdateBanTrangThaiAsync(banId, STATUS_DANG_DUNG);

            _donGoiHienTai = donDangMo;
            _chiTietDangChonId = null;

            string tenBan = _banDangChon?.TenBan
                            ?? _dsBan.FirstOrDefault(b => b.Id == banId)?.TenBan
                            ?? $"Bàn {banId}";

            lblBanHienTai.Text = $"Bàn hiện tại: {tenBan} (chưa thanh toán)";

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(donDangMo.Id) ?? new List<ChiTietDonGoiDto>();

            if (dsChiTiet.Count == 0)
            {
                await CloseDonGoiRongAsync(donDangMo);
                await UpdateBanTrangThaiAsync(banId, STATUS_TRONG);
                ClearBillUi(banId);
                return;
            }

            RenderChiTietList(dsChiTiet);
        }

        // =========================
        //  RENDER CHI TIẾT
        // =========================
        private void HookFlpChiTietResizeOnce()
        {
            if (_hookResizeChiTiet) return;
            _hookResizeChiTiet = true;

            flpChiTiet.SizeChanged += (_, __) =>
            {
                int w = flpChiTiet.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 6;
                if (w < 200) w = 200;

                foreach (var p in flpChiTiet.Controls.OfType<Panel>())
                {
                    p.Width = w;

                    var right = p.Controls.OfType<Panel>().FirstOrDefault(x => x.Name == "pnlRight");
                    int rightW = right?.Width ?? 260;

                    var noteBox = p.Controls.OfType<Panel>().FirstOrDefault(x => x.Name == "pnlNoteBox");
                    if (noteBox != null)
                    {
                        // ✅ 70 -> NOTEBOX_X
                        int newW = p.ClientSize.Width - rightW - NOTEBOX_X - NOTEBOX_RIGHT_GAP;
                        if (newW < NOTEBOX_MIN_W) newW = NOTEBOX_MIN_W;
                        noteBox.Width = newW;
                    }
                }
            };
        }

        private void RenderChiTietList(List<ChiTietDonGoiDto> dsChiTiet)
        {
            HookFlpChiTietResizeOnce();

            flpChiTiet.SuspendLayout();
            flpChiTiet.Controls.Clear();

            decimal tong = 0m;
            int index = 1;

            foreach (var ct in dsChiTiet)
            {
                string tenMon = GetTenMon(ct);
                tong += ct.DonGia * ct.SoLuong;

                var item = CreateChiTietItemPanel(index, tenMon, ct);
                flpChiTiet.Controls.Add(item);

                index++;
            }

            flpChiTiet.ResumeLayout();

            lblTongTien.Text = string.Format(_vi, "{0:N0} đ", tong);
        }

        private string GetTenMon(ChiTietDonGoiDto ct)
        {
            if (ct.ThucAnId.HasValue)
                return _dsThucAn.FirstOrDefault(x => x.Id == ct.ThucAnId.Value)?.Ten ?? "Món ăn";

            if (ct.ThucUongId.HasValue)
                return _dsThucUong.FirstOrDefault(x => x.Id == ct.ThucUongId.Value)?.Ten ?? "Thức uống";

            return "Món";
        }

        // ✅ FIX: click chọn được mọi vùng (kể cả bên phải + right-click), ghi chú không thụt, save note không reload
        private Panel CreateChiTietItemPanel(int index, string tenMon, ChiTietDonGoiDto ct)
        {
            var panel = new Panel
            {
                Height = 86,
                Width = Math.Max(200, flpChiTiet.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 6),
                Margin = new Padding(0, 0, 0, 6),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Tag = ct
            };

            void SelectThis()
            {
                _chiTietDangChonId = ct.Id;
                foreach (Panel p in flpChiTiet.Controls.OfType<Panel>())
                    p.BackColor = Color.White;
                panel.BackColor = Color.FromArgb(230, 240, 255);
            }

            void AttachSelectMouseDown(Control c)
            {
                c.MouseDown += (_, e) =>
                {
                    if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                        SelectThis();
                };

                foreach (Control child in c.Controls)
                    AttachSelectMouseDown(child);
            }

            var lblTen = new Label
            {
                Text = $"{index}. {tenMon}",
                AutoSize = true,
                Location = new Point(8, 8),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblGhiChuCaption = new Label
            {
                Text = "Ghi chú:",
                AutoSize = true,
                Location = new Point(18, 42),
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            // note box có viền + padding
            var pnlNoteBox = new Panel
            {
                Name = "pnlNoteBox",
                Location = new Point(NOTEBOX_X, 38), // ✅ 70 -> NOTEBOX_X (đẩy sang phải)
                Height = 24,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Padding = new Padding(4, 3, 4, 3)
            };

            var txtGhiChu = new TextBox
            {
                Name = "txtGhiChu",
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Text = ct.GhiChu ?? "",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.Black
            };

            pnlNoteBox.Controls.Add(txtGhiChu);

            var panelRight = new Panel
            {
                Name = "pnlRight",
                Dock = DockStyle.Right,
                Width = 260
            };

            var lblDonGia = new Label
            {
                Text = ct.DonGia.ToString("N0", _vi),
                AutoSize = true,
                Location = new Point(5, 10),
                Font = new Font("Segoe UI", 9)
            };

            var panelQty = new Panel
            {
                Location = new Point(70, 6),
                Size = new Size(110, 30),
                BorderStyle = BorderStyle.FixedSingle
            };

            var btnMinus = new Button
            {
                Text = "-",
                Width = 30,
                Height = 26,
                Location = new Point(0, 1),
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            btnMinus.FlatAppearance.BorderSize = 0;

            var lblQty = new Label
            {
                Text = ct.SoLuong.ToString(),
                Width = 40,
                Height = 26,
                Location = new Point(35, 2),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnPlus = new Button
            {
                Text = "+",
                Width = 30,
                Height = 26,
                Location = new Point(74, 1),
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            btnPlus.FlatAppearance.BorderSize = 0;

            var lblThanhTien = new Label
            {
                Text = (ct.DonGia * ct.SoLuong).ToString("N0", _vi),
                AutoSize = true,
                Location = new Point(190, 10),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            panelQty.Controls.Add(btnMinus);
            panelQty.Controls.Add(lblQty);
            panelQty.Controls.Add(btnPlus);

            panelRight.Controls.Add(lblDonGia);
            panelRight.Controls.Add(panelQty);
            panelRight.Controls.Add(lblThanhTien);

            panel.Controls.Add(panelRight);
            panel.Controls.Add(lblTen);
            panel.Controls.Add(lblGhiChuCaption);
            panel.Controls.Add(pnlNoteBox);

            // ✅ 70 -> NOTEBOX_X
            int noteW = panel.ClientSize.Width - panelRight.Width - NOTEBOX_X - NOTEBOX_RIGHT_GAP;
            if (noteW < NOTEBOX_MIN_W) noteW = NOTEBOX_MIN_W;
            pnlNoteBox.Width = noteW;

            async Task SaveNoteAsync()
            {
                if (_banDangChon == null) return;

                var note = (txtGhiChu.Text ?? "").Trim();
                var req = new
                {
                    DonGoiId = ct.DonGoiId,
                    ThucAnId = ct.ThucAnId,
                    ThucUongId = ct.ThucUongId,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    ChietKhau = ct.ChietKhau,
                    GhiChu = string.IsNullOrWhiteSpace(note) ? null : note
                };

                try
                {
                    await _ctDonGoiService.UpdateAsync(ct.Id, req);
                    ct.GhiChu = string.IsNullOrWhiteSpace(note) ? null : note; // update local, KHÔNG reload
                }
                catch { }
            }

            txtGhiChu.KeyDown += async (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    await SaveNoteAsync();
                }
            };
            txtGhiChu.Leave += async (_, __) => await SaveNoteAsync();

            btnPlus.Click += async (_, __) =>
            {
                if (_banDangChon == null) return;

                var req = new
                {
                    DonGoiId = ct.DonGoiId,
                    ThucAnId = ct.ThucAnId,
                    ThucUongId = ct.ThucUongId,
                    SoLuong = ct.SoLuong + 1,
                    DonGia = ct.DonGia,
                    ChietKhau = ct.ChietKhau,
                    GhiChu = ct.GhiChu
                };

                await _ctDonGoiService.UpdateAsync(ct.Id, req);
                await LoadDonGoiChoBanAsync(_banDangChon.Id);
            };

            btnMinus.Click += async (_, __) =>
            {
                if (_banDangChon == null) return;

                if (ct.SoLuong <= 1)
                {
                    await _ctDonGoiService.DeleteAsync(ct.Id);
                    await LoadDonGoiChoBanAsync(_banDangChon.Id);
                    return;
                }

                var req = new
                {
                    DonGoiId = ct.DonGoiId,
                    ThucAnId = ct.ThucAnId,
                    ThucUongId = ct.ThucUongId,
                    SoLuong = ct.SoLuong - 1,
                    DonGia = ct.DonGia,
                    ChietKhau = ct.ChietKhau,
                    GhiChu = ct.GhiChu
                };

                await _ctDonGoiService.UpdateAsync(ct.Id, req);
                await LoadDonGoiChoBanAsync(_banDangChon.Id);
            };

            // gắn chọn cho toàn bộ (kể cả click bên phải / right click)
            AttachSelectMouseDown(panel);

            return panel;
        }

        // =========================
        //  THÊM / XÓA MÓN
        // =========================
        private async void btnThemMon_Click(object? sender, EventArgs e)
        {
            if (_banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboMon.SelectedItem is not MonViewModel monVm)
            {
                MessageBox.Show("Vui lòng chọn món.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
                {
                    var reqDon = new
                    {
                        NhanVienId = _nhanVienDangNhap.Id,
                        BanId = _banDangChon.Id,
                        MoLuc = DateTime.Now,
                        TrangThai = 0,
                        GhiChu = (string?)null
                    };

                    var created = await _donGoiService.CreateAsync(reqDon);
                    if (created == null || created.Id <= 0)
                    {
                        MessageBox.Show("Không tạo được đơn gọi (API trả về null/Id<=0).", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _donGoiHienTai = created;

                    await UpdateBanTrangThaiAsync(_banDangChon.Id, STATUS_DANG_DUNG);
                    HighlightSelectedBan(_banDangChon);
                }

                if (_donGoiHienTai == null || _donGoiHienTai.Id <= 0 || _donGoiHienTai.TrangThai != 0)
                {
                    MessageBox.Show("Đơn gọi hiện tại không hợp lệ.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int? thucAnId = monVm.Loai == "ThucAn" ? monVm.Id : (int?)null;
                int? thucUongId = monVm.Loai == "ThucUong" ? monVm.Id : (int?)null;

                var ds = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id)
                         ?? new List<ChiTietDonGoiDto>();

                var existing = ds.FirstOrDefault(x => x.ThucAnId == thucAnId && x.ThucUongId == thucUongId);

                if (existing != null)
                {
                    var reqUp = new
                    {
                        DonGoiId = existing.DonGoiId,
                        ThucAnId = existing.ThucAnId,
                        ThucUongId = existing.ThucUongId,
                        SoLuong = existing.SoLuong + 1,
                        DonGia = existing.DonGia,
                        ChietKhau = existing.ChietKhau,
                        GhiChu = existing.GhiChu
                    };

                    await _ctDonGoiService.UpdateAsync(existing.Id, reqUp);
                }
                else
                {
                    var reqCreate = new
                    {
                        DonGoiId = _donGoiHienTai.Id,
                        ThucAnId = thucAnId,
                        ThucUongId = thucUongId,
                        SoLuong = 1,
                        DonGia = monVm.DonGia,
                        ChietKhau = 0m,
                        GhiChu = (string?)null
                    };

                    await _ctDonGoiService.CreateAsync(reqCreate);
                }

                await LoadDonGoiChoBanAsync(_banDangChon.Id);
                await LoadDanhSachBanAsync(_banDangChon.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm món: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnXoaMon_Click(object? sender, EventArgs e)
        {
            if (_banDangChon == null || _donGoiHienTai == null) return;

            if (!_chiTietDangChonId.HasValue)
            {
                MessageBox.Show("Bạn hãy click chọn món trong danh sách rồi mới xóa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                await _ctDonGoiService.DeleteAsync(_chiTietDangChonId.Value);
                _chiTietDangChonId = null;

                await LoadDonGoiChoBanAsync(_banDangChon.Id);
                await LoadDanhSachBanAsync(_banDangChon.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa món: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================
        //  CHUYỂN BÀN
        // =========================
        private async void btnChuyenBan_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_donGoiHienTai == null || _banDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn bàn đang có đơn để chuyển.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (_donGoiHienTai.TrangThai != 0)
                {
                    MessageBox.Show("Đơn gọi không còn ở trạng thái đang mở.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int fromId = _banDangChon.Id;

                if (cboBanTo.SelectedItem is not BanDto banTo)
                {
                    MessageBox.Show("Vui lòng chọn bàn đích.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int toId = banTo.Id;

                if (toId == fromId)
                {
                    MessageBox.Show("Bàn đích phải khác bàn hiện tại.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (banTo.TrangThai != STATUS_TRONG)
                {
                    MessageBox.Show("Bàn đích đang được sử dụng, không thể chuyển.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var reqUpdateDon = new
                {
                    NhanVienId = _donGoiHienTai.NhanVienId,
                    BanId = toId,
                    MoLuc = _donGoiHienTai.MoLuc,
                    DongLuc = _donGoiHienTai.DongLuc,
                    TrangThai = _donGoiHienTai.TrangThai,
                    GhiChu = _donGoiHienTai.GhiChu
                };

                await ApiClient.PutAsync<string>(
                    $"api/DonGoi/{_donGoiHienTai.Id}",
                    reqUpdateDon,
                    includeAuth: true
                );

                await UpdateBanTrangThaiAsync(fromId, STATUS_TRONG);
                await UpdateBanTrangThaiAsync(toId, STATUS_DANG_DUNG);

                await LoadDanhSachBanAsync(toId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chuyển bàn:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================
        //  GỘP BÀN
        // =========================
        private async void btnGopBan_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_banDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn bàn nguồn (bàn đang có đơn).", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
                {
                    MessageBox.Show("Bàn nguồn chưa có đơn đang mở để gộp.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int fromId = _banDangChon.Id;

                if (cboBanTo.SelectedItem is not BanDto banTo)
                {
                    MessageBox.Show("Vui lòng chọn bàn đích để gộp vào.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int toId = banTo.Id;

                if (toId == fromId)
                {
                    MessageBox.Show("Bàn đích phải khác bàn nguồn.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (banTo.TrangThai != STATUS_DANG_DUNG)
                {
                    MessageBox.Show("Bàn đích phải đang phục vụ (đang dùng) để gộp.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var tatCaDon = await _donGoiService.GetAllAsync();
                var donDich = tatCaDon
                    .Where(x => x.BanId == toId && x.TrangThai == 0)
                    .OrderByDescending(x => x.MoLuc)
                    .FirstOrDefault();

                if (donDich == null)
                {
                    MessageBox.Show("Bàn đích không có đơn đang mở để gộp.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dsNguon = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id)
                             ?? new List<ChiTietDonGoiDto>();
                var dsDich = await _ctDonGoiService.GetByDonGoiIdAsync(donDich.Id)
                            ?? new List<ChiTietDonGoiDto>();

                foreach (var ct in dsNguon)
                {
                    var same = dsDich.FirstOrDefault(x =>
                        x.ThucAnId == ct.ThucAnId && x.ThucUongId == ct.ThucUongId);

                    if (same != null)
                    {
                        var reqUpdateSame = new
                        {
                            DonGoiId = same.DonGoiId,
                            ThucAnId = same.ThucAnId,
                            ThucUongId = same.ThucUongId,
                            SoLuong = same.SoLuong + ct.SoLuong,
                            DonGia = same.DonGia,
                            ChietKhau = same.ChietKhau,
                            GhiChu = same.GhiChu
                        };
                        await _ctDonGoiService.UpdateAsync(same.Id, reqUpdateSame);

                        await _ctDonGoiService.DeleteAsync(ct.Id);
                    }
                    else
                    {
                        var reqMove = new
                        {
                            DonGoiId = donDich.Id,
                            ThucAnId = ct.ThucAnId,
                            ThucUongId = ct.ThucUongId,
                            SoLuong = ct.SoLuong,
                            DonGia = ct.DonGia,
                            ChietKhau = ct.ChietKhau,
                            GhiChu = ct.GhiChu
                        };
                        await _ctDonGoiService.UpdateAsync(ct.Id, reqMove);
                    }
                }

                var reqCloseNguon = new
                {
                    NhanVienId = _donGoiHienTai.NhanVienId,
                    BanId = _donGoiHienTai.BanId,
                    MoLuc = _donGoiHienTai.MoLuc,
                    DongLuc = DateTime.Now,
                    TrangThai = 2,
                    GhiChu = _donGoiHienTai.GhiChu
                };

                await ApiClient.PutAsync<string>($"api/DonGoi/{_donGoiHienTai.Id}", reqCloseNguon, includeAuth: true);

                await UpdateBanTrangThaiAsync(fromId, STATUS_TRONG);

                await LoadDanhSachBanAsync(toId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gộp bàn:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== IN TẠM (PRE-BILL) ==================
        private async Task<bool> ShowPreBillAsync()
        {
            if (_banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
            {
                MessageBox.Show("Bàn này chưa có đơn đang mở để in tạm.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);
            if (dsChiTiet == null || dsChiTiet.Count == 0)
            {
                MessageBox.Show("Đơn hiện tại chưa có món nào.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            using (var f = new FormHoaDonTam(_donGoiHienTai.Id))
            {
                f.ShowDialog(this);
            }

            return true;
        }

        private async void btnInTam_Click(object? sender, EventArgs e)
        {
            try { await ShowPreBillAsync(); }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in tạm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== THANH TOÁN FULL ==================
        private async void btnThanhToan_Click(object? sender, EventArgs e)
        {
            if (_banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
            {
                MessageBox.Show("Bàn này chưa có đơn gọi đang mở.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);
            if (dsChiTiet == null || dsChiTiet.Count == 0)
            {
                MessageBox.Show("Đơn gọi rỗng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal tong = dsChiTiet.Sum(x => x.DonGia * x.SoLuong);

            using var fChon = new FormChonThanhToan(tong);
            if (fChon.ShowDialog(this) != DialogResult.OK) return;

            string method = fChon.PhuongThuc;
            decimal khachDua = fChon.KhachDua;
            decimal tienThoi = fChon.TienThoi;

            Image? qrImg = null;
            if (method == "Chuyển khoản")
            {
                using var fqr = new FormThanhToanQR(tong, _donGoiHienTai.Id, _banDangChon.Id);
                var dr = fqr.ShowDialog(this);
                if (dr != DialogResult.OK || !fqr.DaNhanTien)
                {
                    MessageBox.Show("Chưa xác nhận nhận tiền. Hủy thanh toán.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                qrImg = fqr.QrImage;
            }

            using (var bill = new FormHoaDon(
                _donGoiHienTai.Id,
                overrideLines: null,
                qrImage: qrImg,
                phuongThuc: method,
                khachDua: khachDua,
                tienThoi: tienThoi,
                tongOverride: tong))
            {
                bill.ShowDialog(this);
            }

            int keepBanId = _banDangChon.Id;

            await CloseDonGoiSauThanhToanAsync();
            await LoadDanhSachBanAsync(keepBanId);
        }

        // ================== THANH TOÁN TỪNG PHẦN ==================
        private async void btnThanhToanTungPhan_Click(object? sender, EventArgs e)
        {
            if (_banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
            {
                MessageBox.Show("Bàn này chưa có đơn gọi đang mở.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);
            if (dsChiTiet == null || dsChiTiet.Count == 0)
            {
                MessageBox.Show("Đơn gọi rỗng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var items = new List<(ChiTietDonGoiDto ct, string tenMon)>();
            foreach (var ct in dsChiTiet)
            {
                string tenMon = "";
                if (ct.ThucAnId.HasValue)
                    tenMon = _dsThucAn.FirstOrDefault(x => x.Id == ct.ThucAnId.Value)?.Ten ?? "Món ăn";
                else if (ct.ThucUongId.HasValue)
                    tenMon = _dsThucUong.FirstOrDefault(x => x.Id == ct.ThucUongId.Value)?.Ten ?? "Thức uống";
                items.Add((ct, tenMon));
            }

            using var f = new FormThanhToanTungPhan(items, _donGoiHienTai.Id, _banDangChon.Id);
            if (f.ShowDialog(this) != DialogResult.OK) return;

            var lines = f.ResultLines;
            if (lines == null || lines.Count == 0) return;

            decimal totalPay = lines.Sum(x => x.DonGia * x.PayQty);
            var method = f.PhuongThuc;
            var khachDua = f.KhachDua;
            var tienThoi = f.TienThoi;
            var qrImg = f.QrImage;

            var billLines = lines.Select(x => new ChiTietHoaDonDto
            {
                TenMon = x.TenMon,
                SoLuong = x.PayQty,
                DonGia = x.DonGia,
                ThanhTien = x.DonGia * x.PayQty
            }).ToList();

            using (var bill = new FormHoaDon(
                _donGoiHienTai.Id,
                overrideLines: billLines,
                qrImage: qrImg,
                phuongThuc: method,
                khachDua: khachDua,
                tienThoi: tienThoi,
                tongOverride: totalPay))
            {
                bill.ShowDialog(this);
            }

            await ApplyPartialPaymentAsync(lines);

            int keepBanId = _banDangChon.Id;
            await LoadDanhSachBanAsync(keepBanId);
        }

        private async Task ApplyPartialPaymentAsync(List<FormThanhToanTungPhan.PayLine> lines)
        {
            if (_donGoiHienTai == null || _banDangChon == null) return;

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);

            foreach (var line in lines)
            {
                var ct = dsChiTiet.FirstOrDefault(x => x.Id == line.ChiTietId);
                if (ct == null) continue;

                int conLai = ct.SoLuong - line.PayQty;
                if (conLai <= 0)
                {
                    await _ctDonGoiService.DeleteAsync(ct.Id);
                }
                else
                {
                    var reqUpdate = new
                    {
                        DonGoiId = ct.DonGoiId,
                        ThucAnId = ct.ThucAnId,
                        ThucUongId = ct.ThucUongId,
                        SoLuong = conLai,
                        DonGia = ct.DonGia,
                        ChietKhau = ct.ChietKhau,
                        GhiChu = ct.GhiChu
                    };

                    await ApiClient.PutAsync<string>(
                        $"api/ChiTietDonGoi/{ct.Id}",
                        reqUpdate,
                        includeAuth: true
                    );
                }
            }

            var con = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);
            if (con.Count == 0)
            {
                await CloseDonGoiSauThanhToanAsync();
            }
        }

        private async Task CloseDonGoiSauThanhToanAsync()
        {
            if (_donGoiHienTai == null) return;

            int banId = _donGoiHienTai.BanId;

            var reqCloseDon = new
            {
                NhanVienId = _donGoiHienTai.NhanVienId,
                BanId = _donGoiHienTai.BanId,
                MoLuc = _donGoiHienTai.MoLuc,
                DongLuc = DateTime.Now,
                TrangThai = 1,
                GhiChu = _donGoiHienTai.GhiChu
            };

            await ApiClient.PutAsync<string>(
                $"api/DonGoi/{_donGoiHienTai.Id}",
                reqCloseDon,
                includeAuth: true
            );

            await UpdateBanTrangThaiAsync(banId, STATUS_TRONG);

            _donGoiHienTai = null;
            _chiTietDangChonId = null;
        }

        private void lblXinChao_Click(object sender, EventArgs e) { }
    }
}
