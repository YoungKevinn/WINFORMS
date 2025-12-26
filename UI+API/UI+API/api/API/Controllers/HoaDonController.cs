using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public HoaDonController(CafeDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tra cứu danh sách hóa đơn theo khoảng thời gian.
        /// GET /api/hoadon?from=2025-11-01&to=2025-11-16
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDonDto>>> GetList(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            // Khoảng mặc định: 7 ngày gần nhất
            var toDate = (to ?? DateTime.Today).Date;
            var fromDate = (from ?? toDate.AddDays(-7)).Date;

            var fromTime = fromDate;
            var toTime = toDate.AddDays(1).AddTicks(-1); // hết ngày "to"

            var query = _context.HoaDons
                .Include(h => h.NhanVien)
                .Include(h => h.Ban)
                .Where(h => h.CreatedAt >= fromTime && h.CreatedAt <= toTime)
                .OrderByDescending(h => h.CreatedAt);

            var data = await query
                .Select(h => new HoaDonDto
                {
                    Id = h.Id,
                    MaHoaDon = h.MaHoaDon ?? string.Empty,
                    ThoiGian = h.CreatedAt,

                    // 4 trường tiền
                    TongTien = h.TongTien,
                    GiamGia = h.GiamGia,
                    Thue = h.Thue,
                    ThanhTien = h.TongTien - h.GiamGia + h.Thue,

                    TenNhanVien = h.NhanVien != null ? h.NhanVien.HoTen : null,
                    TenBan = h.Ban != null ? h.Ban.TenBan : null
                })
                .ToListAsync();

            return Ok(data);
        }

        /// <summary>
        /// Lấy chi tiết hóa đơn (quy ước: Id hóa đơn == Id đơn gọi).
        /// GET /api/hoadon/{id}/chi-tiet
        /// </summary>
        [HttpGet("{id:int}/chi-tiet")]
        public async Task<ActionResult<IEnumerable<ChiTietHoaDonDto>>> GetChiTiet(int id)
        {
            // Chi tiết nằm ở bảng ChiTietDonGoi, join với ThucUong / ThucAn để lấy tên món
            var query = _context.ChiTietDonGois
                .Include(c => c.ThucUong)
                .Include(c => c.ThucAn)
                .Where(c => c.DonGoiId == id);

            var data = await query
                .Select(c => new ChiTietHoaDonDto
                {
                    Id = c.Id,
                    TenMon = c.ThucUongId != null && c.ThucUong != null
                        ? c.ThucUong.Ten
                        : (c.ThucAn != null ? c.ThucAn.Ten : string.Empty),
                    SoLuong = c.SoLuong,
                    DonGia = c.DonGia,
                    ChietKhau = c.ChietKhau,
                    ThanhTien = c.SoLuong * (c.DonGia - c.ChietKhau),
                    GhiChu = c.GhiChu
                })
                .ToListAsync();

            return Ok(data);
        }

        /// <summary>
        /// Tạo hóa đơn mới (nếu phía client cần).
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<HoaDonDto>> Create([FromBody] HoaDonCreateUpdateDto dto)
        {
            var entity = new HoaDon
            {
                MaHoaDon = dto.MaHoaDon,
                NhanVienId = dto.NhanVienId,
                BanId = dto.BanId ?? 0,
                CreatedAt = dto.CreatedAt,
                ClosedAt = dto.ClosedAt,
                TrangThai = dto.TrangThai,
                TongTien = dto.TongTien,
                GiamGia = dto.GiamGia,
                Thue = dto.Thue,
                GhiChu = dto.GhiChu
            };

            _context.HoaDons.Add(entity);
            await _context.SaveChangesAsync();

            // nạp navigation để lấy tên NV / tên bàn
            await _context.Entry(entity).Reference(h => h.NhanVien).LoadAsync();
            await _context.Entry(entity).Reference(h => h.Ban).LoadAsync();

            var result = new HoaDonDto
            {
                Id = entity.Id,
                MaHoaDon = entity.MaHoaDon ?? string.Empty,
                ThoiGian = entity.CreatedAt,

                TongTien = entity.TongTien,
                GiamGia = entity.GiamGia,
                Thue = entity.Thue,
                ThanhTien = entity.TongTien - entity.GiamGia + entity.Thue,

                TenNhanVien = entity.NhanVien?.HoTen,
                TenBan = entity.Ban?.TenBan
            };

            return CreatedAtAction(nameof(GetList), new { id = entity.Id }, result);
        }

        /// <summary>
        /// Cập nhật hóa đơn.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] HoaDonCreateUpdateDto dto)
        {
            var entity = await _context.HoaDons.FindAsync(id);
            if (entity == null)
                return NotFound();

            entity.MaHoaDon = dto.MaHoaDon;
            entity.NhanVienId = dto.NhanVienId;
            entity.BanId = dto.BanId ?? entity.BanId;
            entity.CreatedAt = dto.CreatedAt;
            entity.ClosedAt = dto.ClosedAt;
            entity.TrangThai = dto.TrangThai;
            entity.TongTien = dto.TongTien;
            entity.GiamGia = dto.GiamGia;
            entity.Thue = dto.Thue;
            entity.GhiChu = dto.GhiChu;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Xoá hóa đơn.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.HoaDons.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.HoaDons.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
