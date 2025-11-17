using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        /// Thống kê doanh thu theo ngày / tháng / năm
        /// GET /api/baocao/doanh-thu?from=2025-11-01&to=2025-11-30&kieu=ngay|thang|nam
        /// </summary>
        [HttpGet("doanh-thu")]
        public async Task<ActionResult<IEnumerable<ThongKeDoanhThuDto>>> GetDoanhThu(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] string? kieu = "ngay")
        {
            // Mặc định: 30 ngày gần nhất
            var now = DateTime.Now;
            var tuNgay = (from ?? now.Date.AddDays(-30)).Date;
            var denNgay = (to ?? now.Date).Date.AddDays(1).AddTicks(-1);

            // Lấy các hoá đơn đã thanh toán trong khoảng thời gian
            var baseQuery = _context.HoaDons
                .Where(h => h.CreatedAt >= tuNgay &&
                            h.CreatedAt <= denNgay &&
                            h.TrangThai == 1); // 1 = Paid

            kieu = (kieu ?? "ngay").ToLowerInvariant();

            IQueryable<ThongKeDoanhThuDto> query;

            switch (kieu)
            {
                // THỐNG KÊ THEO THÁNG
                case "thang":
                    query = baseQuery
                        .GroupBy(h => new { h.CreatedAt.Year, h.CreatedAt.Month })
                        .OrderBy(g => g.Key.Year)
                        .ThenBy(g => g.Key.Month)
                        .Select(g => new ThongKeDoanhThuDto
                        {
                            // Dùng ngày mùng 1 đại diện cho tháng
                            Ngay = new DateTime(g.Key.Year, g.Key.Month, 1),
                            SoHoaDon = g.Count(),
                            TongTien = g.Sum(x => x.TongTien),
                            TongGiamGia = g.Sum(x => x.GiamGia),
                            TongThue = g.Sum(x => x.Thue),
                            DoanhThuThucTe = g.Sum(x => x.TongTien - x.GiamGia + x.Thue)
                        });
                    break;

                // THỐNG KÊ THEO NĂM
                case "nam":
                    query = baseQuery
                        .GroupBy(h => h.CreatedAt.Year)
                        .OrderBy(g => g.Key)
                        .Select(g => new ThongKeDoanhThuDto
                        {
                            // Dùng 1/1 đại diện cho năm
                            Ngay = new DateTime(g.Key, 1, 1),
                            SoHoaDon = g.Count(),
                            TongTien = g.Sum(x => x.TongTien),
                            TongGiamGia = g.Sum(x => x.GiamGia),
                            TongThue = g.Sum(x => x.Thue),
                            DoanhThuThucTe = g.Sum(x => x.TongTien - x.GiamGia + x.Thue)
                        });
                    break;

                // MẶC ĐỊNH: THEO NGÀY
                default:
                    query = baseQuery
                        .GroupBy(h => h.CreatedAt.Date)
                        .OrderBy(g => g.Key)
                        .Select(g => new ThongKeDoanhThuDto
                        {
                            Ngay = g.Key,
                            SoHoaDon = g.Count(),
                            TongTien = g.Sum(x => x.TongTien),
                            TongGiamGia = g.Sum(x => x.GiamGia),
                            TongThue = g.Sum(x => x.Thue),
                            DoanhThuThucTe = g.Sum(x => x.TongTien - x.GiamGia + x.Thue)
                        });
                    break;
            }

            var result = await query.ToListAsync();
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
            var baseQuery = _context.HoaDons
                .Include(h => h.NhanVien)
                .Where(h => h.CreatedAt >= tuNgay &&
                            h.CreatedAt <= denNgay &&
                            h.TrangThai == 1); // 1 = Paid

            if (!string.IsNullOrWhiteSpace(maNhanVien))
            {
                baseQuery = baseQuery.Where(h => h.NhanVien.MaNhanVien == maNhanVien);
            }

            var data = await baseQuery
                .GroupBy(h => new
                {
                    h.NhanVienId,
                    h.NhanVien.MaNhanVien,
                    h.NhanVien.HoTen
                })
                .Select(g => new ThongKeDoanhThuNhanVienDto
                {
                    NhanVienId = g.Key.NhanVienId,
                    MaNhanVien = g.Key.MaNhanVien,
                    HoTen = g.Key.HoTen,

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
