using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.ProjectDto;
using TaskManagementSystem.Models.DTO.TaskDto;

namespace TaskManagementSystem.Services.TaskService
{
    public interface ITaskService
    {
        Task<AddTaskRequestDto> AddTask(AddTaskRequestDto addTaskRequestDto);
        Task<UpdateTaskRequestDto> UpdateTask(int id, UpdateTaskRequestDto updateTaskRequestDto);
        Task<Models.Domain.Task> RemoveTask(int taskId, int requesterId);
        Task<List<TaskDto>> GetAllTaskByProjectIdEmpId(Guid projectId, int requesterId);
    }
}
