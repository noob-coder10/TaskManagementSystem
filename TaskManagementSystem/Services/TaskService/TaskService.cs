﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Models.DTO.TaskDto;

namespace TaskManagementSystem.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly TaskManagementDbContext dbContext;
        private readonly IMapper mapper;

        public TaskService(TaskManagementDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<AddTaskRequestDto> AddTask(AddTaskRequestDto addTaskRequestDto, string email)
        {
            var project = await dbContext.Projects.FindAsync(addTaskRequestDto.ProjectId);
            var requester = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);

            if (project == null)
                throw new NotFoundException("Project Id is not valid");

            if (requester == null || project.ManagerId != requester.EmpId)
                throw new UnauthorizedAccessException("You are not authorized to assign a task");

            var assignedTo = await dbContext.Employees.Include(em => em.Projects).FirstOrDefaultAsync(emp => emp.EmpId == addTaskRequestDto.AssignedToEmpId);

            if (assignedTo == null)
                throw new NotFoundException("Assigned to Employee Id is not valid");

            if (assignedTo.Projects.FirstOrDefault(project => project.ProjectId == addTaskRequestDto.ProjectId) == null)
                throw new BadHttpRequestException("Assigned to employee is not part of this project");

            if (addTaskRequestDto.DueDate < DateTime.UtcNow)
                throw new BadHttpRequestException("Please provide a valid due date");

            var task = mapper.Map<Models.Domain.Task>(addTaskRequestDto);

            task.Project = project;
            task.AssignedTo = assignedTo;
            task.CreatedAt = DateTime.UtcNow;
            await dbContext.Tasks.AddAsync(task);
            await dbContext.SaveChangesAsync();

            return addTaskRequestDto;
        }

        public async Task<List<TaskDto>> GetAllTaskByProjectIdEmpId(Guid projectId, string email)
        {
            var requester = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
            if (requester == null)
                throw new BadHttpRequestException("Requester is not found");

            var tasks = dbContext.Tasks.Include(t => t.Project)
                                        .Include(t => t.AssignedTo)
                                        .Where(t => t.Project.ProjectId == projectId && t.AssignedTo.EmpId == requester.EmpId)
                                        .ToList();
            var tasksDto = new List<TaskDto>();
            foreach (var task in tasks)
            {
                tasksDto.Add(mapper.Map<TaskDto>(task));
            }

            return tasksDto;
        }

        public async Task<Models.Domain.Task> RemoveTask(int taskId, string email)
        {
            var requester = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);

            var task = await dbContext.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                throw new NotFoundException("Task is not found");

            if (requester == null || task.Project.ManagerId != requester.EmpId)
                throw new UnauthorizedAccessException("You are not authorized to remove this task");

            dbContext.Remove(task);
            await dbContext.SaveChangesAsync();

            return task;
        }

        public async Task<UpdateTaskRequestDto> UpdateTask(int id, UpdateTaskRequestDto updateTaskRequestDto, string email)
        {
            var requester = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);

            var task = await dbContext.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.TaskId == id);
            if (task == null)
                throw new NotFoundException("Task is not found");

            var project = await dbContext.Projects.FindAsync(updateTaskRequestDto.ProjectId);

            if (project == null)
                throw new NotFoundException("Project Id is not valid");

            if (task.Project.ProjectId != updateTaskRequestDto.ProjectId)
                throw new BadHttpRequestException("This task is not part of this project");

            if (requester == null || project.ManagerId != requester.EmpId)
                throw new UnauthorizedAccessException("You are not authorized to modify this task");

            var taskDomain = mapper.Map<Models.Domain.Task>(updateTaskRequestDto);

            if (updateTaskRequestDto.AssignedToEmpId != 0)
            {
                var assignedTo = await dbContext.Employees.Include(em => em.Projects).FirstOrDefaultAsync(emp => emp.EmpId == updateTaskRequestDto.AssignedToEmpId);

                if (assignedTo == null)
                    throw new NotFoundException("Assigned to Employee Id is not valid");

                if (assignedTo.Projects.FirstOrDefault(project => project.ProjectId == updateTaskRequestDto.ProjectId) == null)
                    throw new BadHttpRequestException("Assigned to employee is not part of this project");

                task.AssignedTo = assignedTo;
            }

            if (updateTaskRequestDto.DueDate != default(DateTime) && updateTaskRequestDto.DueDate < task.CreatedAt)
                throw new BadHttpRequestException("Please provide a valid due date");

            if (taskDomain.Title != null) task.Title = taskDomain.Title;
            if (taskDomain.Description != null) task.Description = taskDomain.Description;
            if (taskDomain.Status != null) task.Status = taskDomain.Status;
            if (taskDomain.DueDate != default(DateTime)) task.DueDate = taskDomain.DueDate;
            if (taskDomain.Status != null) task.Status = taskDomain.Status;


            await dbContext.SaveChangesAsync();

            return updateTaskRequestDto;
        }
    }
}
