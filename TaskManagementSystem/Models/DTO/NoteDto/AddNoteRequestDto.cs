namespace TaskManagementSystem.Models.DTO.NoteDto
{
    public class AddNoteRequestDto
    {
        public string Description { get; set; }

        public int TaskId { get; set; }

        public int CreatedByEmpId { get; set; }
    }
}
