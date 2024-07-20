using AutoMapper;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.DocumentDto;
using TaskManagementSystem.Models.DTO.EmployeeDto;
using TaskManagementSystem.Models.DTO.NoteDto;
using TaskManagementSystem.Models.DTO.ProjectDto;
using TaskManagementSystem.Models.DTO.TaskDto;

namespace TaskManagementSystem.Mappings
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Employee, AddEmployeeRequestDto>().ForMember(x => x.EmployeeFullName, y => y.MapFrom(z => z.FullName))
                                                          .ForMember(x => x.EmployeeEmail, y => y.MapFrom(z => z.Email))
                                                          .ForMember(x => x.EmployeeRole, y => y.MapFrom(z => z.Role))
                                                          .ReverseMap();

            CreateMap<Employee, UpdateEmployeeRequestDto>().ForMember(x => x.EmployeeFullName, y => y.MapFrom(z => z.FullName))
                                                          .ForMember(x => x.EmployeeEmail, y => y.MapFrom(z => z.Email))
                                                          .ForMember(x => x.EmployeeRole, y => y.MapFrom(z => z.Role))
                                                          .ReverseMap();

            CreateMap<Project, EmployeeProjectDto>().ReverseMap();

            CreateMap<Employee, EmployeeDto>().ForMember(x => x.EmployeeId, y => y.MapFrom(z => z.EmpId))
                                              .ForMember(x => x.EmployeeFullName, y => y.MapFrom(z => z.FullName))
                                              .ForMember(x => x.EmployeeEmail, y => y.MapFrom(z => z.Email))
                                              .ForMember(x => x.EmployeeRole, y => y.MapFrom(z => z.Role))
                                              .ForMember(x => x.EmployeeProjects, y => y.MapFrom(z => z.Projects))
                                              .ReverseMap();

            CreateMap<Employee, ProjectMemberDto>().ForMember(x => x.EmployeeId, y => y.MapFrom(z => z.EmpId))
                                                   .ForMember(x => x.EmployeeFullName, y => y.MapFrom(z => z.FullName))
                                                   .ForMember(x => x.EmployeeEmail, y => y.MapFrom(z => z.Email))
                                                   .ReverseMap();

            CreateMap<Project, ProjectDto>().ReverseMap();

            CreateMap<Project, AddProjectRequestDto>().ForMember(x => x.ProjectStartDate, y => y.MapFrom(z => z.StartDate))
                                                  .ForMember(x => x.ProjectEndDate, y => y.MapFrom(z => z.EndDate))
                                                  .ForMember(x => x.ProjectStatus, y => y.MapFrom(z => z.Status))
                                                  .ForMember(x => x.ProjectManagerId, y => y.MapFrom(z => z.ManagerId))
                                                  .ReverseMap();

            CreateMap<Project, UpdateProjectRequestDto>().ForMember(x => x.ProjectStartDate, y => y.MapFrom(z => z.StartDate))
                                                  .ForMember(x => x.ProjectEndDate, y => y.MapFrom(z => z.EndDate))
                                                  .ForMember(x => x.ProjectStatus, y => y.MapFrom(z => z.Status))
                                                  .ForMember(x => x.ProjectManagerId, y => y.MapFrom(z => z.ManagerId))
                                                  .ReverseMap();

            CreateMap<Models.Domain.Task, ProjectTaskDto>().ForMember(x => x.TaskTitle, y => y.MapFrom(z => z.Title))
                                                  .ForMember(x => x.TaskStatus, y => y.MapFrom(z => z.Status))
                                                  .ForMember(x => x.TaskCreatedAt, y => y.MapFrom(z => z.CreatedAt))
                                                  .ForMember(x => x.TaskDueDate, y => y.MapFrom(z => z.DueDate))
                                                  .ForMember(x => x.TaskAssignedTo, y => y.MapFrom(z => z.AssignedTo))
                                                  .ReverseMap();

            CreateMap<Models.Domain.Task, AddTaskRequestDto>().ReverseMap();

            CreateMap<Models.Domain.Task, UpdateTaskRequestDto>().ReverseMap();
            
            CreateMap<Models.Domain.Task, TaskDto>().ReverseMap();


            CreateMap<Note, AddNoteRequestDto>().ReverseMap();

            CreateMap<Employee, NoteCreaterDto>().ForMember(x => x.EmployeeId, y => y.MapFrom(z => z.EmpId))
                                                 .ForMember(x => x.EmployeeFullName, y => y.MapFrom(z => z.FullName))
                                                 .ForMember(x => x.EmployeeEmail, y => y.MapFrom(z => z.Email))
                                                 .ForMember(x => x.EmployeeRole, y => y.MapFrom(z => z.Role))
                                                 .ReverseMap();

            CreateMap<Note, NoteDto>().ReverseMap();


            CreateMap<Document, DocumentDto>().ReverseMap();
        }
    }
}
