using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Assignment.Core;

namespace Assignment.Infrastructure.Tests;

public class WorkItemRepositoryTests : IDisposable
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

        List<User> users = new()
        {
            new User{Id = 1, Name = "Jens", Email = "jens@gmail.com"},
            new User{Id = 2, Name = "Bo", Email = "bo@gmail.com"}
        };

        List<Tag> tags = new()
        {
            new Tag{Id = 1, Name = "Smart"},
            new Tag{Id = 2, Name = "Green"}
        };

        List<WorkItem> workItems = new()
        {
            new WorkItem{Id = 1, State = State.Active, Title = "Project", AssignedToId = 2},
            new WorkItem{Id = 2, State = State.New, Title = "Milestone", AssignedToId = 1},
            new WorkItem{Id = 3, State = State.Removed, Title = "Task"}
        };

        context.Users.AddRange(users);
        context.Tags.AddRange(tags);
        context.WorkItems.AddRange(workItems);
        context.SaveChanges();

        _context = context;
        _repository = new WorkItemRepository(_context);
    }

    [Fact]
    public void Create_given_WorkItem_returns_Created_with_Id()
    {
        var workItem = new WorkItemCreateDTO
        (
            Title: "Bug",
            AssignedToId: null,
            Description: null,
            Tags: new HashSet<string>()
        );
        var (response, workItemId) = _repository.Create(workItem);
        response.Should().Be(Response.Created);
        workItemId.Should().Be(4);
    }

    [Fact]
    public void Create_given_WorkItem_returns_Conflict_with_existing_Id()
    {
        var workItem = new WorkItemCreateDTO
        (
            Title: "Task",
            AssignedToId: null,
            Description: null,
            Tags: new HashSet<string>()
        );
        var (response, workItemId) = _repository.Create(workItem);
        response.Should().Be(Response.Conflict);
        workItemId.Should().Be(3);
    }

    [Fact]
    public void Delete_given_non_existing_Id_returns_NotFound() => _repository.Delete(4).Should().Be(Response.NotFound);

    [Fact]
    public void Delete_given_Id_of_Removed_WorkItem_returns_Conflict() => _repository.Delete(3).Should().Be(Response.Conflict);

    [Fact]
    public void Delete_given_Id_of_Active_WorkItem_sets_its_state_to_Removed()
    {
        _repository.Delete(1);
        var state = _context.WorkItems.FirstOrDefault(w => w.Id == 1)!.State;
        state.Should().Be(State.Removed);
    }

    [Fact]
    public void Update_given_WorkItem_returns_Updated()
    {
        var workItem = new WorkItemUpdateDTO
        (
            Id: 1,
            Title: "Bug",
            AssignedToId: 1,
            Description: null,
            Tags: new HashSet<string>(),
            State: Core.State.Active
        );
        _repository.Update(workItem).Should().Be(Response.Updated);
        var entity = _context.WorkItems.FirstOrDefault(w => w.Id == 1);
        entity!.Title.Should().Be("Bug");
        entity!.AssignedToId.Should().Be(1);
    }
    
    public void Dispose() => _context.Dispose();
}
