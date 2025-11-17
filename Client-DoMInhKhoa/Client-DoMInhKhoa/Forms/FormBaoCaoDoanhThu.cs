using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;

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
            this.dgvTongQuan.CellFormatting += dgvTongQuan_CellFormatting;

            // Bắt event click header để sắp xếp
            this.dgvTongQuan.ColumnHeaderMouseClick += dgvTongQuan_ColumnHeaderMouseClick;
            this.dgvNhanVien.ColumnHeaderMouseClick += dgvNhanVien_ColumnHeaderMouseClick;
        }

        private string FormatCurrency(decimal value)
        {
            // -> 25.000
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

                // lưu lại để CellFormatting biết đang ở mode nào
                _currentKieuThongKe = kieu;

                // gọi API
                var tongQuanTask = _baoCaoService.GetThongKeDoanhThuAsync(from, to, kieu);
                var nhanVienTask = _baoCaoService.GetThongKeDoanhThuNhanVienAsync(from, to);

                await Task.WhenAll(tongQuanTask, nhanVienTask);

                var tongQuan = tongQuanTask.Result ?? new List<ThongKeDoanhThuDto>();
                var theoNhanVien = nhanVienTask.Result ?? new List<ThongKeDoanhThuNhanVienDto>();

                // Lưu dữ liệu gốc
                _allTongQuan = tongQuan;
                _allTheoNhanVien = theoNhanVien;

                // Áp dụng lọc theo ô tìm kiếm (nếu có)
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
                    (!string.IsNullOrEmpty(x.MaNhanVien) &&
                        x.MaNhanVien.ToLower().Contains(kw)) ||
                    (!string.IsNullOrEmpty(x.HoTen) &&
                        x.HoTen.ToLower().Contains(kw)) ||
                    x.SoHoaDon.ToString().Contains(kw) ||
                    x.TongTien.ToString(CultureInfo.InvariantCulture).Contains(kw)
                );
            }

            _currentTongQuan = tong.ToList();
            _currentTheoNhanVien = nv.ToList();

            // Bind lại lưới + summary
            BindTongQuan(_currentTongQuan, _currentKieuThongKe);
            BindNhanVien(_currentTheoNhanVien);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        // ====== HÀM SORT GENERIC CHO MỌI CỘT ======
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
                return ascending
                    ? source.OrderBy(key).ToList()
                    : source.OrderByDescending(key).ToList();
            }
            if (type == typeof(decimal) || type == typeof(double) || type == typeof(float))
            {
                Func<T, decimal> key = x =>
                {
                    var v = prop.GetValue(x);
                    if (v == null) return decimal.MinValue;
                    return Convert.ToDecimal(v);
                };
                return ascending
                    ? source.OrderBy(key).ToList()
                    : source.OrderByDescending(key).ToList();
            }
            if (type == typeof(DateTime))
            {
                Func<T, DateTime> key = x =>
                {
                    var v = prop.GetValue(x);
                    if (v == null) return DateTime.MinValue;
                    return (DateTime)v;
                };
                return ascending
                    ? source.OrderBy(key).ToList()
                    : source.OrderByDescending(key).ToList();
            }
            if (type == typeof(string))
            {
                Func<T, string> key = x => prop.GetValue(x)?.ToString() ?? string.Empty;
                return ascending
                    ? source.OrderBy(key).ToList()
                    : source.OrderByDescending(key).ToList();
            }

            // fallback: sort theo ToString
            Func<T, string> keyFallback = x => prop.GetValue(x)?.ToString() ?? string.Empty;
            return ascending
                ? source.OrderBy(keyFallback).ToList()
                : source.OrderByDescending(keyFallback).ToList();
        }

        // ====== BIND & FORMAT TỔNG QUAN ======
        private void BindTongQuan(List<ThongKeDoanhThuDto> data, string kieu)
        {
            // Lưu lại để còn sắp xếp khi click header
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

                // Format cơ bản, CellFormatting sẽ override chi tiết
                dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.Ngay)].DefaultCellStyle.Format =
                    kieu switch
                    {
                        "thang" => "MM/yyyy",
                        "nam" => "yyyy",
                        _ => "dd/MM/yyyy"
                    };
            }

            if (dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.SoHoaDon)] != null)
            {
                dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.SoHoaDon)].HeaderText = "Số hóa đơn";
            }

            if (dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.TongTien)] != null)
            {
                var col = dgvTongQuan.Columns[nameof(ThongKeDoanhThuDto.TongTien)];
                col.HeaderText = "Tổng doanh thu";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Format = "N0"; // 25.000
                col.DefaultCellStyle.FormatProvider = _viCulture;
            }

            // Cho phép sort bằng code cho MỌI cột
            foreach (DataGridViewColumn col in dgvTongQuan.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            // summary
            var tongTien = _currentTongQuan.Sum(x => x.TongTien);
            var soHoaDon = _currentTongQuan.Sum(x => x.SoHoaDon);

            lblTongThu.Text = $"Tổng doanh thu: {FormatCurrency(tongTien)}";
            lblSoHoaDon.Text = $"Số hóa đơn: {soHoaDon}";
        }

        // ====== BIND & FORMAT THEO NHÂN VIÊN ======
        private void BindNhanVien(List<ThongKeDoanhThuNhanVienDto> data)
        {
            // Lưu lại để còn sắp xếp khi click header
            _currentTheoNhanVien = data ?? new List<ThongKeDoanhThuNhanVienDto>();

            dgvNhanVien.DataSource = null;
            dgvNhanVien.DataSource = _currentTheoNhanVien;

            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.AllowUserToDeleteRows = false;

            // Ẩn cột Id
            if (dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.NhanVienId)] != null)
            {
                dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.NhanVienId)].Visible = false;
            }

            if (dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.MaNhanVien)] != null)
            {
                dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.MaNhanVien)].HeaderText = "Mã NV";
            }

            if (dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.HoTen)] != null)
            {
                dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.HoTen)].HeaderText = "Họ tên";
            }

            if (dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.SoHoaDon)] != null)
            {
                dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.SoHoaDon)].HeaderText = "Số hóa đơn";
            }

            if (dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.TongTien)] != null)
            {
                var col = dgvNhanVien.Columns[nameof(ThongKeDoanhThuNhanVienDto.TongTien)];
                col.HeaderText = "Tổng doanh thu";
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Format = "N0"; // 25.000
                col.DefaultCellStyle.FormatProvider = _viCulture;
            }

            // Cho phép sort bằng code cho MỌI cột
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

        // ====== SẮP XẾP GRID DOANH THU TỔNG (MỌI CỘT) ======
        private void dgvTongQuan_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentTongQuan == null || _currentTongQuan.Count == 0) return;

            var column = dgvTongQuan.Columns[e.ColumnIndex];
            var prop = column.DataPropertyName;
            if (string.IsNullOrEmpty(prop)) return;

            bool ascending = column.HeaderCell.SortGlyphDirection != SortOrder.Ascending;
            string columnName = column.Name;

            _currentTongQuan = SortList(_currentTongQuan, prop, ascending);

            // Rebind + giữ format
            BindTongQuan(_currentTongQuan, _currentKieuThongKe);

            foreach (DataGridViewColumn col in dgvTongQuan.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;

            var currentColumn = dgvTongQuan.Columns[columnName];
            if (currentColumn != null)
            {
                currentColumn.HeaderCell.SortGlyphDirection =
                    ascending ? SortOrder.Ascending : SortOrder.Descending;
            }
        }

        // ====== SẮP XẾP GRID THEO NHÂN VIÊN (MỌI CỘT) ======
        private void dgvNhanVien_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_currentTheoNhanVien == null || _currentTheoNhanVien.Count == 0) return;

            var column = dgvNhanVien.Columns[e.ColumnIndex];
            var prop = column.DataPropertyName;
            if (string.IsNullOrEmpty(prop)) return;

            bool ascending = column.HeaderCell.SortGlyphDirection != SortOrder.Ascending;
            string columnName = column.Name;

            _currentTheoNhanVien = SortList(_currentTheoNhanVien, prop, ascending);

            // Rebind + giữ format
            BindNhanVien(_currentTheoNhanVien);

            foreach (DataGridViewColumn col in dgvNhanVien.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;

            var currentColumn = dgvNhanVien.Columns[columnName];
            if (currentColumn != null)
            {
                currentColumn.HeaderCell.SortGlyphDirection =
                    ascending ? SortOrder.Ascending : SortOrder.Descending;
            }
        }

        /// <summary>
        /// Format cột thời gian trong grid Doanh thu tổng:
        /// - Theo ngày: dd/MM/yyyy
        /// - Theo tháng: MM/yyyy (nếu 0001 thì lấy từ bộ lọc)
        /// - Theo năm: yyyy (nếu 0001 thì lấy từ bộ lọc)
        /// </summary>
        private void dgvTongQuan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTongQuan.Columns[e.ColumnIndex].DataPropertyName != nameof(ThongKeDoanhThuDto.Ngay))
                return;

            if (e.Value is not DateTime dt)
                return;

            switch (_currentKieuThongKe)
            {
                case "ngay":
                    // dd/MM/yyyy
                    e.Value = dt.ToString("dd/MM/yyyy");
                    e.FormattingApplied = true;
                    break;

                case "thang":
                    if (dt.Year == 1)
                    {
                        if (dtpFrom.Value.Year == dtpTo.Value.Year &&
                            dtpFrom.Value.Month == dtpTo.Value.Month)
                        {
                            e.Value = dtpFrom.Value.ToString("MM/yyyy");
                        }
                        else
                        {
                            e.Value = $"{dtpFrom.Value:MM/yyyy} - {dtpTo.Value:MM/yyyy}";
                        }
                    }
                    else
                    {
                        e.Value = dt.ToString("MM/yyyy");
                    }
                    e.FormattingApplied = true;
                    break;

                case "nam":
                    if (dt.Year == 1)
                    {
                        if (dtpFrom.Value.Year == dtpTo.Value.Year)
                        {
                            e.Value = dtpFrom.Value.ToString("yyyy");
                        }
                        else
                        {
                            e.Value = $"{dtpFrom.Value:yyyy} - {dtpTo.Value:yyyy}";
                        }
                    }
                    else
                    {
                        e.Value = dt.ToString("yyyy");
                    }
                    e.FormattingApplied = true;
                    break;
            }
        }
    }
}
