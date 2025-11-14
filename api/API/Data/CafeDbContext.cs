using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class CafeDbContext : DbContext
    {
        public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options) { }

        public DbSet<NhanVien> NhanViens => Set<NhanVien>();
        public DbSet<Ban> Bans => Set<Ban>();
        public DbSet<DanhMuc> DanhMucs => Set<DanhMuc>();
        public DbSet<ThucUong> ThucUongs => Set<ThucUong>();
        public DbSet<ThucAn> ThucAns => Set<ThucAn>();
        public DbSet<DonGoi> DonGois => Set<DonGoi>();
        public DbSet<ChiTietDonGoi> ChiTietDonGois => Set<ChiTietDonGoi>();
        public DbSet<ThanhToan> ThanhToans => Set<ThanhToan>();
        public DbSet<HoaDon> HoaDons => Set<HoaDon>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NhanVien>().ToTable("NhanVien");
            modelBuilder.Entity<Ban>().ToTable("Ban");
            modelBuilder.Entity<DanhMuc>().ToTable("DanhMuc");
            modelBuilder.Entity<ThucUong>().ToTable("ThucUong");
            modelBuilder.Entity<ThucAn>().ToTable("ThucAn");
            modelBuilder.Entity<DonGoi>().ToTable("DonGoi");
            modelBuilder.Entity<ChiTietDonGoi>().ToTable("ChiTietDonGoi");
            modelBuilder.Entity<ThanhToan>().ToTable("ThanhToan");
            modelBuilder.Entity<HoaDon>().ToTable("HoaDon");
            modelBuilder.Entity<AuditLog>().ToTable("AuditLog");

            modelBuilder.Entity<DonGoi>()
                .HasOne(d => d.NhanVien)
                .WithMany(n => n.DonGois)
                .HasForeignKey(d => d.NhanVienId);

            modelBuilder.Entity<DonGoi>()
                .HasOne(d => d.Ban)
                .WithMany(b => b.DonGois)
                .HasForeignKey(d => d.BanId);

            modelBuilder.Entity<ChiTietDonGoi>()
                .HasOne(c => c.DonGoi)
                .WithMany(d => d.ChiTietDonGois)
                .HasForeignKey(c => c.DonGoiId);
        }
    }
}
