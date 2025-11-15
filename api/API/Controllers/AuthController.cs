using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public AuthController(CafeDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Tạo admin mới (dùng lúc setup, sau này có thể tắt endpoint này đi)
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegisterDto dto)
        {
            // check trùng username
            var existed = await _context.NhanViens
                .AnyAsync(x => x.TenDangNhap == dto.TenDangNhap);

            if (existed)
                return BadRequest("Tên đăng nhập đã tồn tại.");

            // Hash mật khẩu bằng MD5("hehe" + password)
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

            return Ok(new { message = "Tạo admin thành công", nv.Id, nv.HoTen });
        }

        // Đăng nhập admin -> trả về JWT
        [HttpPost("admin-login")]
        public async Task<ActionResult<AuthResponseDto>> AdminLogin(AdminLoginDto dto)
        {
            var user = await _context.NhanViens
                .FirstOrDefaultAsync(x =>
                    x.TenDangNhap == dto.TenDangNhap &&
                    x.VaiTro == "Admin");

            if (user == null || string.IsNullOrEmpty(user.MatKhauHash))
                return Unauthorized("Sai tài khoản hoặc không có quyền admin.");

            // Kiểm tra mật khẩu bằng MD5("hehe" + password)
            var ok = PasswordHasher.VerifyPassword(dto.MatKhau, user.MatKhauHash);
            if (!ok)
                return Unauthorized("Sai mật khẩu.");

            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        // 🔒 Admin đã đăng nhập mới đổi được mật khẩu
        [HttpPost("change-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            // Lấy Id admin từ token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                             ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
                return Unauthorized("Không xác định được người dùng từ token.");

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized("Token không hợp lệ.");

            var user = await _context.NhanViens
                .FirstOrDefaultAsync(x => x.Id == userId && x.VaiTro == "Admin");

            if (user == null || string.IsNullOrEmpty(user.MatKhauHash))
                return Unauthorized("Không tìm thấy tài khoản admin.");

            // Kiểm tra mật khẩu cũ
            var oldOk = PasswordHasher.VerifyPassword(dto.MatKhauCu, user.MatKhauHash);
            if (!oldOk)
                return BadRequest("Mật khẩu cũ không đúng.");

            // Tạo hash mới và lưu
            user.MatKhauHash = PasswordHasher.CreatePasswordHash(dto.MatKhauMoi);
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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // 👈 để change-password lấy Id
                new Claim(JwtRegisteredClaimNames.UniqueName, user.HoTen),
                new Claim(ClaimTypes.Name, user.HoTen),
                new Claim(ClaimTypes.Role, user.VaiTro),               // "Admin"
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
