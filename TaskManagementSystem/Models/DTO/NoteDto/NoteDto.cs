namespace TaskManagementSystem.Models.DTO.NoteDto
{
    public class NoteDto
    {
        public int NoteId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public NoteCreaterDto NoteCreater { get; set; }
    }
}
