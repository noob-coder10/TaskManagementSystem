using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TaskManagementSystem.Models.Domain;

namespace TaskManagementSystem.Data
{
    public class TaskManagementDbContext : DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
        {
        }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Domain.Task> Tasks { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}
