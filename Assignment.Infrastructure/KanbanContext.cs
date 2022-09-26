using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Assignment.Infrastructure;

public class KanbanContext : DbContext
{

    protected override void OnModelCreating(ModelBuilder modelBuilder) //this confirgure the properties in the classes
    {
        modelBuilder.Entity<WorkItem>(entity =>
        {
            entity.Property(e => e.Id);

            entity.Property(e => e.Title).HasMaxLength(100); //makes a maks length of 100
            entity.Property(e => e.Title).IsRequired(); // makes it required

            entity.Property(e => e.AssignedTo);

            entity.Property(e => e.Description);

            entity.Property(e => e.State).HasConversion<string>();


        });


        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.id);

            entity.Property(e => e.Name).HasMaxLength(100); //makes a maks length of 100

            entity.HasIndex(e => e.email).IsUnique();

        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50); //makes a maks length of 100

            entity.HasIndex(e => e.Name).IsUnique(); //makes it Unique

        });


    }
}