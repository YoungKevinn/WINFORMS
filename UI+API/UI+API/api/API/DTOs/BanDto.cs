namespace API.DTOs
{
    public class BanDto
    {
        public int Id { get; set; }
        public string TenBan { get; set; } = string.Empty;

        // int, giống với entity
        public int TrangThai { get; set; }
    }
}
