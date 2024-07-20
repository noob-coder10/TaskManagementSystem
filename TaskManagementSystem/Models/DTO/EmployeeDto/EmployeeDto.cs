using TaskManagementSystem.Models.Domain;

namespace TaskManagementSystem.Models.DTO.EmployeeDto
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeRole { get; set; }


        public List<EmployeeProjectDto>EmployeeProjects { get; set; }
    }
}
