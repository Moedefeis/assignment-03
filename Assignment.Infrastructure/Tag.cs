namespace Assignment.Infrastructure;


public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<WorkItem> WorkItem { get; set; } //many-to-many like a join statemen
}
