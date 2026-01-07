using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Models;
using API.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CafeDbContext _context;
        private readonly IConfiguration _config;

        // Gộp failed logs trong X phút
        private const int FailAggWindowMinutes = 10;

        public AuthController(CafeDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string GetClientIp()
        {
            return HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        private AuditLog NewAuthLog(string hanhDong, int? idBanGhi, string? nguoiThucHien, object? giaTriMoi, object? giaTriCu = null)
        {
            return new AuditLog
            {
                TenBang = "Auth",
                IdBanGhi = idBanGhi,
                HanhDong = hanhDong,
                GiaTriCu = giaTriCu == null ? null : JsonSerializer.Serialize(giaTriCu),
                GiaTriMoi = giaTriMoi == null ? null : JsonSerializer.Serialize(giaTriMoi),
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };
        }

        private async Task TryWriteAuditAsync(AuditLog log)
        {
            try
            {
                _context.AuditLogs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Không để audit làm hỏng luồng auth
            }
        }

        private class FailAggPayload
        {
            public string? TenDangNhap { get; set; }
            public string? Reason { get; set; }
            public string? Ip { get; set; }
            public int Count { get; set; }
            public DateTime FirstAt { get; set; }
            public DateTime LastAt { get; set; }
        }

        /// <summary>
        /// Gộp các log FAILED cùng loại trong 10 phút gần nhất theo NguoiThucHien.
        /// Nếu có log gần đây -> update Count/LastAt/Reason/Ip thay vì insert mới.
        /// </summary>
        private async Task TryWriteAuthFailAggregateAsync(
            string hanhDongFailed,
            int? idBanGhi,
            string? nguoiThucHienKey,
            string tenDangNhapInput,
            string reason,
            string ip)
        {
            var key = string.IsNullOrWhiteSpace(nguoiThucHienKey) ? tenDangNhapInput : nguoiThucHienKey;
            if (string.IsNullOrWhiteSpace(key)) key = "Unknown";

            try
            {
                var from = DateTime.Now.AddMinutes(-FailAggWindowMinutes);

                var latest = await _context.AuditLogs
                    .Where(l =>
                        l.TenBang == "Auth" &&
                        l.HanhDong == hanhDongFailed &&
                        l.NguoiThucHien == key &&
                        l.ThoiGian >= from)
                    .OrderByDescending(l => l.ThoiGian)
                    .FirstOrDefaultAsync();

                if (latest == null)
                {
                    var payload = new FailAggPayload
                    {
                        TenDangNhap = tenDangNhapInput,
                        Reason = reason,
                        Ip = ip,
                        Count = 1,
                        FirstAt = DateTime.Now,
                        LastAt = DateTime.Now
                    };

                    _context.AuditLogs.Add(NewAuthLog(
                        hanhDongFailed,
                        idBanGhi,
                        key,
                        payload
                    ));

                    await _context.SaveChangesAsync();
                    return;
                }

                FailAggPayload existing;
                try
                {
                    existing = string.IsNullOrWhiteSpace(latest.GiaTriMoi)
                        ? new FailAggPayload()
                        : (JsonSerializer.Deserialize<FailAggPayload>(latest.GiaTriMoi) ?? new FailAggPayload());
                }
                catch
                {
                    existing = new FailAggPayload();
                }

                if (existing.FirstAt == default) existing.FirstAt = latest.ThoiGian;
                if (existing.Count <= 0) existing.Count = 0;

                existing.Count += 1;
                existing.LastAt = DateTime.Now;
                existing.Reason = reason;
                existing.Ip = ip;
                existing.TenDangNhap = tenDangNhapInput;

                latest.IdBanGhi = idBanGhi;
                latest.GiaTriMoi = JsonSerializer.Serialize(existing);
                latest.ThoiGian = DateTime.Now; // cập nhật timestamp để kéo dài window gộp

                await _context.SaveChangesAsync();
            }
            catch
            {
                // fail-safe: nếu gộp lỗi thì bỏ qua để không ảnh hưởng auth
            }
        }

        // Tạo admin mới (dùng lúc setup, sau này có thể tắt endpoint này đi)
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegisterDto dto)
        {
            var existed = await _context.NhanViens.AnyAsync(x => x.TenDangNhap == dto.TenDangNhap);
            if (existed) return BadRequest("Tên đăng nhập đã tồn tại.");

            var hash = PasswordHasher.CreatePasswordHash(dto.MatKhau);

            var nv = new NhanVien
            {
                MaNhanVien = dto.MaNhanVien,
                HoTen = dto.HoTen,
                VaiTro = "Admin",
                TenDangNhap = dto.TenDangNhap,
                MatKhauHash = hash
            };

            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();

            await TryWriteAuditAsync(NewAuthLog(
                "RegisterAdmin",
                nv.Id,
                dto.TenDangNhap,
                new { dto.TenDangNhap, dto.HoTen, dto.MaNhanVien, VaiTro = "Admin", Ip = GetClientIp() }
            ));

            return Ok(new { message = "Tạo admin thành công", nv.Id, nv.HoTen });
        }

        // Đăng nhập admin -> trả JWT
        [HttpPost("admin-login")]
        public async Task<ActionResult<AuthResponseDto>> AdminLogin(AdminLoginDto dto)
        {
            var ip = GetClientIp();

            var user = await _context.NhanViens.FirstOrDefaultAsync(x =>
                x.TenDangNhap == dto.TenDangNhap && x.VaiTro == "Admin");

            if (user == null || string.IsNullOrEmpty(user.MatKhauHash))
            {
                await TryWriteAuthFailAggregateAsync(
                    "AdminLoginFailed",
                    user?.Id,
                    dto.TenDangNhap,
                    dto.TenDangNhap,
                    "UserNotFoundOrNotAdmin",
                    ip
                );
                return Unauthorized("Sai tài khoản hoặc không có quyền admin.");
            }

            var ok = PasswordHasher.VerifyPassword(dto.MatKhau, user.MatKhauHash);
            if (!ok)
            {
                await TryWriteAuthFailAggregateAsync(
                    "AdminLoginFailed",
                    user.Id,
                    user.TenDangNhap,
                    dto.TenDangNhap,
                    "WrongPassword",
                    ip
                );
                return Unauthorized("Sai mật khẩu.");
            }

            var token = GenerateJwtToken(user);

            await TryWriteAuditAsync(NewAuthLog(
                "AdminLoginSuccess",
                user.Id,
                user.TenDangNhap,
                new { user.TenDangNhap, user.HoTen, user.MaNhanVien, user.VaiTro, Ip = ip }
            ));

            return Ok(token);
        }

        // Đăng nhập nhân viên -> trả JWT (cho phép nhập MaNhanVien hoặc TenDangNhap)
        [HttpPost("employee-login")]
        public async Task<ActionResult<AuthResponseDto>> EmployeeLogin(AdminLoginDto dto)
        {
            var ip = GetClientIp();

            var user = await _context.NhanViens.FirstOrDefaultAsync(x =>
                (x.TenDangNhap == dto.TenDangNhap || x.MaNhanVien == dto.TenDangNhap) &&
                x.VaiTro != "Admin");

            if (user == null || string.IsNullOrEmpty(user.MatKhauHash))
            {
                await TryWriteAuthFailAggregateAsync(
                    "EmployeeLoginFailed",
                    user?.Id,
                    dto.TenDangNhap,
                    dto.TenDangNhap,
                    "UserNotFoundOrNotEmployee",
                    ip
                );
                return Unauthorized("Sai tài khoản hoặc không có quyền nhân viên.");
            }

            var ok = PasswordHasher.VerifyPassword(dto.MatKhau, user.MatKhauHash);
            if (!ok)
            {
                await TryWriteAuthFailAggregateAsync(
                    "EmployeeLoginFailed",
                    user.Id,
                    user.TenDangNhap ?? user.MaNhanVien,
                    dto.TenDangNhap,
                    "WrongPassword",
                    ip
                );
                return Unauthorized("Sai mật khẩu.");
            }

            var token = GenerateJwtToken(user);

            
            var isDefaultPassword = dto.MatKhau == "123456" && PasswordHasher.VerifyPassword("123456", user.MatKhauHash);
            token.MustChangePassword = isDefaultPassword;

            await TryWriteAuditAsync(NewAuthLog(
                "EmployeeLoginSuccess",
                user.Id,
                user.TenDangNhap ?? user.MaNhanVien,
                new { user.TenDangNhap, user.MaNhanVien, user.HoTen, user.VaiTro, Ip = ip }
            ));

            return Ok(token);
        }

        // Người dùng đã đăng nhập mới đổi được mật khẩu
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var ip = GetClientIp();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                             ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
            {
                await TryWriteAuthFailAggregateAsync(
                    "ChangePasswordFailed",
                    null,
                    "Unknown",
                    "Unknown",
                    "MissingUserIdClaim",
                    ip
                );
                return Unauthorized("Không xác định được người dùng từ token.");
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                await TryWriteAuthFailAggregateAsync(
                    "ChangePasswordFailed",
                    null,
                    "Unknown",
                    "Unknown",
                    "InvalidUserIdClaim",
                    ip
                );
                return Unauthorized("Token không hợp lệ.");
            }

            var user = await _context.NhanViens.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null || string.IsNullOrEmpty(user.MatKhauHash))
            {
                await TryWriteAuthFailAggregateAsync(
                    "ChangePasswordFailed",
                    userId,
                    "Unknown",
                    "Unknown",
                    "UserNotFound",
                    ip
                );
                return Unauthorized("Không tìm thấy tài khoản.");
            }

            var oldOk = PasswordHasher.VerifyPassword(dto.MatKhauCu, user.MatKhauHash);
            if (!oldOk)
            {
                await TryWriteAuthFailAggregateAsync(
                    "ChangePasswordFailed",
                    user.Id,
                    user.TenDangNhap ?? user.MaNhanVien,
                    user.TenDangNhap ?? user.MaNhanVien ?? "Unknown",
                    "OldPasswordMismatch",
                    ip
                );
                return BadRequest("Mật khẩu cũ không đúng.");
            }

            user.MatKhauHash = PasswordHasher.CreatePasswordHash(dto.MatKhauMoi);

            // Log success (không gộp)
            _context.AuditLogs.Add(NewAuthLog(
                "ChangePasswordSuccess",
                user.Id,
                user.TenDangNhap ?? user.MaNhanVien,
                new { user.Id, user.TenDangNhap, user.MaNhanVien, Ip = ip }
            ));

            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công. Vui lòng đăng nhập lại." });
        }

        private AuthResponseDto GenerateJwtToken(NhanVien user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpireMinutes"]!));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.HoTen),
                new Claim(ClaimTypes.Name, user.HoTen),
                new Claim(ClaimTypes.Role, user.VaiTro),
                new Claim("MaNhanVien", user.MaNhanVien ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto
            {
                Token = tokenString,
                ExpiresAt = expires,
                HoTen = user.HoTen,
                VaiTro = user.VaiTro
            };
        }
    }
}
