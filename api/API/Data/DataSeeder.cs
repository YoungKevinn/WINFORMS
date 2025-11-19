using API;
using API.Models;
using API.Security;              // 👈 để dùng PasswordHasher
using Microsoft.Extensions.Configuration;
using System.Linq;

public static class DataSeeder
{
    public static void SeedDefaultAdmin(CafeDbContext context, IConfiguration config)
    {
        var section = config.GetSection("DefaultAdmin");
        var username = section["UserName"];
        var password = section["Password"];
        var hoTen = section["HoTen"];
        var role = section["Role"] ?? "Admin";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return;

        if (context.NhanViens.Any(x => x.TenDangNhap == username))
            return;

        var admin = new NhanVien
        {
            MaNhanVien = "AD001",                                 // tuỳ bạn, có thể để null
            TenDangNhap = username,
            HoTen = string.IsNullOrWhiteSpace(hoTen)
                          ? "Quản trị hệ thống"
                          : hoTen,
            VaiTro = role,                                    // phải đúng "Admin"
            MatKhauHash = PasswordHasher.CreatePasswordHash(password)
        };

        context.NhanViens.Add(admin);
        context.SaveChanges();
    }
}
