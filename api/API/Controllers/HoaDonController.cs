using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetAll()
        {
            return await _context.HoaDons.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<HoaDon>> GetById(int id)
        {
            var item = await _context.HoaDons.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<HoaDon>> Create(
            HoaDonCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var item = new HoaDon
            {
                MaHoaDon = dto.MaHoaDon,
                NhanVienId = dto.NhanVienId,
                BanId = dto.BanId,
                CreatedAt = dto.CreatedAt == default ? DateTime.Now : dto.CreatedAt,
                ClosedAt = dto.ClosedAt,
                TrangThai = dto.TrangThai,
                TongTien = dto.TongTien,
                GiamGia = dto.GiamGia,
                Thue = dto.Thue,
                GhiChu = dto.GhiChu
            };

            _context.HoaDons.Add(item);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(HoaDon),
                IdBanGhi = item.Id,
                HanhDong = "Tao",
                GiaTriMoi = JsonSerializer.Serialize(item),
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            HoaDonCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var item = await _context.HoaDons.FindAsync(id);
            if (item == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(item);

            item.MaHoaDon = dto.MaHoaDon;
            item.NhanVienId = dto.NhanVienId;
            item.BanId = dto.BanId;
            item.CreatedAt = dto.CreatedAt;
            item.ClosedAt = dto.ClosedAt;
            item.TrangThai = dto.TrangThai;
            item.TongTien = dto.TongTien;
            item.GiamGia = dto.GiamGia;
            item.Thue = dto.Thue;
            item.GhiChu = dto.GhiChu;

            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";
            var giaTriMoi = JsonSerializer.Serialize(item);

            var log = new AuditLog
            {
                TenBang = nameof(HoaDon),
                IdBanGhi = item.Id,
                HanhDong = "CapNhat",
                GiaTriCu = giaTriCu,
                GiaTriMoi = giaTriMoi,
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("{id:int}/thanh-toan")]
        public async Task<IActionResult> ThanhToanHoaDon(
    int id,
    HoaDonThanhToanDto dto,
    [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var hd = await _context.HoaDons.FindAsync(id);
            if (hd == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(hd);

            // cập nhật trạng thái hoá đơn
            hd.TongTien = dto.TongTien;
            hd.GiamGia = dto.GiamGia;
            hd.Thue = dto.Thue;
            hd.GhiChu = dto.GhiChu;
            hd.TrangThai = 1;             // 1 = Paid
            hd.ClosedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";
            var giaTriMoi = JsonSerializer.Serialize(hd);

            // ghi AuditLog giống các action khác 
            var log = new AuditLog
            {
                TenBang = nameof(HoaDon),
                IdBanGhi = hd.Id,
                HanhDong = "ThanhToan",
                GiaTriCu = giaTriCu,
                GiaTriMoi = giaTriMoi,
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetByDateRange(
    [FromQuery] DateTime? from,
    [FromQuery] DateTime? to)
        {
            var query = _context.HoaDons.AsQueryable();

            if (from.HasValue)
                query = query.Where(h => h.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(h => h.CreatedAt <= to.Value);

            var data = await query
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

            return Ok(data);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var item = await _context.HoaDons.FindAsync(id);
            if (item == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(item);

            _context.HoaDons.Remove(item);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(HoaDon),
                IdBanGhi = id,
                HanhDong = "Xoa",
                GiaTriCu = giaTriCu,
                GiaTriMoi = null,
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("{id:int}/chi-tiet")]
        public async Task<ActionResult<IEnumerable<ChiTietHoaDonDto>>> GetChiTiet(int id)
        {
            // id ở đây là DonGoiId (đơn gọi ứng với hoá đơn)
            var data = await _context.ChiTietDonGois
                .Where(c => c.DonGoiId == id)
                .Select(c => new ChiTietHoaDonDto
                {
                    Id = c.Id,

                    // Lấy tên món từ ThucUong hoặc ThucAn
                    TenMon = c.ThucUongId != null && c.ThucUong != null
                                ? c.ThucUong.Ten        // <-- dùng Ten
                                : c.ThucAnId != null && c.ThucAn != null
                                    ? c.ThucAn.Ten      // <-- dùng Ten
                                    : "Món khác",

                    // Phân loại để sau này WinForms hiển thị
                    LoaiMon = c.ThucUongId != null ? "Thức uống"
                             : c.ThucAnId != null ? "Thức ăn"
                             : "Khác",

                    SoLuong = c.SoLuong,
                    DonGia = c.DonGia,
                    ChietKhau = c.ChietKhau,
                    ThanhTien = c.SoLuong * c.DonGia - c.ChietKhau
                })
                .ToListAsync();

            return Ok(data);
        }
    }

}
