using ComplexToDo.Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComplexToDo.Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<ComplexToDo.Project.Models.ToDo> ToDos { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ComplexToDo.Project.Models.ToDo>()
                .HasOne(t => t.ToDoList)
                .WithMany(t => t.ToDos)
                .HasForeignKey(t => t.ToDoListId);

            builder.Entity<ToDoList>()
                .HasOne(t => t.ApplicationUser)
                .WithMany(e => e.ToDoLists)
                .HasForeignKey(f => f.UserId);
        }
    }
}
