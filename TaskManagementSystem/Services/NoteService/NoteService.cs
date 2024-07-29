using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.NoteDto;

namespace TaskManagementSystem.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly TaskManagementDbContext dbContext;
        private readonly IMapper mapper;

        public NoteService(TaskManagementDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<AddNoteRequestDto> AddNote(AddNoteRequestDto addNoteRequestDto, string email)
        {
            var requester = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
            var task = await dbContext.Tasks.Include(t => t.Project).Include(t => t.AssignedTo).FirstOrDefaultAsync(t => t.TaskId == addNoteRequestDto.TaskId);

            if (task == null)
                throw new NotFoundException("Task id is not valid");

            if (requester == null || (task.AssignedTo.EmpId != requester.EmpId && task.Project.ManagerId != requester.EmpId))
                throw new UnauthorizedAccessException("You are not authorized to create a note in this task");

            var note = mapper.Map<Note>(addNoteRequestDto);

            note.CreatedAt = DateTime.UtcNow;
            note.CreatedByEmpId = requester.EmpId;

            note.Task = task;

            await dbContext.Notes.AddAsync(note);

            await dbContext.SaveChangesAsync();

            return addNoteRequestDto;
        }

        public async Task<List<NoteDto>> GetAllNoteByTaskIdEmpId(int taskId, string email)
        {
            var requester = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
            var task = await dbContext.Tasks.Include(t => t.Project).Include(t => t.AssignedTo).FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                throw new NotFoundException("Task id is not valid");

            if (requester == null || (task.Project.ManagerId != requester.EmpId && task.AssignedTo.EmpId != requester.EmpId))
                throw new UnauthorizedAccessException("You are not authorized to view these notes");

            var notes = dbContext.Notes.Include(n => n.Task).Include(n => n.Task.Project)
                                        .Where(n => n.Task.TaskId == taskId)
                                        .ToList();

            var notesDto = new List<NoteDto>();
            foreach (var note in notes)
            {
                var noteDto = mapper.Map<NoteDto>(note);
                var creater = await dbContext.Employees.FindAsync(note.CreatedByEmpId);

                noteDto.NoteCreater = mapper.Map<NoteCreaterDto>(creater);
                notesDto.Add(noteDto);
            }

            return notesDto;
        }

        public async Task<Note> RemoveNote(int taskId, string email)
        {
            var requester = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
            var note = await dbContext.Notes.FindAsync(taskId);

            if (note == null)
                throw new NotFoundException("Note is not found");

            if (requester != null || note.CreatedByEmpId != requester.EmpId)
                throw new UnauthorizedAccessException("You are not authorized to delete this note");

            dbContext.Remove(note);
            await dbContext.SaveChangesAsync();

            return note;
        }

        public async Task<UpdateNoteRequestDto> UpdateNote(int id, UpdateNoteRequestDto updateNoteRequestDto, string email)
        {
            var requester = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
            var note = await dbContext.Notes.FindAsync(id);

            if (note == null)
                throw new NotFoundException("Note is not found");

            if (requester == null || note.CreatedByEmpId != requester.EmpId)
                throw new UnauthorizedAccessException("You are not authorized to modify this note");

            note.Description = updateNoteRequestDto.Description;
            note.CreatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return updateNoteRequestDto;
        }
    }
}
