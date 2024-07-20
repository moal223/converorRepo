using System.ComponentModel.DataAnnotations;

namespace converor.Core.Models
{
    public class FileDescription
    {
        [Key]
        public int Id { get; set; }
        public string ContentDisposition { get; set; }
        public string ContentType { get; set; }
        public DateTime UploadedDate { get; set; }
        [StringLength(250)]
        public string FileName { get; set; }
        public long Size { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public FileContent Content { get; set; }
    }
}
