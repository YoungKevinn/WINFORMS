namespace API.DTOs
{
    public class BanCreateUpdateDto
    {
        public string TenBan { get; set; } = string.Empty;

        // int, giống với entity
        public int TrangThai { get; set; }
    }
}
