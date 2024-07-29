using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TaskManagementSystem.Data;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.EmployeeDto;
using TaskManagementSystem.Models.DTO.ProjectDto;
using TaskManagementSystem.Services.EmployeeService;
using TaskManagementSystem.Services.ProjectsService;

namespace TaskManagementSystem.Controllers
{
    //Controller for querying, adding, updating and deleting Project entity
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProjectController(IProjectService projectService, IHttpContextAccessor httpContextAccessor)
        {
            this.projectService = projectService;
            this.httpContextAccessor = httpContextAccessor;
        }

        //Endpoint for adding a new project entity
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddProject([FromBody]AddProjectRequestDto addProjectRequestDto)
        {
            try
            {
                var managerEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await projectService.AddProject(addProjectRequestDto, managerEmail);
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
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Endpoint for getting a project details by its id
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Admin, Manager, Employee")]
        public async Task<IActionResult> GetProjectById([FromRoute] Guid id)
        {
            try
            {
                var response = await projectService.GetProjectById(id);
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

        //Endpoint for updating an existing project details
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateProject([FromRoute] Guid id, [FromBody] UpdateProjectRequestDto updateProjectRequestDto)
        {
            try
            {
                var managerEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await projectService.UpdateProject(id, updateProjectRequestDto, managerEmail);
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

        //Endpoint for removing a project entity
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RemoveProject([FromRoute] Guid id)
        {
            try
            {
                var managerEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await projectService.RemoveProject(id, managerEmail);
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
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Endpoint for adding members to a project
        [HttpPost]
        [Route("ProjectMember")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddProjectMember([FromBody] ProjectMemberRequestDto projectMemberRequestDto)
        {
            try
            {
                var managerEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await projectService.AddProjectMember(projectMemberRequestDto, managerEmail);
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

        //Endpoint for removing members from a project
        [HttpDelete]
        [Route("ProjectMember")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RemoveProjectMember([FromBody] ProjectMemberRequestDto projectMemberRequestDto)
        {
            try
            {
                var managerEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await projectService.RemoveProjectMember(projectMemberRequestDto, managerEmail);
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
    }
}
