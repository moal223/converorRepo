using System.ComponentModel.DataAnnotations;

namespace converor.api.Dtos.Files
{
    public class GetRequestDto
    {
        public int Id { get; set; }
        public string ContentDisposition { get; set; }
        public string ContentType { get; set; }
        public DateTime UploadedDate { get; set; }
        [StringLength(250)]
        public string FileName { get; set; }
        public long Size { get; set; }
        public string UserId { get; set; }
    }
}
