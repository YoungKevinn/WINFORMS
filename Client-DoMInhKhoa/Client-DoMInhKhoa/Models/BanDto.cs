namespace Client_DoMInhKhoa.Models
{
    public class BanDto
    {
        public int Id { get; set; }
        public string TenBan { get; set; } = string.Empty;

        // PHẢI là int
        public int TrangThai { get; set; }
    }
}
