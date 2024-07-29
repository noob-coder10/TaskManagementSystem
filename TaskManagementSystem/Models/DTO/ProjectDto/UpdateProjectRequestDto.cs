namespace TaskManagementSystem.Models.DTO.ProjectDto
{
    public class UpdateProjectRequestDto
    {
        public string? ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public string? ProjectStatus { get; set; }

    }
}
