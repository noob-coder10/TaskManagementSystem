using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Models.Domain
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

     
        public List<Project>? Projects { get; set; }
        
        public List<Task>? AssignedTasks { get; set; }

    }
}
