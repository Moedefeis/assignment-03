namespace Assignment.Infrastructure;

public class WorkItem
{
    public int Id { get; set; }

    public string Title { get; set; }

    public User? AssignedTo { get; set; } //it is optional because it can be nullable

    public string? Description { get; set; }

    public State State { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } //many-to-many also in tag. like a join statemen
}

public enum State
{
    New,
    Active,
    Resolved,
    Closed,
    Removed
}