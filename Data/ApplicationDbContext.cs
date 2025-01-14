using Microsoft.EntityFrameworkCore;
using Data.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity { Id = 1, Email = "admin@example.com", Password = "admin123" }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
