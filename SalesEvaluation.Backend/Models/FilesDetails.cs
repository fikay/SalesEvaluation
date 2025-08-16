using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace SalesEvaluation.Backend.Models
{
    public class FilesDetails
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        public string FileType { get; set; } = string.Empty;

        public string UploadedBy { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public Guid UserId { get; set; }

        public byte[] Content { get; set; } = Array.Empty<byte>();  
    }
}
