using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly TaskManagementDbContext dbContext;
        private readonly IMapper mapper;
        private readonly INoteService noteService;

        public NoteController(TaskManagementDbContext dbContext, IMapper mapper, INoteService noteService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.noteService = noteService;
        }

        //Endpoint for adding a new note
        [HttpPost]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> AddNote([FromBody] AddNoteRequestDto addNoteRequestDto)
        {
            try
            {
                var response = await noteService.AddNote(addNoteRequestDto);
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

        //Endpoint for modifying a task
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> UpdateNote([FromRoute] int id, [FromBody] UpdateNoteRequestDto updateNoteRequestDto)
        {
            try
            {
                var response = await noteService.UpdateNote(id, updateNoteRequestDto);
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

        //Endpoint for deleting a note
        [HttpDelete]
        [Route("{taskId:int}/{requesterId:int}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> RemoveNote([FromRoute] int taskId, [FromRoute] int requesterId)
        {
            try
            {
                var response = await noteService.RemoveNote(taskId, requesterId);
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

        //Endpoint for getting a list of all notes of a task
        [HttpGet]
        [Route("{taskId:int}/{requesterId:int}")]
        [Authorize(Roles = "Admin, Manager, Employee")]
        public async Task<IActionResult> GetAllNoteByTaskIdEmpId([FromRoute] int taskId, [FromRoute] int requesterId)
        {
            try
            {
                var response = await noteService.GetAllNoteByTaskIdEmpId(taskId, requesterId);
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
