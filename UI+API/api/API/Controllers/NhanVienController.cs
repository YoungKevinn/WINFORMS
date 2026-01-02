using API.DTOs;
using API.Models;
using API.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public NhanVienController(CafeDbContext context)
        {
            _context = context;
        }

        private string Actor()
        {
            // Ưu tiên lấy theo claim MaNhanVien, fallback Name/Id
            return User.FindFirst("MaNhanVien")?.Value
                ?? User.Identity?.Name
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? "Unknown";
        }

        private static NhanVienViewDto ToViewDto(NhanVien nv) => new NhanVienViewDto
        {
            Id = nv.Id,
            MaNhanVien = nv.MaNhanVien,
            HoTen = nv.HoTen,
            VaiTro = nv.VaiTro,
            TenDangNhap = nv.TenDangNhap,
            TrangThai = nv.TrangThai
        };

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<NhanVienViewDto>>> GetAll()
        {
            var data = await _context.NhanViens
                .Select(nv => new NhanVienViewDto
                {
                    Id = nv.Id,
                    MaNhanVien = nv.MaNhanVien,
                    HoTen = nv.HoTen,
                    VaiTro = nv.VaiTro,
                    TenDangNhap = nv.TenDangNhap,
                    TrangThai = nv.TrangThai
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<NhanVienViewDto>> GetById(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();
            return Ok(ToViewDto(nv));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<NhanVienViewDto>> Create(NhanVienCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaNhanVien))
                return BadRequest("Mã nhân viên là bắt buộc.");

            var ma = dto.MaNhanVien.Trim();

            var existed = await _context.NhanViens.AnyAsync(x => x.MaNhanVien == ma);
            if (existed)
                return BadRequest("Mã nhân viên đã tồn tại.");

            var nv = new NhanVien
            {
                MaNhanVien = ma,
                HoTen = dto.HoTen.Trim(),
                VaiTro = string.IsNullOrWhiteSpace(dto.VaiTro) ? "NhanVien" : dto.VaiTro.Trim(),
                TenDangNhap = ma, // login = MaNV
                MatKhauHash = PasswordHasher.CreatePasswordHash("123456"),

                TrangThai = 0,
                FailedLoginCount = 0,
                LockoutEndUtc = null
            };

            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();

            _context.AuditLogs.Add(new AuditLog
            {
                TenBang = nameof(NhanVien),
                IdBanGhi = nv.Id,
                HanhDong = "Tao",
                GiaTriCu = null,
                GiaTriMoi = JsonSerializer.Serialize(ToViewDto(nv)),
                NguoiThucHien = Actor(),
                ThoiGian = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = nv.Id }, ToViewDto(nv));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, NhanVienCreateUpdateDto dto)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(ToViewDto(nv));

            if (!string.IsNullOrWhiteSpace(dto.MaNhanVien))
            {
                var ma = dto.MaNhanVien.Trim();

                if (!string.Equals(nv.MaNhanVien, ma, StringComparison.OrdinalIgnoreCase))
                {
                    var existed = await _context.NhanViens.AnyAsync(x => x.MaNhanVien == ma && x.Id != id);
                    if (existed)
                        return BadRequest("Mã nhân viên đã tồn tại.");

                    nv.MaNhanVien = ma;
                    nv.TenDangNhap = ma; // đồng bộ username = MaNV
                }
            }

            nv.HoTen = dto.HoTen.Trim();
            nv.VaiTro = string.IsNullOrWhiteSpace(dto.VaiTro) ? nv.VaiTro : dto.VaiTro.Trim();

            await _context.SaveChangesAsync();

            _context.AuditLogs.Add(new AuditLog
            {
                TenBang = nameof(NhanVien),
                IdBanGhi = id,
                HanhDong = "Sua",
                GiaTriCu = giaTriCu,
                GiaTriMoi = JsonSerializer.Serialize(ToViewDto(nv)),
                NguoiThucHien = Actor(),
                ThoiGian = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetStatus(int id, NhanVienStatusDto dto)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();

            if (nv.VaiTro == "Admin" && dto.TrangThai == 2)
                return BadRequest("Không cho phép khóa tài khoản Admin.");

            var giaTriCu = JsonSerializer.Serialize(ToViewDto(nv));

            nv.TrangThai = dto.TrangThai;

            if (dto.TrangThai == 2)
            {
                nv.FailedLoginCount = 0;
                nv.LockoutEndUtc = null;
            }

            await _context.SaveChangesAsync();

            _context.AuditLogs.Add(new AuditLog
            {
                TenBang = nameof(NhanVien),
                IdBanGhi = id,
                HanhDong = "DoiTrangThai",
                GiaTriCu = giaTriCu,
                GiaTriMoi = JsonSerializer.Serialize(ToViewDto(nv)),
                NguoiThucHien = Actor(),
                ThoiGian = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật trạng thái thành công." });
        }

        [HttpPut("{id:int}/set-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetPassword(int id, SetPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MatKhauMoi))
                return BadRequest("Mật khẩu mới là bắt buộc.");

            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(ToViewDto(nv));

            nv.MatKhauHash = PasswordHasher.CreatePasswordHash(dto.MatKhauMoi.Trim());

            nv.FailedLoginCount = 0;
            nv.LockoutEndUtc = null;

            await _context.SaveChangesAsync();

            _context.AuditLogs.Add(new AuditLog
            {
                TenBang = nameof(NhanVien),
                IdBanGhi = id,
                HanhDong = "DoiMatKhau",
                GiaTriCu = giaTriCu,
                GiaTriMoi = null,
                NguoiThucHien = Actor(),
                ThoiGian = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đặt lại mật khẩu thành công." });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();

            if (nv.VaiTro == "Admin")
                return BadRequest("Không cho phép xóa/khóa tài khoản Admin.");

            var giaTriCu = JsonSerializer.Serialize(ToViewDto(nv));

            nv.TrangThai = 2;
            nv.FailedLoginCount = 0;
            nv.LockoutEndUtc = null;

            await _context.SaveChangesAsync();

            _context.AuditLogs.Add(new AuditLog
            {
                TenBang = nameof(NhanVien),
                IdBanGhi = id,
                HanhDong = "Khoa",
                GiaTriCu = giaTriCu,
                GiaTriMoi = JsonSerializer.Serialize(ToViewDto(nv)),
                NguoiThucHien = Actor(),
                ThoiGian = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
