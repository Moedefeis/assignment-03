using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Assignment.Infrastructure;

public class KanbanContext : DbContext
{   
    public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) { }

    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) //this confirgure the properties in the classes
    {
        
            //entity.HasKey(e => e.Id);

            modelBuilder.Entity<WorkItem>()
            .Property(e => e.Title)
            .HasMaxLength(100).IsRequired(); //makes a maks length of 100

            //entity.Property(e => e.AssignedTo);
            modelBuilder.Entity<WorkItem>()
            .HasOne<User>(e => e.AssignedTo); //optional AssignedTo User

            modelBuilder.Entity<WorkItem>()
            .Property(e => e.Description);

             modelBuilder.Entity<WorkItem>()
             .Property(e => e.State)
             .HasConversion<string>().IsRequired();
        
            

            modelBuilder.Entity<User>()
            .Property(e => e.Name)
            .HasMaxLength(100).IsRequired(); //makes a maks length of 100

            modelBuilder.Entity<User>()
            .Property(e => e.Email)
            .HasMaxLength(100).IsRequired(); //makes a maks length of 100

            modelBuilder.Entity<User>()
            .HasIndex(e => e.Email).IsUnique();
        
        

            modelBuilder.Entity<Tag>()
            .Property(e => e.Name)
            .HasMaxLength(50); //makes a maks length of 100

            modelBuilder.Entity<Tag>()
            .HasIndex(e => e.Name)
            .IsUnique(); //makes it Unique
    }
}