using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementSystem.Data;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.ProjectDto;
using TaskManagementSystem.Models.DTO.TaskDto;
using TaskManagementSystem.Services.ProjectsService;
using TaskManagementSystem.Services.TaskService;

namespace TaskManagementSystem.Controllers
{
    //Controller for querying, adding, updating and deleting task entity
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TaskController(ITaskService taskService, IHttpContextAccessor httpContextAccessor)
        {
            this.taskService = taskService;
            this.httpContextAccessor = httpContextAccessor;
        }

        //Endpoint for adding a new task
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddTask([FromBody] AddTaskRequestDto addTaskRequestDto)
        {
            try
            {
                var requesterEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await taskService.AddTask(addTaskRequestDto, requesterEmail);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Endpoint for updating an existing task
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateTask([FromRoute] int id, [FromBody] UpdateTaskRequestDto updateTaskRequestDto)
        {
            try
            {
                var requesterEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await taskService.UpdateTask(id, updateTaskRequestDto, requesterEmail);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Endpoint for removing a task
        [HttpDelete]
        [Route("{taskId:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RemoveTask([FromRoute] int taskId)
        {
            try
            {
                var requesterEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                
                var response = await taskService.RemoveTask(taskId, requesterEmail);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Endpoint for getting a list of all tasks of a project assigned to an employee
        [HttpGet]
        [Route("{projectId:Guid}")]
        [Authorize(Roles = "Admin, Manager, Employee")]
        public async Task<IActionResult> GetAllTaskByProjectIdEmpId([FromRoute] Guid projectId)
        {
            try
            {
                var requesterEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await taskService.GetAllTaskByProjectIdEmpId(projectId, requesterEmail);
                return Ok(response);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
