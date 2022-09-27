using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Assignment.Core;

namespace Assignment.Infrastructure.Tests;

public class TagRepositoryTests : IDisposable
{
	private readonly KanbanContext _context;
	private readonly TagRepository _repository;

	public TagRepositoryTests()
	{
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        List<WorkItem> items = new();

        List<WorkItem> workItems = new()
        {
            new WorkItem{Id = 0, State = State.Active, Title = "Ged"},
            new WorkItem{Id = 1, State = State.New, Title = "Hest"}
        };
        context.Tags.Add(new Tag { Id = 0, Name = "Jens", WorkItem = workItems });
        context.SaveChanges();

        _context = context;
        _repository = new TagRepository(_context);
    }

	public void Dispose()
	{
        _context.Dispose();
	}


    [Fact]

    public void CreateGivenTag(){
        //Arrange
        var (Response, TagId) = _repository.Create(new TagCreateDTO("ITU"));

        //Assert
        Response.Should().Be(Response.Created);

        TagId.Should().Be(2);

    }    
}
