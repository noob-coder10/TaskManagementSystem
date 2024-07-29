using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.ProjectDto;

namespace TaskManagementSystem.Services.ProjectsService
{
    public interface IProjectService
    {
        Task<AddProjectRequestDto> AddProject(AddProjectRequestDto addProjectRequestDto, string email);
        Task<ProjectDto> GetProjectById(Guid id);
        Task<UpdateProjectRequestDto> UpdateProject(Guid id, UpdateProjectRequestDto updateProjectRequestDto, string email);
        Task<Project> RemoveProject(Guid id, string email);
        Task<ProjectMemberRequestDto> AddProjectMember(ProjectMemberRequestDto projectMemberRequestDto, string email);
        Task<ProjectMemberRequestDto> RemoveProjectMember(ProjectMemberRequestDto projectMemberRequestDto, string email);
    }
}
