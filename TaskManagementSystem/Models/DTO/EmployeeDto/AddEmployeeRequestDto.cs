using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models.DTO.EmployeeDto
{
    public class AddEmployeeRequestDto
    {
        [Required]
        
        public string EmployeeFullName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmployeeEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string EmployeePassword { get; set; }
        public string EmployeeRole { get; set; }
    }
}
