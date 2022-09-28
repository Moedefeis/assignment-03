namespace Assignment.Infrastructure;

public class WorkItem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public User? User { get; set; } //it is optional because it can be nullable

    public int? UserId { get; set; } //it is optional because it can be nullable

    public string? Description { get; set; }

    public State State { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = null!; //many-to-many also in tag. like a join statemen

    public DateTime Created { get; set; }

    public DateTime StateUpdated { get; set; }
}