using System;

namespace API.DTOs
{
    public class AuditLogSearchRequest
    {
        public string? Keyword { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}
