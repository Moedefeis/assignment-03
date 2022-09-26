namespace Assignment.Infrastructure;


public class Tag
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public WorkItem WorkItem { get; set; } //many-to-many like a join statemen
}
