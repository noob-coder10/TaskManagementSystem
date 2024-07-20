using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.DocumentDto;
using TaskManagementSystem.Models.DTO.NoteDto;

namespace TaskManagementSystem.Services.DocumentService
{
    public interface IDocumentService
    {
        Task<Document> UploadDocument(AddDocumentRequestDto addDocumentRequestDto);
        Task<Document> RemoveDocument(int documentId, int requesterId);
        Task<List<DocumentDto>> GetAllDocumentByNoteId(int noteId);
    }
}
