using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaoCaoController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public BaoCaoController(CafeDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Thống kê doanh thu theo ngày / tuần / tháng
        /// GET /api/baocao/doanh-thu?from=2025-11-01&to=2025-11-30&kieu=ngay|tuan|thang
        /// </summary>
        [HttpGet("doanh-thu")]
        public async Task<ActionResult<IEnumerable<ThongKeDoanhThuDto>>> GetDoanhThu(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] string? kieu = "ngay")
        {
            // mặc định: 30 ngày gần nhất
            var now = DateTime.Now;
            var tuNgay = (from ?? now.Date.AddDays(-30)).Date;
            var denNgay = (to ?? now.Date).Date.AddDays(1).AddTicks(-1);

            // Lấy các hoá đơn đã thanh toán trong khoảng thời gian
            var list = await _context.HoaDons
                .Where(h => h.CreatedAt >= tuNgay &&
                            h.CreatedAt <= denNgay &&
                            h.TrangThai == 1) // 1 = Paid
                .ToListAsync();

            // chuyển về lower để so sánh
            kieu = (kieu ?? "ngay").ToLowerInvariant();

            IEnumerable<ThongKeDoanhThuDto> result;

            switch (kieu)
            {
                // THỐNG KÊ THEO THÁNG
                case "thang":
                    result = list
                        .GroupBy(h => new { h.CreatedAt.Year, h.CreatedAt.Month })
                        .OrderBy(g => g.Key.Year)
                        .ThenBy(g => g.Key.Month)
                        .Select(g => new ThongKeDoanhThuDto
                        {
                            Nhan = $"{g.Key.Month:00}/{g.Key.Year}",
                            SoHoaDon = g.Count(),
                            TongTien = g.Sum(x => x.TongTien),
                            TongGiamGia = g.Sum(x => x.GiamGia),
                            TongThue = g.Sum(x => x.Thue),
                            DoanhThuThucTe = g.Sum(x => x.TongTien - x.GiamGia + x.Thue)
                        });
                    break;

                // THỐNG KÊ THEO TUẦN
                case "tuan":
                    var ci = CultureInfo.CurrentCulture;
                    result = list
                        .GroupBy(h => new
                        {
                            h.CreatedAt.Year,
                            Week = ci.Calendar.GetWeekOfYear(
                                h.CreatedAt,
                                CalendarWeekRule.FirstFourDayWeek,
                                DayOfWeek.Monday)
                        })
                        .OrderBy(g => g.Key.Year)
                        .ThenBy(g => g.Key.Week)
                        .Select(g => new ThongKeDoanhThuDto
                        {
                            Nhan = $"Tuần {g.Key.Week} - {g.Key.Year}",
                            SoHoaDon = g.Count(),
                            TongTien = g.Sum(x => x.TongTien),
                            TongGiamGia = g.Sum(x => x.GiamGia),
                            TongThue = g.Sum(x => x.Thue),
                            DoanhThuThucTe = g.Sum(x => x.TongTien - x.GiamGia + x.Thue)
                        });
                    break;

                // MẶC ĐỊNH: THEO NGÀY
                default:
                    result = list
                        .GroupBy(h => h.CreatedAt.Date)
                        .OrderBy(g => g.Key)
                        .Select(g => new ThongKeDoanhThuDto
                        {
                            Nhan = g.Key.ToString("dd/MM/yyyy"),
                            SoHoaDon = g.Count(),
                            TongTien = g.Sum(x => x.TongTien),
                            TongGiamGia = g.Sum(x => x.GiamGia),
                            TongThue = g.Sum(x => x.Thue),
                            DoanhThuThucTe = g.Sum(x => x.TongTien - x.GiamGia + x.Thue)
                        });
                    break;
            }

            return Ok(result);
        }

        /// <summary>
        /// Thống kê doanh thu theo nhân viên
        /// GET /api/baocao/doanh-thu-nhan-vien?from=2025-11-01&to=2025-11-30&maNhanVien=NV001 (optional)
        /// </summary>
        [HttpGet("doanh-thu-nhan-vien")]
        public async Task<ActionResult<IEnumerable<ThongKeDoanhThuNhanVienDto>>> GetDoanhThuNhanVien(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] string? maNhanVien)
        {
            var now = DateTime.Now;
            var tuNgay = (from ?? now.Date.AddDays(-30)).Date;
            var denNgay = (to ?? now.Date).Date.AddDays(1).AddTicks(-1);

            // Lấy các hóa đơn đã thanh toán + include nhân viên
            var query = _context.HoaDons
                .Include(h => h.NhanVien)
                .Where(h => h.CreatedAt >= tuNgay &&
                            h.CreatedAt <= denNgay &&
                            h.TrangThai == 1); // 1 = Paid

            if (!string.IsNullOrWhiteSpace(maNhanVien))
            {
                query = query.Where(h => h.NhanVien.MaNhanVien == maNhanVien);
            }

            var data = await query
                .GroupBy(h => new { h.NhanVien.MaNhanVien, h.NhanVien.HoTen })
                .Select(g => new ThongKeDoanhThuNhanVienDto
                {
                    MaNhanVien = g.Key.MaNhanVien,
                    TenNhanVien = g.Key.HoTen,

                    SoHoaDon = g.Count(),
                    TongTien = g.Sum(x => x.TongTien),
                    TongGiamGia = g.Sum(x => x.GiamGia),
                    TongThue = g.Sum(x => x.Thue),
                    DoanhThuThucTe = g.Sum(x => x.TongTien - x.GiamGia + x.Thue)
                })
                .OrderByDescending(x => x.DoanhThuThucTe)
                .ToListAsync();

            return Ok(data);
        }
    }
}
