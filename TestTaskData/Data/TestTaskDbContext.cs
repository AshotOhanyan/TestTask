using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TestTaskData.DbModels;

namespace TestTaskData.Data
{
    public class TestTaskDbContext : DbContext
    {
        public TestTaskDbContext()
        {

        }

        public TestTaskDbContext(DbContextOptions<TestTaskDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = Environment.GetEnvironmentVariable("TestTask_CONNECTIONSTRING", EnvironmentVariableTarget.Machine);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }

            optionsBuilder.UseSqlServer(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

#if DEBUG
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
#endif

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define primary keys, indexes, and other constraints here.

            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<Image>(ConfigureImage);
            modelBuilder.Entity<Friendship>(ConfigureFriendship);
        }

        private void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.UserName).IsRequired();
            builder.Property(user => user.PasswordHash).IsRequired();

            // Configure the one-to-many relationship between User and Image
            builder.HasMany(user => user.Images)
                .WithOne(image => image.User)
                .HasForeignKey(image => image.UserId);

            // Configure the many-to-many relationship between User and Friendship
            builder.HasMany(user => user.Friends)
          .WithOne(friendship => friendship.UserA) // Update this line
          .HasForeignKey(friendship => friendship.UserAId) // Update this line
          .OnDelete(DeleteBehavior.Restrict); // Configure the desired delete behavior
        }

        private void ConfigureImage(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(image => image.Id);

            // Configure the many-to-one relationship between Image and User
            builder.HasOne(image => image.User)
                .WithMany(user => user.Images)
                .HasForeignKey(image => image.UserId);
        }

        private void ConfigureFriendship(EntityTypeBuilder<Friendship> builder)
        {
            builder.HasKey(friendship => friendship.Id);

            builder.HasOne(friendship => friendship.UserA)
                .WithMany(user => user.Friends)
                .HasForeignKey(friendship => friendship.UserAId)
                .OnDelete(DeleteBehavior.Restrict); // Configure the desired delete behavior

            builder.HasOne(friendship => friendship.UserB)
                .WithMany()
                .HasForeignKey(friendship => friendship.UserBId)
                .OnDelete(DeleteBehavior.Restrict); // Configure the desired delete behavior
        }
    }
}
