using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Zimozi.Assessment.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .HasOne(t => t.ChargedUser)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskComment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.TaskComments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.TaskComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public static class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new Context(
                serviceProvider.GetRequiredService<DbContextOptions<Context>>());

            if (context.Users.Any())
            {
                return;
            }

            var adminUser = new User
            {
                Name = "admin",
                Email = "admin@example.com",
                Password = HashPassword("admin123"),
                Role = Role.Admin
            };

            var regularUser = new User
            {
                Name = "user",
                Email = "user@example.com",
                Password = HashPassword("user123"),
                Role = Role.User
            };

            context.Users.AddRange(adminUser, regularUser);
            context.SaveChanges();

            var task1 = new Task
            {
                Name = "Implement Authentication",
                Description = "Add JWT authentication to the API",
                Status = Status.Pending,
                DeadlineDate = DateTime.UtcNow.AddDays(3),
                UserId = adminUser.Id
            };

            var task2 = new Task
            {
                Name = "Create User Interface",
                Description = "Design and implement user interface for task management",
                Status = Status.Pending,
                DeadlineDate = DateTime.UtcNow.AddDays(3),
                UserId = regularUser.Id
            };

            var task3 = new Task
            {
                Name = "Write Documentation",
                Description = "Create API documentation",
                Status = Status.Pending,
                DeadlineDate = DateTime.UtcNow.AddDays(5),
                UserId = adminUser.Id
            };

            context.Tasks.AddRange(task1, task2, task3);
            context.SaveChanges();

            var comment1 = new TaskComment
            {
                TaskId = task1.Id,
                UserId = adminUser.Id,
                Content = "Started working on JWT implementation"
            };

            var comment2 = new TaskComment
            {
                TaskId = task1.Id,
                UserId = regularUser.Id,
                Content = "Do we need refresh tokens as well?"
            };

            var comment3 = new TaskComment
            {
                TaskId = task2.Id,
                UserId = regularUser.Id,
                Content = "I'll use React for the front-end"
            };

            context.TaskComments.AddRange(comment1, comment2, comment3);
            context.SaveChanges();
        }

        public static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
