using API;
using API.Models;
using API.Security;              // 👈 để dùng PasswordHasher
using Microsoft.Extensions.Configuration;
using System.Linq;

public static class DataSeeder
{
    public static void SeedDefaultAdmin(CafeDbContext context, IConfiguration config)
    {
        // Đọc cấu hình từ appsettings.json (DefaultAdmin)
        var section = config.GetSection("DefaultAdmin");
        var username = section["UserName"];   // tên đăng nhập muốn dùng
        var password = section["Password"];   // mật khẩu plain
        var hoTen = section["HoTen"];         // họ tên hiển thị
        var role = section["Role"] ?? "Admin";

        // Nếu chưa cấu hình thì thôi
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return;

        // Tìm admin đã tồn tại theo TenDangNhap HOẶC MaNhanVien = "AD001"
        var existing = context.NhanViens
            .FirstOrDefault(x =>
                x.TenDangNhap == username || x.MaNhanVien == "AD001");

        // Nếu đã có rồi → update cho khớp config, KHÔNG tạo mới
        if (existing != null)
        {
            existing.MaNhanVien = existing.MaNhanVien ?? "AD001";   // nếu null thì set
            existing.TenDangNhap = username;
            existing.HoTen = string.IsNullOrWhiteSpace(hoTen)
                                ? (existing.HoTen ?? "Quản trị hệ thống")
                                : hoTen;
            existing.VaiTro = role;
            existing.MatKhauHash = PasswordHasher.CreatePasswordHash(password);

            context.SaveChanges();
            return;
        }

        // Nếu chưa có → tạo admin mới
        var admin = new NhanVien
        {
            MaNhanVien = "AD001",                                 // phải unique
            TenDangNhap = username,
            HoTen = string.IsNullOrWhiteSpace(hoTen)
                          ? "Quản trị hệ thống"
                          : hoTen,
            VaiTro = role,                                        // "Admin"
            MatKhauHash = PasswordHasher.CreatePasswordHash(password)
        };

        context.NhanViens.Add(admin);
        context.SaveChanges();
    }
}
