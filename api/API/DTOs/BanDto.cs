namespace API.DTOs
{
    public class BanCreateUpdateDto
    {
        public string TenBan { get; set; } = null!;
        public bool TrangThai { get; set; }   // false = trống, true = đang dùng
    }
}
