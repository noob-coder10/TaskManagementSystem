using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.NoteDto;
using TaskManagementSystem.Models.DTO.TaskDto;

namespace TaskManagementSystem.Services.NoteService
{
    public interface INoteService
    {
        Task<AddNoteRequestDto> AddNote(AddNoteRequestDto addNoteRequestDto, string email);
        Task<UpdateNoteRequestDto> UpdateNote(int id, UpdateNoteRequestDto updateNoteRequestDto, string email);
        Task<Note> RemoveNote(int taskId, string email);
        Task<List<NoteDto>> GetAllNoteByTaskIdEmpId(int taskId, string email);
    }
}
