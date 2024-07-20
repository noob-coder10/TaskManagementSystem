using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models.Domain
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }


        public Task Task { get; set; }

        public int CreatedByEmpId { get; set; }

        public List<Document> Documents { get; set; }
    }
}
