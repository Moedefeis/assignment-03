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
            new WorkItem{Id = 1, State = State.Active, Title = "Project", User = users[1], UserId = 2},
            new WorkItem{Id = 2, State = State.New, Title = "Milestone", User = users[0], UserId = 1},
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
            AssignedToId: 2,
            Description: null,
            Tags: new HashSet<string>()
        );
        var (response, workItemId) = _repository.Create(workItem);
        response.Should().Be(Response.Created);
        workItemId.Should().Be(4);
        _context.WorkItems.Find(4)!.Created.AddSeconds(2).Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

    }

    [Fact]
    public void Create_given_WorkItem_returns_Conflict_with_existing_Id()
    {
        var workItem = new WorkItemCreateDTO
        (
            Title: "Task",
            AssignedToId: 1,
            Description: null,
            Tags: new HashSet<string>()
        );
        var (response, workItemId) = _repository.Create(workItem);
        response.Should().Be(Response.Conflict);
        workItemId.Should().Be(3);
    }

    [Fact]
    public void Create_given_WorkItem_with_non_existing_userId_returns_BadRequest_with_given_userId()
    {
        var workItem = new WorkItemCreateDTO
        (
            Title: "Bug",
            AssignedToId: 69,
            Description: null,
            Tags: new HashSet<string>()
        );
        var (response, workItemId) = _repository.Create(workItem);
        response.Should().Be(Response.BadRequest);
        workItemId.Should().Be(69);
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
            State: State.Closed
        );
        _repository.Update(workItem).Should().Be(Response.Updated);
        var entity = _context.WorkItems.FirstOrDefault(w => w.Id == 1);
        entity!.Title.Should().Be("Bug");
        entity!.UserId.Should().Be(1);
        _context.WorkItems.Find(1)!.StateUpdated.AddSeconds(2).Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Update_given_WorkItem_with_non_existing_userId_returns_BadRequest()
    {
        var workItem = new WorkItemUpdateDTO
        (
            Id: 1,
            Title: "Bug",
            AssignedToId: 69,
            Description: null,
            Tags: new HashSet<string>(),
            State: State.Active
        );
        _repository.Update(workItem).Should().Be(Response.BadRequest);
    }

    [Fact]
    public void Read_returns_all_workItems()
    {
        var workItems = new WorkItemDTO[]
        {
            new WorkItemDTO(2, "Milestone", "Jens", new HashSet<string>(), State.New),
            new WorkItemDTO(1, "Project", "Bo", new HashSet<string>(), State.Active),
            new WorkItemDTO(3, "Task", "", new HashSet<string>(), State.Removed),
        };
        _repository.Read().Should().BeEquivalentTo(workItems);
    }

    [Fact]
    public void ReadRemoved_returns_all_Removed_workItems()
    {
        var workItems = new WorkItemDTO[]
        {
            new WorkItemDTO(3, "Task", "", new HashSet<string>(), State.Removed),
        };
        _repository.ReadRemoved().Should().BeEquivalentTo(workItems);
    }

    [Fact]
    public void ReadByUser_given_userId_returns_workItems()
    {
        var workItems = new WorkItemDTO[]
        {
            new WorkItemDTO(1, "Project", "Bo", new HashSet<string>(), State.Active),
        };
        _repository.ReadByUser(2).Should().BeEquivalentTo(workItems);
    }

    [Fact]
    public void ReadByUser_given_non_existing_userId_returns_no_workItems() => _repository.ReadByUser(69).Should().BeEmpty();

    public void Dispose() => _context.Dispose();
}
