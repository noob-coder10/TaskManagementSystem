using TaskManagementSystem.Models.Domain;

namespace TaskManagementSystem.Models.DTO.DocumentDto
{
    public class DocumentDto
    {
        public int DocumentId { get; set; }
        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public string DocumentPath { get; set; }
        public DateTime UploadedAt { get; set; }

    }
}
