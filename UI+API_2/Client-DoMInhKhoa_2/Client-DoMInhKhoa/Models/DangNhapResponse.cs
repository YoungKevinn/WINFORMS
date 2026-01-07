using Newtonsoft.Json;
using System;

namespace Client_DoMInhKhoa.Models
{
    public class DangNhapResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;

        [JsonProperty("tenDangNhap")]
        public string? TenDangNhap { get; set; }

        [JsonProperty("hoTen")]
        public string? HoTen { get; set; }

        [JsonProperty("vaiTro")]
        public string VaiTro { get; set; } = string.Empty;

        [JsonProperty("expiresAt")]
        public DateTime? ExpiresAt { get; set; }

        [JsonProperty("mustChangePassword")]
        public bool MustChangePassword { get; set; }

        [JsonProperty("hetHan")]
        public DateTime? HetHanServer { get; set; }

        [JsonIgnore]
        public DateTime? HetHanHopLe => ExpiresAt ?? HetHanServer;
    }
}
