using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models.Domain
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; } 
        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public string DocumentPath { get; set; }
        public DateTime UploadedAt { get; set; }

        public Note Note { get; set; }

    }
}
