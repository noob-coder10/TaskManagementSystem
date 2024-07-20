using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TaskManagementSystem.Data;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.DocumentDto;

namespace TaskManagementSystem.Services.DocumentService
{
    public class DocumentService : IDocumentService
    {
        private readonly TaskManagementDbContext dbContext;
        private readonly IMapper mapper;
        private readonly string rootPath;

        public DocumentService(TaskManagementDbContext dbContext, IMapper mapper, IOptions<FileUpload> fileupload)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            rootPath = fileupload.Value.RootPath;
        }

        public async Task<List<DocumentDto>> GetAllDocumentByNoteId(int noteId)
        {
            var documents = dbContext.Documents.Include(d => d.Note).Where(d => d.Note.NoteId == noteId).ToList();

            var documentsDto = new List<DocumentDto>();
            foreach (var doc in documents)
                documentsDto.Add(mapper.Map<DocumentDto>(doc));

            return documentsDto;
        }

        public async Task<Document> RemoveDocument(int documentId, int requesterId)
        {
            var document = await dbContext.Documents.Include(d => d.Note).FirstOrDefaultAsync(d => d.DocumentId == documentId);

            if (document == null)
                throw new NotFoundException("Document is not found");

            if (document.Note.CreatedByEmpId != requesterId)
                throw new BadHttpRequestException("You are not authorized to remove this document from this note");

            if (System.IO.File.Exists(document.DocumentPath))
                System.IO.File.Delete(document.DocumentPath);

            dbContext.Documents.Remove(document);
            await dbContext.SaveChangesAsync();

            return document;
        }

        public async Task<Document> UploadDocument(AddDocumentRequestDto addDocumentRequestDto)
        {
            JsonDocument json = JsonDocument.Parse(addDocumentRequestDto.RequestBody);
            JsonElement root = json.RootElement;

            int noteId = root.GetProperty("NoteId").GetInt32();
            int requesterId = root.GetProperty("RequesterId").GetInt32();


            var note = await dbContext.Notes.FindAsync(noteId);
            if (note == null)
                throw new NotFoundException("Note is not found");


            if (note.CreatedByEmpId != requesterId)
                throw new BadHttpRequestException("You are not authorized to upload a document in this note");

            var document = addDocumentRequestDto.Document;
            if (document == null || document.Length == 0)
            {
                throw new BadHttpRequestException("No file uploaded.");
            }
            var fileName = Path.GetFileNameWithoutExtension(document.FileName);
            var fileExt = Path.GetExtension(document.FileName);

            var documentPath = rootPath + fileName + $"_{DateTime.Now:yyyyMMddHHmmssfff}" + fileExt;
            using (var stream = System.IO.File.Create(documentPath))
            {
                await document.CopyToAsync(stream);
            }

            var documentDomain = new Document()
            {
                Name = document.Name,
                ContentType = document.ContentType,
                Size = document.Length,
                DocumentPath = documentPath,
                UploadedAt = DateTime.UtcNow,
                Note = note
            };

            await dbContext.Documents.AddAsync(documentDomain);
            await dbContext.SaveChangesAsync();

            return documentDomain;
        }
    }
}
