using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.ProjectDto;

namespace TaskManagementSystem.Services.ProjectsService
{
    public interface IProjectService
    {
        Task<AddProjectRequestDto> AddProject(AddProjectRequestDto addProjectRequestDto);
        Task<ProjectDto> GetProjectById(Guid id);
        Task<UpdateProjectRequestDto> UpdateProject(Guid id, UpdateProjectRequestDto updateProjectRequestDto);
        Task<Project> RemoveProject(Guid id);
        Task<ProjectMemberRequestDto> AddProjectMember(ProjectMemberRequestDto projectMemberRequestDto);
        Task<ProjectMemberRequestDto> RemoveProjectMember(ProjectMemberRequestDto projectMemberRequestDto);
    }
}
