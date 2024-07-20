using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        //Endpoint for adding a new task
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddTask([FromBody] AddTaskRequestDto addTaskRequestDto)
        {
            try
            {
                var response = await taskService.AddTask(addTaskRequestDto);
                return Ok(response);
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
                var response = await taskService.UpdateTask(id, updateTaskRequestDto);
                return Ok(response);
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
        [Route("{taskId:int}/{requesterId:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RemoveTask([FromRoute] int taskId, [FromRoute] int requesterId)
        {
            try
            {
                var response = await taskService.RemoveTask(taskId, requesterId);
                return Ok(response);
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
        [Route("{projectId:Guid}/{requesterId:int}")]
        [Authorize(Roles = "Admin, Manager, Employee")]
        public async Task<IActionResult> GetAllTaskByProjectIdEmpId([FromRoute] Guid projectId, [FromRoute] int requesterId)
        {
            try
            {
                var response = await taskService.GetAllTaskByProjectIdEmpId(projectId, requesterId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
