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
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);

        // var context = new KanbanContext(builder.Options);
        _context = new KanbanContext(builder.Options);
        _context.Database.EnsureCreated();
        List<WorkItem> items = new();

        List<WorkItem> workItems = new()
        {
            new WorkItem{State = State.Active, AssignedTo = new User{Name = "Lars", Email = "mail", Tasks = items}, Title = "Ged"},
            //new WorkItem{Id = 1, State = State.New, Title = "Hest"}
        };
         _context.Tags.AddRange(new Tag {Name = "Jens", WorkItem = workItems });
        _context.SaveChanges();

        // _context = context;
        _repository = new TagRepository(_context);
    }

	public void Dispose()
	{
        _context.Database.EnsureDeleted();
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
