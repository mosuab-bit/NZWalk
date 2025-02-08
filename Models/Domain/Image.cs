using System.ComponentModel.DataAnnotations.Schema;

namespace NZ_Walk.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtenstion { get; set; }
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }

    }
}
