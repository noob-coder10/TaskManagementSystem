using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.EmployeeDto;

namespace TaskManagementSystem.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployees();
        Task<EmployeeDto> GetEmployeeById(int id);
        Task<AddEmployeeRequestDto> AddEmployee(AddEmployeeRequestDto addEmployeeRequestDto);
        Task<UpdateEmployeeRequestDto> UpdateEmployee(int id, UpdateEmployeeRequestDto updateEmployeeRequestDto);
        Task<Employee> RemoveEmployee(int id);
    }
}
