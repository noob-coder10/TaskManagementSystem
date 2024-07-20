using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        //Endpoint for adding a new project entity
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddProject([FromBody]AddProjectRequestDto addProjectRequestDto)
        {
            try
            {
                var response = await projectService.AddProject(addProjectRequestDto);
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
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> UpdateProject([FromRoute] Guid id, [FromBody] UpdateProjectRequestDto updateProjectRequestDto)
        {
            try
            {
                var response = await projectService.UpdateProject(id, updateProjectRequestDto);
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

        //Endpoint for removing a project entity
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RemoveProject([FromRoute] Guid id)
        {
            try
            {
                var response = await projectService.RemoveProject(id);
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

        //Endpoint for adding members to a project
        [HttpPost]
        [Route("ProjectMember")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddProjectMember([FromBody] ProjectMemberRequestDto projectMemberRequestDto)
        {
            try
            {
                var response = await projectService.AddProjectMember(projectMemberRequestDto);
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

        //Endpoint for removing members from a project
        [HttpDelete]
        [Route("ProjectMember")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RemoveProjectMember([FromBody] ProjectMemberRequestDto projectMemberRequestDto)
        {
            try
            {
                var response = await projectService.RemoveProjectMember(projectMemberRequestDto);
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
    }
}
