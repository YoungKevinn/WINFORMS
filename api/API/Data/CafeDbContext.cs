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

        public IEnumerable<object> ThanhToan { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ------------ MAP TÊN BẢNG -------------
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

            // ------------ QUAN HỆ -------------

            // DonGoi - NhanVien (many DonGoi cho 1 NhanVien)
            modelBuilder.Entity<DonGoi>()
                .HasOne(d => d.NhanVien)
                .WithMany(n => n.DonGois)
                .HasForeignKey(d => d.NhanVienId);

            // DonGoi - Ban (many DonGoi cho 1 Ban)
            modelBuilder.Entity<DonGoi>()
                .HasOne(d => d.Ban)
                .WithMany(b => b.DonGois)
                .HasForeignKey(d => d.BanId);

            // ChiTietDonGoi - DonGoi
            modelBuilder.Entity<ChiTietDonGoi>()
                .HasOne(c => c.DonGoi)
                .WithMany(d => d.ChiTietDonGois)
                .HasForeignKey(c => c.DonGoiId);

            // ChiTietDonGoi - ThucUong (optional)
            modelBuilder.Entity<ChiTietDonGoi>()
                .HasOne(c => c.ThucUong)
                .WithMany(tu => tu.ChiTietDonGois)   // dùng navigation bên ThucUong
                .HasForeignKey(c => c.ThucUongId);

            // ChiTietDonGoi - ThucAn (optional)
            modelBuilder.Entity<ChiTietDonGoi>()
                .HasOne(c => c.ThucAn)
                .WithMany(ta => ta.ChiTietDonGois)   // dùng navigation bên ThucAn
                .HasForeignKey(c => c.ThucAnId);

            // ThanhToan - DonGoi
            modelBuilder.Entity<ThanhToan>()
                .HasOne(t => t.DonGoi)
                .WithMany(d => d.ThanhToans)
                .HasForeignKey(t => t.DonGoiId);

            // HoaDon - NhanVien
            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.NhanVien)
                .WithMany()
                .HasForeignKey(h => h.NhanVienId);

            // HoaDon - Ban
            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.Ban)
                .WithMany()
                .HasForeignKey(h => h.BanId);

            // ------------ DECIMAL PRECISION -------------
            modelBuilder.Entity<ThucUong>()
                .Property(t => t.DonGia)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ThucAn>()
                .Property(t => t.DonGia)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ChiTietDonGoi>()
                .Property(c => c.DonGia)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ChiTietDonGoi>()
                .Property(c => c.ChietKhau)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<HoaDon>()
                .Property(h => h.TongTien)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<HoaDon>()
                .Property(h => h.GiamGia)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<HoaDon>()
                .Property(h => h.Thue)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ThanhToan>()
                .Property(t => t.SoTien)
                .HasColumnType("decimal(18,2)");
        }
    }
}
