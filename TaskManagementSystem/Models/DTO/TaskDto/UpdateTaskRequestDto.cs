namespace TaskManagementSystem.Models.DTO.TaskDto
{
    public class UpdateTaskRequestDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid ProjectId { get; set; }
        public int? AssignedToEmpId { get; set; }
    }
}
