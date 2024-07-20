using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.EmployeeDto;

namespace TaskManagementSystem.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly TaskManagementDbContext dbContext;
        private readonly IMapper mapper;

        public EmployeeService(TaskManagementDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<AddEmployeeRequestDto> AddEmployee(AddEmployeeRequestDto addEmployeeRequestDto)
        {
            var employeeDomain = mapper.Map<Employee>(addEmployeeRequestDto);
            await dbContext.Employees.AddAsync(employeeDomain);
            await dbContext.SaveChangesAsync();

            return addEmployeeRequestDto;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var employees = await dbContext.Employees.Include(e => e.Projects).Include(e => e.AssignedTasks).ToListAsync();

            return employees;
        }

        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            var employee = await dbContext.Employees.Include(e => e.Projects).Include(e => e.AssignedTasks).FirstOrDefaultAsync(e => e.EmpId == id);

            if (employee == null)
            {
                throw new NotFoundException("Employee is not found");
            }

            var employeeDto = mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public async Task<Employee> RemoveEmployee(int id)
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(emp => emp.EmpId == id);

            if (employee == null)
                throw new NotFoundException("Employee is not found");

            dbContext.Remove(employee);
            await dbContext.SaveChangesAsync();

            return employee;
        }

        public async Task<UpdateEmployeeRequestDto> UpdateEmployee(int id, UpdateEmployeeRequestDto updateEmployeeRequestDto)
        {
            var empDomain = mapper.Map<Employee>(updateEmployeeRequestDto);
            var emp = await dbContext.Employees.FirstOrDefaultAsync(emp => emp.EmpId == id);
            if (emp == null)
                throw new NotFoundException("Employee is not found");

            if (empDomain.FullName != null) emp.FullName = empDomain.FullName;
            if (empDomain.Email != null) emp.Email = empDomain.Email;
            if (empDomain.Role != null) emp.Role = empDomain.Role;

            await dbContext.SaveChangesAsync();

            return updateEmployeeRequestDto;
        }
    }
}
