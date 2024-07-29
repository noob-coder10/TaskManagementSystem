namespace TaskManagementSystem.Models.DTO.ProjectDto
{
    public class ProjectMemberRequestDto
    {
        public Guid ProjectId { get; set; }
        public int EmpId { get; set; }
    }
}
