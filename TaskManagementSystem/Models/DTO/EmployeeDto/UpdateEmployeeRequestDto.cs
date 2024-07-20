namespace TaskManagementSystem.Models.DTO.EmployeeDto
{
    public class UpdateEmployeeRequestDto
    {
        public string? EmployeeFullName { get; set; }
        public string? EmployeeEmail { get; set; }
        public string? EmployeePassword { get; set; }
        public string? EmployeeRole { get; set; }
    }
}
