using TaskManagementSystem.Models.Domain;

namespace TaskManagementSystem.Models.DTO.ProjectDto
{
    public class ProjectTaskDto
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string TaskStatus { get; set; }

        public DateTime TaskCreatedAt { get; set; }
        public DateTime TaskDueDate { get; set; }


        public ProjectMemberDto TaskAssignedTo { get; set; }
    }
}
