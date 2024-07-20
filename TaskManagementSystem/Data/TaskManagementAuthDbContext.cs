using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Data
{
    public class TaskManagementAuthDbContext : IdentityDbContext
    {
        public TaskManagementAuthDbContext(DbContextOptions<TaskManagementAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var employeeRoleId = "55FF88FC-1534-4F82-81AC-4AF36FBF19AC";
            var managerRoleId = "24CA026E-289C-45D4-AFFE-69966F51DCA3";
            var adminRoleId = "F90621C0-49B4-45CC-BA5D-F95F147826D4";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = employeeRoleId,
                    ConcurrencyStamp = employeeRoleId,
                    Name = "Employee",
                    NormalizedName = "Employee".ToUpper()
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    ConcurrencyStamp = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "Manager".ToUpper()
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
