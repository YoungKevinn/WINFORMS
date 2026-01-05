using Client_DoMInhKhoa.Helpers;
using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// ✅ Chỉ dùng chart của WinForms
using System.Windows.Forms.DataVisualization.Charting;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBaoCaoDoanhThu : Form
    {
        private readonly BaoCaoService _baoCaoService = new BaoCaoService();
        private readonly CultureInfo _viCulture = CultureInfo.GetCultureInfo("vi-VN");

        // Lưu kiểu thống kê hiện tại: "ngay" | "thang" | "nam"
        private string _currentKieuThongKe = "ngay";

        // Dữ liệu gốc (từ API)
        private List<ThongKeDoanhThuDto> _allTongQuan = new();
        private List<ThongKeDoanhThuNhanVienDto> _allTheoNhanVien = new();

        // Dữ liệu đang hiển thị (đã lọc / sort)
        private List<ThongKeDoanhThuDto> _currentTongQuan = new();
        private List<ThongKeDoanhThuNhanVienDto> _currentTheoNhanVien = new();

        public FormBaoCaoDoanhThu()
        {
            InitializeComponent();

            // Định dạng ngày dd/MM/yyyy cho DateTimePicker
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.CustomFormat = "dd/MM/yyyy";
            dtpTo.Format = DateTimePickerFormat.Custom;
            dtpTo.CustomFormat = "dd/MM/yyyy";

            // Mặc định: từ đầu tháng tới hôm nay
            dtpTo.Value = DateTime.Today;
            dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            // Kiểu thống kê
            cboKieuThongKe.Items.AddRange(new object[]
            {
                "Theo ngày",
                "Theo tháng",
                "Theo năm"
            });
            cboKieuThongKe.SelectedIndex = 0;

            // Format lại cột "Ngày/Tháng/Năm" trong grid tổng quan
            dgvTongQuan.CellFormatting += dgvTongQuan_CellFormatting;

            // Bắt event click header để sắp xếp
            dgvTongQuan.ColumnHeaderMouseClick += dgvTongQuan_ColumnHeaderMouseClick;
            dgvNhanVien.ColumnHeaderMouseClick += dgvNhanVien_ColumnHeaderMouseClick;
        }

        private string FormatCurrency(decimal value)
        {
            return string.Format(_viCulture, "{0:N0}", value);
        }

        private async void btnXemBaoCao_Click(object sender, EventArgs e)
        {
            btnXemBaoCao.Enabled = false;
            try
            {
                var from = dtpFrom.Value.Date;
                var to = dtpTo.Value.Date;

                var kieu = "ngay";
                if (cboKieuThongKe.SelectedIndex == 1) kieu = "thang";
                else if (cboKieuThongKe.SelectedIndex == 2) kieu = "nam";

                _currentKieuThongKe = kieu;

                var tongQuanTask = _baoCaoService.GetThongKeDoanhThuAsync(from, to, kieu);
                var nhanVienTask = _baoCaoService.GetThongKeDoanhThuNhanVienAsync(from, to);

                await Task.WhenAll(tongQuanTask, nhanVienTask);

                _allTongQuan = tongQuanTask.Result ?? new List<ThongKeDoanhThuDto>();
                _allTheoNhanVien = nhanVienTask.Result ?? new List<ThongKeDoanhThuNhanVienDto>();

                ApplySearchFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnXemBaoCao.Enabled = true;
            }
        }

        private void btnXemBieuDo_Click(object sender, EventArgs e)
        {
            if (_allTongQuan == null || _allTongQuan.Count == 0)
            {
                MessageBox.Show("Bạn hãy bấm \"Xem báo cáo\" trước để tải dữ liệu doanh thu.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var data = (_currentTongQuan != null && _currentTongQuan.Count > 0)
                ? _currentTongQuan
                : _allTongQuan;

            using var f = new FormBieuDoDoanhThu(data, _currentKieuThongKe, dtpFrom.Value.Date, dtpTo.Value.Date);
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);
        }

        // ====== LỌC THEO Ô TÌM KIẾM CHUNG ======
        private void ApplySearchFilter()
        {
            var keyword = txtSearch?.Text.Trim();
            IEnumerable<ThongKeDoanhThuDto> tong = _allTongQuan ?? new List<ThongKeDoanhThuDto>();
            IEnumerable<ThongKeDoanhThuNhanVienDto> nv = _allTheoNhanVien ?? new List<ThongKeDoanhThuNhanVienDto>();

            if (!string.IsNullOrEmpty(keyword))
            {
                var kw = keyword.ToLower();

                tong = tong.Where(x =>
                    x.Ngay.ToString("dd/MM/yyyy").Contains(kw) ||
                    x.Ngay.ToString("MM/yyyy").Contains(kw) ||
                    x.Ngay.ToString("yyyy").Contains(kw) ||
                    x.SoHoaDon.ToString().Contains(kw) ||
                    x.TongTien.ToString(CultureInfo.InvariantCulture).Contains(kw)
                );

                nv = nv.Where(x =>
                    x.NhanVienId.ToString().Contains(kw) ||
                    (!string.IsNullOrEmpty(x.MaNhanVien) && x.MaNhanVien.ToLower().Contains(kw)) ||
                    (!string.IsNullOrEmpty(x.HoTen) && x.HoTen.ToLower().Contains(kw)) ||
                    x.SoHoaDon.ToString().Contains(kw) ||
                    x.TongTien.ToString(CultureInfo.InvariantCulture).Contains(kw)
                );
            }

            _currentTongQuan = tong.ToList();
            _currentTheoNhanVien = nv.ToList();

            BindTongQuan(_currentTongQuan, _currentKieuThongKe);
            BindNhanVien(_currentTheoNhanVien);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        // ====== SORT GENERIC ======
        private List<T> SortList<T>(List<T> source, string propName, bool ascending)
        {
            var prop = typeof(T).GetProperty(propName);
            if (prop == null) return source;

            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (type == typeof(int) || type == typeof(short) || type == typeof(long))
            {
                Func<T, long> key = x =>
                {
                    var v = prop.GetValue(x);
                    if (v == null) return long.MinValue;
                    return Convert.ToInt64(v);
                };
                return ascending ? source.OrderBy(key).ToList() : source.OrderByDescending(key).ToList();
            }

            if (type == typeof(decimal) || type == typeof(double) || type == typeof(float))
            {
                Func<T, decimal> key = x =>
                {
                    var v = prop.GetValue(x);
                    if (v == null) return decimal.MinValue;
                    return Convert.ToDecimal(v);
                };
                return ascending ? source.OrderBy(key).ToList() : source.OrderByDescending(key).ToList();
            }

            if (type == typeof(DateTime))
            {
                Func<T, DateTime> key = x =>
                {
                    var v = prop.GetValue(x);
                    if (v == null) return DateTime.MinValue;
                    return (DateTime)v;
                };
                return ascending ? source.OrderBy(key).ToList() : source.OrderByDescending(key).ToList();
            }

            if (type == typeof(string))
            {
                Func<T, string> key = x => prop.GetValue(x)?.ToString() ?? string.Empty;
                return ascending ? source.OrderBy(key).ToList() : source.OrderByDescending(key).ToList();
            }

            Func<T, string> keyFallback = x => prop.GetValue(x)?.ToString() ?? string.Empty;
            return ascending ? source.OrderBy(keyFallback).ToList() : source.OrderByDescending(keyFallback).ToList();
        }

        // ====== BIND TỔNG QUAN ======
        private void BindTongQuan(List<ThongKeDoanhThuDto> data, string kieu)
        {
            _currentTongQuan = data ?? new List<ThongKeDoanhThuDto>();

            dgvTongQuan.DataSource = null;
            dgvTongQuan.DataSource = _currentTongQuan;

            dgvTongQuan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTongQuan.ReadOnly = true;
            dgvTongQuan.AllowUserToAddRows = false;
            dgvTongQuan.AllowUserToDeleteRows = false;

            if (dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.Ngay)] != null)
            {
                dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.Ngay)].HeaderText =
                    kieu switch
                    {
                        "thang" => "Tháng",
                        "nam" => "Năm",
                        _ => "Ngày"
                    };

                dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.Ngay)].DefaultCellStyle.Format =
                    kieu switch
                    {
                        "thang" => "MM/yyyy",
                        "nam" => "yyyy",
                        _ => "dd/MM/yyyy"
                    };
            }

            if (dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.SoHoaDon)] != null)
                dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.SoHoaDon)].HeaderText = "Số hóa đơn";

            if (dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.TongTien)] != null)
            {
                var col = dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.TongTien)];
                col.HeaderText = "Tổng doanh thu";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Format = "N0";
                col.DefaultCellStyle.FormatProvider = _viCulture;
            }

            foreach (DataGridViewColumn col in dgvTongQuan.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            var tongTien = _currentTongQuan.Sum(x => x.TongTien);
            var soHoaDon = _currentTongQuan.Sum(x => x.SoHoaDon);

            lblTongThu.Text = $"Tổng doanh thu: {FormatCurrency(tongTien)}";
            lblSoHoaDon.Text = $"Số hóa đơn: {soHoaDon}";
        }

        // ====== BIND THEO NHÂN VIÊN ======
        private void BindNhanVien(List<ThongKeDoanhThuNhanVienDto> data)
        {
            _currentTheoNhanVien = data ?? new List<ThongKeDoanhThuNhanVienDto>();

            dgvNhanVien.DataSource = null;
            dgvNhanVien.DataSource = _currentTheoNhanVien;

            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.AllowUserToDeleteRows = false;

            if (dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.NhanVienId)] != null)
                dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.NhanVienId)].Visible = false;

            if (dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.MaNhanVien)] != null)
                dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.MaNhanVien)].HeaderText = "Mã NV";

            if (dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.HoTen)] != null)
                dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.HoTen)].HeaderText = "Họ tên";

            if (dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.SoHoaDon)] != null)
                dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.SoHoaDon)].HeaderText = "Số hóa đơn";

            if (dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.TongTien)] != null)
            {
                var col = dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.TongTien)];
                col.HeaderText = "Tổng doanh thu";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Format = "N0";
                col.DefaultCellStyle.FormatProvider = _viCulture;
            }

            foreach (DataGridViewColumn col in dgvNhanVien.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            var tongThu = _currentTheoNhanVien.Sum(x => x.TongTien);
            var soNhanVien = _currentTheoNhanVien.Count;

            lblTongThuNhanVien.Text = $"Tổng doanh thu: {FormatCurrency(tongThu)}";
            lblSoNhanVien.Text = $"Số nhân viên có hóa đơn: {soNhanVien}";
        }

        // ====== SORT TỔNG QUAN ======
        private void dgvTongQuan_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentTongQuan == null || _currentTongQuan.Count == 0) return;

            var column = dgvTongQuan.Columns[e.ColumnIndex];
            var prop = column.DataPropertyName;
            if (string.IsNullOrEmpty(prop)) return;

            bool ascending = column.HeaderCell.SortGlyphDirection != SortOrder.Ascending;
            string columnName = column.Name;

            _currentTongQuan = SortList(_currentTongQuan, prop, ascending);

            BindTongQuan(_currentTongQuan, _currentKieuThongKe);

            foreach (DataGridViewColumn col in dgvTongQuan.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;

            var currentColumn = dgvTongQuan.Columns[columnName];
            if (currentColumn != null)
                currentColumn.HeaderCell.SortGlyphDirection = ascending ? SortOrder.Ascending : SortOrder.Descending;
        }

        // ====== SORT NHÂN VIÊN ======
        private void dgvNhanVien_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentTheoNhanVien == null || _currentTheoNhanVien.Count == 0) return;

            var column = dgvNhanVien.Columns[e.ColumnIndex];
            var prop = column.DataPropertyName;
            if (string.IsNullOrEmpty(prop)) return;

            bool ascending = column.HeaderCell.SortGlyphDirection != SortOrder.Ascending;
            string columnName = column.Name;

            _currentTheoNhanVien = SortList(_currentTheoNhanVien, prop, ascending);

            BindNhanVien(_currentTheoNhanVien);

            foreach (DataGridViewColumn col in dgvNhanVien.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;

            var currentColumn = dgvNhanVien.Columns[columnName];
            if (currentColumn != null)
                currentColumn.HeaderCell.SortGlyphDirection = ascending ? SortOrder.Ascending : SortOrder.Descending;
        }

        private void dgvTongQuan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTongQuan.Columns[e.ColumnIndex].DataPropertyName != nameof(ThongKeDoanhThuDto.Ngay))
                return;

            if (e.Value is not DateTime dt)
                return;

            switch (_currentKieuThongKe)
            {
                case "ngay":
                    e.Value = dt.ToString("dd/MM/yyyy");
                    e.FormattingApplied = true;
                    break;

                case "thang":
                    if (dt.Year == 1)
                    {
                        if (dtpFrom.Value.Year == dtpTo.Value.Year &&
                            dtpFrom.Value.Month == dtpTo.Value.Month)
                            e.Value = dtpFrom.Value.ToString("MM/yyyy");
                        else
                            e.Value = $"{dtpFrom.Value:MM/yyyy} - {dtpTo.Value:MM/yyyy}";
                    }
                    else e.Value = dt.ToString("MM/yyyy");

                    e.FormattingApplied = true;
                    break;

                case "nam":
                    if (dt.Year == 1)
                    {
                        if (dtpFrom.Value.Year == dtpTo.Value.Year)
                            e.Value = dtpFrom.Value.ToString("yyyy");
                        else
                            e.Value = $"{dtpFrom.Value:yyyy} - {dtpTo.Value:yyyy}";
                    }
                    else e.Value = dt.ToString("yyyy");

                    e.FormattingApplied = true;
                    break;
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if ((_currentTongQuan == null || _currentTongQuan.Count == 0) &&
                (_currentTheoNhanVien == null || _currentTheoNhanVien.Count == 0))
            {
                MessageBox.Show("Chưa có dữ liệu. Hãy bấm \"Xem báo cáo\" trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var isTongQuan = tabControl.SelectedTab == tabTongQuan;

            var dgv = isTongQuan ? dgvTongQuan : dgvNhanVien;
            var sheet = isTongQuan ? "DoanhThuTong" : "TheoNhanVien";
            var title = isTongQuan ? "BÁO CÁO DOANH THU (TỔNG)" : "BÁO CÁO DOANH THU (THEO NHÂN VIÊN)";

            decimal? tong = null;
            if (isTongQuan && _currentTongQuan != null && _currentTongQuan.Count > 0)
                tong = _currentTongQuan.Sum(x => x.TongTien);
            else if (!isTongQuan && _currentTheoNhanVien != null && _currentTheoNhanVien.Count > 0)
                tong = _currentTheoNhanVien.Sum(x => x.TongTien);

            ExcelExporter.ExportDataGridViewToExcel(
                dgv,
                sheetName: sheet,
                title: title,
                fromDate: dtpFrom.Value.Date,
                toDate: dtpTo.Value.Date,
                totalRevenue: tong
            );
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            if ((_currentTongQuan == null || _currentTongQuan.Count == 0) &&
                (_currentTheoNhanVien == null || _currentTheoNhanVien.Count == 0))
            {
                MessageBox.Show("Chưa có dữ liệu. Hãy bấm \"Xem báo cáo\" trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool isTongQuan = tabControl.SelectedTab == tabTongQuan;

            var dgv = isTongQuan ? dgvTongQuan : dgvNhanVien;
            var sheet = isTongQuan ? "DoanhThuTong" : "TheoNhanVien";
            var title = isTongQuan ? "BÁO CÁO DOANH THU (TỔNG QUAN)" : "BÁO CÁO DOANH THU (THEO NHÂN VIÊN)";

            decimal? tong = null;
            if (isTongQuan && _currentTongQuan != null && _currentTongQuan.Count > 0)
                tong = _currentTongQuan.Sum(x => x.TongTien);
            else if (!isTongQuan && _currentTheoNhanVien != null && _currentTheoNhanVien.Count > 0)
                tong = _currentTheoNhanVien.Sum(x => x.TongTien);

            PdfExporter.ExportDataGridViewToPdf(
                dgv,
                title: title,
                sheetName: sheet,
                fromDate: dtpFrom.Value.Date,
                toDate: dtpTo.Value.Date,
                totalRevenue: tong
            );
        }

        // ==================== FORM BIỂU ĐỒ (WINFORMS) ====================
        internal sealed class FormBieuDoDoanhThu : Form
        {
            private readonly List<ThongKeDoanhThuDto> _data;
            private readonly string _kieu;
            private readonly DateTime _from;
            private readonly DateTime _to;

            private Chart _chart;
            private Label _lblTitle;

            public FormBieuDoDoanhThu(List<ThongKeDoanhThuDto> data, string kieu, DateTime from, DateTime to)
            {
                _data = data ?? new List<ThongKeDoanhThuDto>();
                _kieu = string.IsNullOrWhiteSpace(kieu) ? "ngay" : kieu;
                _from = from;
                _to = to;

                Text = "Biểu đồ doanh thu";
                Width = 980;
                Height = 560;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;

                BuildUi();
                DrawChart();
            }

            private void BuildUi()
            {
                _lblTitle = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 40,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Padding = new Padding(12, 0, 12, 0),
                    Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                    Text = $"Doanh thu ({FormatKieu(_kieu)})  |  {_from:dd/MM/yyyy} → {_to:dd/MM/yyyy}"
                };

                _chart = new Chart { Dock = DockStyle.Fill };

                var area = new ChartArea("main");
                area.AxisX.Interval = 1;
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisX.LabelStyle.Angle = -45;

                area.AxisY.LabelStyle.Format = "N0";
                area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

                area.AxisY2.Enabled = AxisEnabled.True;
                area.AxisY2.MajorGrid.Enabled = false;

                _chart.ChartAreas.Add(area);
                _chart.Legends.Add(new Legend("legend"));

                var sRevenue = new Series("Doanh thu")
                {
                    ChartType = SeriesChartType.Column,
                    ChartArea = "main",
                    IsValueShownAsLabel = true,
                    YValueType = ChartValueType.Double,
                    LabelFormat = "N0"
                };
                sRevenue["PointWidth"] = "0.6";

                var sInvoices = new Series("Số hóa đơn")
                {
                    ChartType = SeriesChartType.Line,
                    ChartArea = "main",
                    YAxisType = AxisType.Secondary,
                    BorderWidth = 3,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6,
                    YValueType = ChartValueType.Int32
                };

                _chart.Series.Add(sRevenue);
                _chart.Series.Add(sInvoices);

                Controls.Add(_chart);
                Controls.Add(_lblTitle);
            }

            private void DrawChart()
            {
                var sRevenue = _chart.Series["Doanh thu"];
                var sInvoices = _chart.Series["Số hóa đơn"];
                sRevenue.Points.Clear();
                sInvoices.Points.Clear();

                foreach (var item in _data.OrderBy(x => x.Ngay))
                {
                    var label = _kieu switch
                    {
                        "thang" => item.Ngay.ToString("MM/yyyy"),
                        "nam" => item.Ngay.ToString("yyyy"),
                        _ => item.Ngay.ToString("dd/MM")
                    };

                    var revenue = (double)GetRevenue(item);
                    sRevenue.Points.AddXY(label, revenue);
                    sInvoices.Points.AddXY(label, item.SoHoaDon);
                }

                _chart.ChartAreas[0].RecalculateAxesScale();
            }

            private static decimal GetRevenue(ThongKeDoanhThuDto x)
            {
                if (x == null) return 0m;

                var p = x.GetType().GetProperty("DoanhThuThucTe");
                if (p != null && p.PropertyType == typeof(decimal))
                {
                    var v = (decimal)(p.GetValue(x) ?? 0m);
                    if (v != 0m) return v;
                }

                return x.TongTien;
            }

            private static string FormatKieu(string kieu)
            {
                return kieu switch
                {
                    "thang" => "Theo tháng",
                    "nam" => "Theo năm",
                    _ => "Theo ngày"
                };
            }
        }
    }
}
