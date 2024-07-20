using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TaskManagementSystem.Data;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Models.DTO.DocumentDto;
using TaskManagementSystem.Models.DTO.NoteDto;
using TaskManagementSystem.Services.DocumentService;
using TaskManagementSystem.Services.NoteService;

namespace TaskManagementSystem.Controllers
{
    //Controller for querying, adding, updating and deleting document entity
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService documentService;

        public DocumentController(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        //Endpoint for uploading a document under a note
        [HttpPost("upload")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> UploadDocument([FromForm] AddDocumentRequestDto addDocumentRequestDto)
        {
            try
            {
                var response = await documentService.UploadDocument(addDocumentRequestDto);
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

        //Endpoint for removing an uploaded document
        [HttpDelete]
        [Route("{documentId:int}/{requesterId:int}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> RemoveDocument([FromRoute] int documentId, [FromRoute] int requesterId)
        {
            try
            {
                var response = await documentService.RemoveDocument(documentId, requesterId);
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

        //Endpoint for getting all documents of a note
        [HttpGet]
        [Route("{noteId:int}")]
        [Authorize(Roles = "Admin, Manager, Employee")]
        public async Task<IActionResult> GetAllDocumentByNoteId([FromRoute] int noteId)
        {
            try
            {
                var response = await documentService.GetAllDocumentByNoteId(noteId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
