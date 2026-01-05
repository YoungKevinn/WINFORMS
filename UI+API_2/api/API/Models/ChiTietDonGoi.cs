using System;

namespace API.Models
{
    public class ChiTietDonGoi
    {
        public int Id { get; set; }

        public int DonGoiId { get; set; }

        // 1 dòng có thể là đồ uống hoặc đồ ăn, nên để nullable
        public int? ThucUongId { get; set; }
        public int? ThucAnId { get; set; }

        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ChietKhau { get; set; }

        public string? GhiChu { get; set; }

        // Navigation
        public DonGoi DonGoi { get; set; } = null!;
        public ThucUong? ThucUong { get; set; }
        public ThucAn? ThucAn { get; set; }
    }
}
