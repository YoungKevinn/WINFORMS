using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormQuanLyBan : Form
    {
        // ====== UI controls ======
        private Panel panelTitle;
        private Label lblTitle;

        private Panel panelFilter;
        private Button btnFilterAll;
        private Button btnFilterTrong;
        private Button btnFilterDangPhucVu;
        private Label lblThongKe;

        // DÙ tên là panelTables nhưng thực tế là FlowLayoutPanel
        private Panel panelTables;   // sẽ new FlowLayoutPanel

        private Panel panelBottom;
        private Label lblBanFrom;
        private ComboBox cboBanFrom;
        private Label lblBanTo;
        private ComboBox cboBanTo;
        private Button btnChuyenBan;
        private Button btnGopBan;

        // ====== Data từ API ======
        private List<BanDto> _dsBan = new();
        private readonly Dictionary<BanDto, Button> _banButtons = new();

        private List<ThucAnDto> _dsThucAn = new();
        private List<ThucUongDto> _dsThucUong = new();

        private readonly DonGoiService _donGoiService = new();
        private readonly ChiTietDonGoiService _ctDonGoiService = new();
        private readonly ThucAnService _thucAnService = new();
        private readonly ThucUongService _thucUongService = new();

        private enum FilterMode { TatCa, Trong, DangPhucVu }

        private FilterMode _currentFilter = FilterMode.TatCa;
        private BanDto? _banDangChon;
        private Button? _activeFilterButton;

        // ===========================================================
        // CTOR
        // ===========================================================
        public FormQuanLyBan()
        {
            Text = "Quản lý bàn";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 600);
            Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            BackColor = Color.White;
            AutoScaleMode = AutoScaleMode.None;

            // giảm flicker
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            BuildLayout();
            AttachEvents();
        }

        // ===========================================================
        // 1. XÂY DỰNG UI
        // ===========================================================
        private void BuildLayout()
        {
            // ----- TITLE -----
            panelTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };

            lblTitle = new Label
            {
                Text = "QUẢN LÝ BÀN",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            panelTitle.Controls.Add(lblTitle);

            // ----- FILTER -----
            panelFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            btnFilterAll = new Button
            {
                Text = "Tất cả",
                Width = 80,
                Height = 32,
                Location = new Point(20, 9)
            };
            btnFilterTrong = new Button
            {
                Text = "Còn trống",
                Width = 90,
                Height = 32,
                Location = new Point(110, 9)
            };
            btnFilterDangPhucVu = new Button
            {
                Text = "Đang dùng",
                Width = 90,
                Height = 32,
                Location = new Point(210, 9)
            };

            lblThongKe = new Label
            {
                AutoSize = true,
                Location = new Point(320, 15),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            panelFilter.Controls.AddRange(new Control[]
            {
                btnFilterAll, btnFilterTrong, btnFilterDangPhucVu, lblThongKe
            });

            // ----- PANEL BÀN (LƯỚI) -----
            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(40, 40, 40, 40)
            };
            flp.DoubleBuffered(true);

            panelTables = flp;       // giữ kiểu Panel nhưng gán FlowLayoutPanel

            // ----- BOTTOM: chuyển / gộp bàn -----
            panelBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.White
            };

            lblBanFrom = new Label
            {
                Text = "Bàn nguồn:",
                AutoSize = true,
                Location = new Point(20, 20)
            };
            cboBanFrom = new ComboBox
            {
                Location = new Point(100, 16),
                Width = 130,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            lblBanTo = new Label
            {
                Text = "Bàn đích:",
                AutoSize = true,
                Location = new Point(250, 20)
            };
            cboBanTo = new ComboBox
            {
                Location = new Point(320, 16),
                Width = 130,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnChuyenBan = new Button
            {
                Text = "Chuyển bàn",
                Width = 100,
                Height = 30,
                Location = new Point(470, 15)
            };

            btnGopBan = new Button
            {
                Text = "Gộp bàn",
                Width = 100,
                Height = 30,
                Location = new Point(580, 15)
            };

            panelBottom.Controls.AddRange(new Control[]
            {
                lblBanFrom, cboBanFrom,
                lblBanTo, cboBanTo,
                btnChuyenBan, btnGopBan
            });

            // ====== THỨ TỰ DOCK ======
            Controls.Add(panelTables);   // Fill
            Controls.Add(panelBottom);   // Bottom
            Controls.Add(panelFilter);   // Top 2
            Controls.Add(panelTitle);    // Top 1
        }

        private void AttachEvents()
        {
            Load += FormQuanLyBan_Load;

            btnFilterAll.Click += (s, e) =>
            {
                SetActiveFilter(btnFilterAll);
                _currentFilter = FilterMode.TatCa;
                ApplyFilter();
            };
            btnFilterTrong.Click += (s, e) =>
            {
                SetActiveFilter(btnFilterTrong);
                _currentFilter = FilterMode.Trong;
                ApplyFilter();
            };
            btnFilterDangPhucVu.Click += (s, e) =>
            {
                SetActiveFilter(btnFilterDangPhucVu);
                _currentFilter = FilterMode.DangPhucVu;
                ApplyFilter();
            };

            btnChuyenBan.Click += BtnChuyenBan_Click;
            btnGopBan.Click += BtnGopBan_Click;
        }

        // ===========================================================
        // 2. LOAD DỮ LIỆU TỪ API
        // ===========================================================
        private async void FormQuanLyBan_Load(object? sender, EventArgs e)
        {
            await LoadDataAsync();
            BuildButtons();
            BindCombos();

            SetActiveFilter(btnFilterAll);
            _currentFilter = FilterMode.TatCa;
            ApplyFilter();
        }

        private async Task LoadDataAsync()
        {
            // Bàn
            _dsBan = await BanService.GetAllAsync();

            // Cache tên món để hiển thị chi tiết đơn cho admin
            _dsThucAn = await _thucAnService.GetAllAsync();
            _dsThucUong = await _thucUongService.GetAllAsync();
        }

        private void BuildButtons()
        {
            panelTables.Controls.Clear();
            _banButtons.Clear();

            foreach (var ban in _dsBan.OrderBy(b => b.Id))
            {
                var btn = CreateBanButton(ban);
                _banButtons[ban] = btn;
                panelTables.Controls.Add(btn);
            }
        }

        private void BindCombos()
        {
            cboBanFrom.DataSource = null;
            cboBanTo.DataSource = null;

            if (_dsBan.Count == 0) return;

            // clone list để 2 combobox không share cùng DataSource
            cboBanFrom.DataSource = _dsBan.ToList();
            cboBanFrom.DisplayMember = "TenBan";
            cboBanFrom.ValueMember = "Id";

            cboBanTo.DataSource = _dsBan.ToList();
            cboBanTo.DisplayMember = "TenBan";
            cboBanTo.ValueMember = "Id";

            if (cboBanFrom.Items.Count > 0)
                cboBanFrom.SelectedIndex = 0;
            if (cboBanTo.Items.Count > 1)
                cboBanTo.SelectedIndex = 1;
        }

        // ===========================================================
        // 3. FILTER
        // ===========================================================
        private bool MatchCurrentFilter(BanDto ban)
        {
            return _currentFilter switch
            {
                FilterMode.Trong => ban.TrangThai == 0,
                FilterMode.DangPhucVu => ban.TrangThai == 1,
                _ => true,
            };
        }

        private void ApplyFilter()
        {
            var filtered = _dsBan.Where(MatchCurrentFilter)
                                 .OrderBy(b => b.Id)
                                 .ToList();

            foreach (var kv in _banButtons)
            {
                bool show = filtered.Contains(kv.Key);
                kv.Value.Visible = show;
            }

            UpdateThongKeLabel(filtered.Count);
        }

        private Button CreateBanButton(BanDto ban)
        {
            string displayName = string.IsNullOrWhiteSpace(ban.TenBan)
                ? $"BÀN {ban.Id}"
                : ban.TenBan.ToUpperInvariant();

            var btn = new Button
            {
                Text = displayName,
                Tag = ban,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 150,
                Height = 90,
                Margin = new Padding(20)
            };
            btn.FlatAppearance.BorderSize = 2;

            ApplyColorByStatus(btn, ban.TrangThai);
            btn.Click += BanButton_Click;

            return btn;
        }

        private async void BanButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not BanDto ban) return;

            foreach (var b in panelTables.Controls.OfType<Button>())
                b.FlatAppearance.BorderSize = 2;

            btn.FlatAppearance.BorderSize = 4;   // button đang chọn dày hơn
            _banDangChon = ban;

            await ShowThongTinDonGoiAsync(ban);
        }

        private void UpdateBanButton(BanDto ban)
        {
            if (_banButtons.TryGetValue(ban, out var btn))
            {
                ApplyColorByStatus(btn, ban.TrangThai);
            }
        }

        private void ApplyColorByStatus(Button btn, int trangThai)
        {
            if (trangThai == 0) // Trống
            {
                btn.BackColor = Color.FromArgb(240, 245, 255);
                btn.ForeColor = Color.RoyalBlue;
                btn.FlatAppearance.BorderColor = Color.RoyalBlue;
            }
            else // Đang phục vụ
            {
                btn.BackColor = Color.FromArgb(0, 120, 215);
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 215);
            }
        }

        // Hiển thị chi tiết đơn + ghi chú cho ADMIN
        private async Task ShowThongTinDonGoiAsync(BanDto ban)
        {
            var donMo = await GetDonGoiDangMoTheoBanAsync(ban.Id);
            if (donMo == null)
            {
                MessageBox.Show(
                    $"{ban.TenBan} hiện không có đơn đang phục vụ.",
                    "Thông tin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(donMo.Id);
            if (dsChiTiet.Count == 0)
            {
                MessageBox.Show(
                    $"{ban.TenBan} đang có đơn mở nhưng chưa có món nào.",
                    "Thông tin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var vi = new CultureInfo("vi-VN");
            decimal tong = 0;
            var lines = new List<string>
            {
                $"Bàn: {ban.TenBan}",
                $"Đơn ID: {donMo.Id} - Mở lúc: {donMo.MoLuc:g}",
            };

            if (!string.IsNullOrWhiteSpace(donMo.GhiChu))
            {
                lines.Add($"Ghi chú đơn: {donMo.GhiChu}");
            }

            lines.Add("");
            lines.Add("Danh sách món:");

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

                var thanhTien = ct.DonGia * ct.SoLuong;
                tong += thanhTien;

                string ghiChuCt = string.IsNullOrWhiteSpace(ct.GhiChu)
                    ? ""
                    : $" - Ghi chú: {ct.GhiChu}";

                lines.Add(
                    $"{ct.SoLuong} x {tenMon} ({ct.DonGia.ToString("N0", vi)}) = {thanhTien.ToString("N0", vi)}{ghiChuCt}");
            }

            lines.Add("");
            lines.Add($"TỔNG: {tong.ToString("N0", vi)} đ");

            string message = string.Join(Environment.NewLine, lines);

            MessageBox.Show(
                message,
                "Đơn đang phục vụ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ===========================================================
        // 4. THỐNG KÊ & NÚT FILTER
        // ===========================================================
        private void UpdateThongKeLabel(int visibleCount)
        {
            int tong = _dsBan.Count;
            int soTrong = _dsBan.Count(b => b.TrangThai == 0);
            int soDangPhucVu = _dsBan.Count(b => b.TrangThai == 1);

            lblThongKe.Text =
                $"Filter: {_currentFilter} | Hiện: {visibleCount} bàn | " +
                $"Tổng: {tong} | Còn trống: {soTrong} | Đang phục vụ: {soDangPhucVu}";
        }

        private void SetActiveFilter(Button btn)
        {
            _activeFilterButton = btn;

            void Style(Button b, bool active)
            {
                b.BackColor = active ? Color.White : Color.Gainsboro;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = active ? 2 : 1;
                b.FlatAppearance.BorderColor = active ? Color.RoyalBlue : Color.Silver;
            }

            Style(btnFilterAll, btn == btnFilterAll);
            Style(btnFilterTrong, btn == btnFilterTrong);
            Style(btnFilterDangPhucVu, btn == btnFilterDangPhucVu);
        }

        // ===========================================================
        // 5. CHUYỂN / GỘP BÀN (LÀM THẬT VỚI API)
        // ===========================================================
        private async Task<DonGoiDto?> GetDonGoiDangMoTheoBanAsync(int banId)
        {
            var tatCaDonGoi = await _donGoiService.GetAllAsync();
            return tatCaDonGoi
                .Where(d => d.BanId == banId && d.TrangThai == 0)
                .OrderByDescending(d => d.MoLuc)
                .FirstOrDefault();
        }

        private async void BtnChuyenBan_Click(object? sender, EventArgs e)
        {
            if (cboBanFrom.SelectedItem is not BanDto banNguon ||
                cboBanTo.SelectedItem is not BanDto banDich)
            {
                MessageBox.Show("Vui lòng chọn bàn nguồn và bàn đích.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (banNguon.Id == banDich.Id)
            {
                MessageBox.Show("Bàn nguồn và bàn đích phải khác nhau.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var donNguon = await GetDonGoiDangMoTheoBanAsync(banNguon.Id);
            if (donNguon == null)
            {
                MessageBox.Show($"{banNguon.TenBan} hiện không có đơn đang phục vụ.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var donDich = await GetDonGoiDangMoTheoBanAsync(banDich.Id);
            if (donDich != null)
            {
                MessageBox.Show("Bàn đích đã có đơn đang phục vụ. Nếu muốn gộp, hãy dùng chức năng Gộp bàn.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Cập nhật DonGoi: đổi BanId sang bàn đích
            var reqUpdateDonGoi = new
            {
                NhanVienId = donNguon.NhanVienId,
                BanId = banDich.Id,
                MoLuc = donNguon.MoLuc,
                DongLuc = donNguon.DongLuc,
                TrangThai = donNguon.TrangThai,
                GhiChu = donNguon.GhiChu
            };

            await ApiClient.PutAsync<string>(
                $"api/DonGoi/{donNguon.Id}",
                reqUpdateDonGoi,
                includeAuth: true
            );

            // Cập nhật trạng thái bàn
            banNguon.TrangThai = 0;
            banDich.TrangThai = 1;

            await ApiClient.PutAsync<string>($"api/Ban/{banNguon.Id}", banNguon, includeAuth: true);
            await ApiClient.PutAsync<string>($"api/Ban/{banDich.Id}", banDich, includeAuth: true);

            await LoadDataAsync();
            BuildButtons();
            BindCombos();
            ApplyFilter();

            MessageBox.Show(
                $"Đã chuyển đơn đang phục vụ từ {banNguon.TenBan} sang {banDich.TenBan}.",
                "Chuyển bàn",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private async void BtnGopBan_Click(object? sender, EventArgs e)
        {
            if (cboBanFrom.SelectedItem is not BanDto banNguon ||
                cboBanTo.SelectedItem is not BanDto banDich)
            {
                MessageBox.Show("Vui lòng chọn bàn nguồn và bàn đích.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (banNguon.Id == banDich.Id)
            {
                MessageBox.Show("Bàn nguồn và bàn đích phải khác nhau.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var donNguon = await GetDonGoiDangMoTheoBanAsync(banNguon.Id);
            var donDich = await GetDonGoiDangMoTheoBanAsync(banDich.Id);

            if (donNguon == null || donDich == null)
            {
                MessageBox.Show("Cả bàn nguồn và bàn đích đều phải đang phục vụ (có đơn mở).",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(
                    $"Gộp tất cả món từ {banNguon.TenBan} sang {banDich.TenBan}?",
                    "Xác nhận gộp bàn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                return;

            // Lấy tất cả chi tiết của đơn nguồn
            var chiTietNguon = await _ctDonGoiService.GetByDonGoiIdAsync(donNguon.Id);

            foreach (var ct in chiTietNguon)
            {
                var reqUpdateCt = new
                {
                    DonGoiId = donDich.Id,
                    ThucAnId = ct.ThucAnId,
                    ThucUongId = ct.ThucUongId,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    ChietKhau = ct.ChietKhau,
                    GhiChu = ct.GhiChu
                };

                await ApiClient.PutAsync<string>(
                    $"api/ChiTietDonGoi/{ct.Id}",
                    reqUpdateCt,
                    includeAuth: true
                );
            }

            // Đóng đơn nguồn
            var reqCloseDonNguon = new
            {
                NhanVienId = donNguon.NhanVienId,
                BanId = donNguon.BanId,
                MoLuc = donNguon.MoLuc,
                DongLuc = DateTime.Now,
                TrangThai = 1,
                GhiChu = donNguon.GhiChu
            };

            await ApiClient.PutAsync<string>(
                $"api/DonGoi/{donNguon.Id}",
                reqCloseDonNguon,
                includeAuth: true
            );

            // Cập nhật trạng thái bàn nguồn về TRỐNG
            banNguon.TrangThai = 0;
            await ApiClient.PutAsync<string>($"api/Ban/{banNguon.Id}", banNguon, includeAuth: true);

            await LoadDataAsync();
            BuildButtons();
            BindCombos();
            ApplyFilter();

            MessageBox.Show(
                $"Đã gộp {banNguon.TenBan} vào {banDich.TenBan}.",
                "Gộp bàn",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    // ====== Extension nhỏ để bật DoubleBuffer cho Control ======
    internal static class ControlExtensions
    {
        public static void DoubleBuffered(this Control control, bool enable)
        {
            var prop = typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic);
            prop?.SetValue(control, enable, null);
        }
    }
}
