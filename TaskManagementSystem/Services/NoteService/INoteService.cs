using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.NoteDto;
using TaskManagementSystem.Models.DTO.TaskDto;

namespace TaskManagementSystem.Services.NoteService
{
    public interface INoteService
    {
        Task<AddNoteRequestDto> AddNote(AddNoteRequestDto addNoteRequestDto);
        Task<UpdateNoteRequestDto> UpdateNote(int id, UpdateNoteRequestDto updateNoteRequestDto);
        Task<Note> RemoveNote(int taskId, int requesterId);
        Task<List<NoteDto>> GetAllNoteByTaskIdEmpId(int taskId, int requesterId);
    }
}
