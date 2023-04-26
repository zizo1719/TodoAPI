using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TODO.Models;

namespace TODO.Data
{
    public class TodoItemDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ToDoItem> TodoItems { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserTodoItem> UserTodoItems { get; set; }

        public TodoItemDbContext(DbContextOptions<TodoItemDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship between ApplicationUser and ToDoItem using UserTodoItem join table
            modelBuilder.Entity<UserTodoItem>()
                .HasKey(uti => new { uti.UserId, uti.TodoItemId });

            modelBuilder.Entity<UserTodoItem>()
                .HasOne(uti => uti.User)
                .WithMany(u => u.UserTodoItems)
                .HasForeignKey(uti => uti.UserId);

            modelBuilder.Entity<UserTodoItem>()
                .HasOne(uti => uti.TodoItem)
                .WithMany(ti => ti.UserTodoItems)
                .HasForeignKey(uti => uti.TodoItemId);
        }
    }
}