using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagementSystem.Data;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.NoteDto;
using TaskManagementSystem.Models.DTO.ProjectDto;
using TaskManagementSystem.Models.DTO.TaskDto;
using TaskManagementSystem.Services.NoteService;
using TaskManagementSystem.Services.TaskService;

namespace TaskManagementSystem.Controllers
{
    //Controller for querying, adding, updating and deleting note entity
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService noteService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public NoteController( INoteService noteService, IHttpContextAccessor httpContextAccessor)
        {
            this.noteService = noteService;
            this.httpContextAccessor = httpContextAccessor;
        }

        //Endpoint for adding a new note
        [HttpPost]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> AddNote([FromBody] AddNoteRequestDto addNoteRequestDto)
        {
            try
            {
                var requesterEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await noteService.AddNote(addNoteRequestDto, requesterEmail);
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

        //Endpoint for modifying a task
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> UpdateNote([FromRoute] int id, [FromBody] UpdateNoteRequestDto updateNoteRequestDto)
        {
            try
            {
                var requesterEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await noteService.UpdateNote(id, updateNoteRequestDto, requesterEmail);
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

        //Endpoint for deleting a note
        [HttpDelete]
        [Route("{taskId:int}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> RemoveNote([FromRoute] int taskId)
        {
            try
            {
                var requesterEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await noteService.RemoveNote(taskId, requesterEmail);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Endpoint for getting a list of all notes of a task
        [HttpGet]
        [Route("{taskId:int}")]
        [Authorize(Roles = "Admin, Manager, Employee")]
        public async Task<IActionResult> GetAllNoteByTaskIdEmpId([FromRoute] int taskId)
        {
            try
            {
                var requesterEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var response = await noteService.GetAllNoteByTaskIdEmpId(taskId, requesterEmail);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
