using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagementSystem.Data;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.EmployeeDto;
using TaskManagementSystem.Services.EmployeeService;

namespace TaskManagementSystem.Controllers
{
    //Controller for querying, adding, updating and deleting Employee entity
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EmployeeController(IEmployeeService employeeService, IHttpContextAccessor httpContextAccessor)
        {
            this.employeeService = employeeService;
            this.httpContextAccessor = httpContextAccessor;
        }

        //Endpoint for getting a list of all employees
        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await employeeService.GetAllEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Endpoint for getting details of a single employee by its ID
        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Employee")]
        [Route("{id:int}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
        {
            try
            {
                var requesterEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var requesterRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                var employee = await employeeService.GetEmployeeById(id, requesterEmail, requesterRole);
                return Ok(employee);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }


        //Endpoint for updating an existing employee details
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromBody] UpdateEmployeeRequestDto updateEmployeeRequestDto)
        {
            try
            {
                var response = await employeeService.UpdateEmployee(id, updateEmployeeRequestDto);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Endpoint for deleting a employee entity
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveEmployee([FromRoute] int id)
        {
            try
            {
                var response = await employeeService.RemoveEmployee(id);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
