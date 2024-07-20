using TaskManagementSystem.Models.Domain;

namespace TaskManagementSystem.Models.DTO.ProjectDto
{
    public class ProjectDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }


        public ProjectMemberDto Manager { get; set; }


        public List<ProjectMemberDto>? ProjectMembers { get; set; }

        public List<ProjectTaskDto>? AssignedTasks { get; set; }

    }
}
