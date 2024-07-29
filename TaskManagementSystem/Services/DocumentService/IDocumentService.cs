using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.DocumentDto;
using TaskManagementSystem.Models.DTO.NoteDto;

namespace TaskManagementSystem.Services.DocumentService
{
    public interface IDocumentService
    {
        Task<Document> UploadDocument(IFormFile document, int noteId, string email);
        Task<Document> RemoveDocument(int documentId, string email);
        Task<List<DocumentDto>> GetAllDocumentByNoteId(int noteId);
    }
}
