using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS1.Models
{
    public class UserManagmentContext : DbContext
    {
        public UserManagmentContext(DbContextOptions<UserManagmentContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Groups>().HasKey(e => e.Id);
            builder.Entity<Groups>().Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Entity<Users>().HasKey(e => e.Id);
            builder.Entity<Users>().Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Entity<GroupLinks>().HasKey(e => e.Id);
            builder.Entity<GroupLinks>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<GroupLinks>().HasIndex(c => new { c.ParentId, c.ChildId }).IsUnique().HasName("IX_GG");

            builder.Entity<UserGroups>().HasKey(e => e.Id);
            builder.Entity<UserGroups>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<UserGroups>().HasIndex(c => new { c.GroupId, c.UserId }).IsUnique().HasName("IX_GU");
            base.OnModelCreating(builder);
        }

        public DbSet<Groups> Groups { get; set; }
        public DbSet<GroupLinks> GroupLinks { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserGroups> UserGroups { get; set; }
    }


}
