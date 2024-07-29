using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagementSystem.Data;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Models.Domain;
using TaskManagementSystem.Models.DTO.ProjectDto;

namespace TaskManagementSystem.Services.ProjectsService
{
    public class ProjectService : IProjectService
    {
        private readonly TaskManagementDbContext dbContext;
        private readonly IMapper mapper;

        public ProjectService(TaskManagementDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<AddProjectRequestDto> AddProject(AddProjectRequestDto addProjectRequestDto, string managerEmail)
        {
            var manager = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email.ToLower() == managerEmail.ToLower());

            if (manager == null)
                throw new UnauthorizedAccessException("You are not authorized to create a project");

            var project = mapper.Map<Project>(addProjectRequestDto);
            project.ManagerId = manager.EmpId;
            await dbContext.Projects.AddAsync(project);
            await dbContext.SaveChangesAsync();

            return addProjectRequestDto;
        }

        public async Task<ProjectMemberRequestDto> AddProjectMember(ProjectMemberRequestDto projectMemberRequestDto, string managerEmail)
        {
            var project = await dbContext.Projects.FindAsync(projectMemberRequestDto.ProjectId);
            var manager = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email.ToLower() == managerEmail.ToLower());

            if (project == null)
                throw new NotFoundException("Project not found");


            if (manager == null || project.ManagerId != manager.EmpId)
                throw new UnauthorizedAccessException("You are not authorized to add an employee to this project");

            var emp = await dbContext.Employees.Include(em => em.Projects).FirstOrDefaultAsync(emp => emp.EmpId == projectMemberRequestDto.EmpId);

            if (emp == null)
                throw new NotFoundException("Employee not found");

            if (emp.Projects.FirstOrDefault(pr => pr.ProjectId == projectMemberRequestDto.ProjectId) != null)
                throw new BadHttpRequestException("This employee is already a member of this project");


            if (project.ProjectMembers == null)
                project.ProjectMembers = new List<Employee>();

            project.ProjectMembers.Add(emp);

            await dbContext.SaveChangesAsync();

            return projectMemberRequestDto;
        }


        public async Task<ProjectDto> GetProjectById(Guid id)
        {
            var project = await dbContext.Projects.Include(e => e.ProjectMembers)
                                                  .Include(e => e.AssignedTasks)
                                                  .FirstOrDefaultAsync(e => e.ProjectId == id);
            if (project == null)
                throw new NotFoundException("Project is Not Found");

            var projectDto = mapper.Map<ProjectDto>(project);
            projectDto.Manager = mapper.Map<ProjectMemberDto>(await dbContext.Employees.FindAsync(project.ManagerId));
            return projectDto;
        }

        public async Task<Project> RemoveProject(Guid id, string managerEmail)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(proj => proj.ProjectId == id);

            if (project == null)
                throw new NotFoundException("Project is not found");

            var manager = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email.ToLower() == managerEmail.ToLower());

            if (manager == null || manager.EmpId != project.ManagerId)
                throw new UnauthorizedAccessException("You are not authorized to remove this project");

            dbContext.Remove(project);
            await dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<ProjectMemberRequestDto> RemoveProjectMember(ProjectMemberRequestDto projectMemberRequestDto, string managerEmail)
        {
            var project = await dbContext.Projects.FindAsync(projectMemberRequestDto.ProjectId);
            var manager = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email.ToLower() == managerEmail.ToLower());

            if (project == null)
                throw new NotFoundException("Project not found");


            if (manager == null || project.ManagerId != manager.EmpId)
                throw new UnauthorizedAccessException("You are not authorized to remove an employee from this project");

            var emp = await dbContext.Employees.Include(em => em.Projects).FirstOrDefaultAsync(emp => emp.EmpId == projectMemberRequestDto.EmpId);

            if (emp == null)
                throw new NotFoundException("Employee not found");

            if (emp.Projects.FirstOrDefault(pr => pr.ProjectId == projectMemberRequestDto.ProjectId) == null)
                throw new BadHttpRequestException("This employee is not a member of this project");


            if (project.ProjectMembers.FirstOrDefault(emp) != null)
                project.ProjectMembers.Remove(emp);

            await dbContext.SaveChangesAsync();

            return projectMemberRequestDto;
        }

        public async Task<UpdateProjectRequestDto> UpdateProject(Guid id, UpdateProjectRequestDto updateProjectRequestDto, string managerEmail)
        {
            var projectDomain = mapper.Map<Project>(updateProjectRequestDto);
            var project = await dbContext.Projects.FirstOrDefaultAsync(proj => proj.ProjectId == id);
            if (project == null)
                throw new NotFoundException("Project is not found");

            var manager = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email.ToLower() == managerEmail.ToLower());

            if (manager == null || manager.EmpId != project.ManagerId)
                throw new UnauthorizedAccessException("You are not authorized to modify this project");


            if (projectDomain.ProjectName != null) project.ProjectName = projectDomain.ProjectName;
            if (projectDomain.ProjectDescription != null) project.ProjectDescription = projectDomain.ProjectDescription;
            if (projectDomain.StartDate != default(DateTime)) project.StartDate = projectDomain.StartDate;
            if (projectDomain.EndDate != default(DateTime)) project.EndDate = projectDomain.EndDate;
            if (projectDomain.Status != null) project.Status = projectDomain.Status;

            if (project.StartDate > project.EndDate)
                throw new BadHttpRequestException("Project's Start Date can not be greater than project's end date");

            await dbContext.SaveChangesAsync();

            return updateProjectRequestDto;
        }
    }
}
