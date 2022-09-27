using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Assignment.Core;

namespace Assignment.Infrastructure.Tests;

public class WorkItemRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly WorkItemRepository _repository;

    public WorkItemRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();


        List<Tag> tags = new()
        {
            new Tag{Id = 0, Name = "Smart", WorkItem = new List<WorkItem> 
                {
                    new WorkItem{Id = 0, State = State.Active, Title = "Ged"},
                    new WorkItem{Id = 1, State = State.New, Title = "Hest", },
                } 
            }
        };

        List<WorkItem> workItems = new()
        {
            new WorkItem{Id = 0, State = State.Active, Title = "Ged"},
            new WorkItem{Id = 1, State = State.New, Title = "Hest", },
            new WorkItem{Id = 2, State = State.Removed, Title = "Får", }
        };
        context.Tags.Add(new Tag { Id = 0, Name = "Jens", WorkItem = workItems });
        context.SaveChanges();

        _context = context;
        _repository = new WorkItemRepository(_context);
    }

    [Fact]
    public void Create_given_WorkItem_returns_Created_with_Id()
    {
        var workItem = new WorkItemCreateDTO
        (
            Title: "idk",
            AssignedToId: null,
            Description: null,
            Tags: new List<string> { "Smart" }
        );
        var (response, status) = _repository.Create(workItem);
        response.Should().Be(Response.Created);
        status.Should().Be(3);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
