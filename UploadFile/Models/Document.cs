using System.ComponentModel.DataAnnotations.Schema;

namespace UploadFile.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        //public byte[] FileData { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
