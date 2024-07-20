using TaskManagementSystem.Models.Domain;

namespace TaskManagementSystem.Models.DTO.TaskDto
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }


    }
}
