using System.Collections.Generic;

namespace API.Models
{
    public class ThucAn
    {
        public int Id { get; set; }

        // FK tới DanhMuc
        public int DanhMucId { get; set; }

        // Tên món ăn
        public string Ten { get; set; } = string.Empty;

        // Đơn giá
        public decimal DonGia { get; set; }

        // Đang hoạt động hay đã ẩn (true = còn bán)
        public bool DangHoatDong { get; set; }

        // Navigation tới DanhMuc
        public DanhMuc? DanhMuc { get; set; }

        // Navigation tới các ChiTietDonGoi có món ăn này
        public ICollection<ChiTietDonGoi> ChiTietDonGois { get; set; }
            = new List<ChiTietDonGoi>();
    }
}
