using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inveasy.Models;
using Microsoft.Extensions.Hosting;

namespace Inveasy.Data
{
    public class InveasyContext : DbContext
    {
        public InveasyContext (DbContextOptions<InveasyContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<Project> Project { get; set; } = default!;
        public DbSet<Donation> Donation { get; set; } = default!;
        public DbSet<RewardTier> RewardTier { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Role> Role { get; set; } = default!;
        public DbSet<View> View { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(s => s.Roles)
                .WithMany()
                .UsingEntity<Dictionary<string, string>>(
                "UserRole",
                    j => j
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex("RoleId", "UserId").IsUnique();
                        j.ToTable("UserRole");
                    }
                );            
        }

    }
}
