using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.ProjectDto;
using TaskManagementSystem.Models.DTO.TaskDto;

namespace TaskManagementSystem.Services.TaskService
{
    public interface ITaskService
    {
        Task<AddTaskRequestDto> AddTask(AddTaskRequestDto addTaskRequestDto, string requester);
        Task<UpdateTaskRequestDto> UpdateTask(int id, UpdateTaskRequestDto updateTaskRequestDto, string requester);
        Task<Models.Domain.Task> RemoveTask(int taskId, string requester);
        Task<List<TaskDto>> GetAllTaskByProjectIdEmpId(Guid projectId, string requester);
    }
}
