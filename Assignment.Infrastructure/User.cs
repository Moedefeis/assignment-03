namespace Assignment.Infrastructure;

public class User
{
    public int id { get; set; }

    [Required]
    public string Name { get; set; }


    public string email { get; set; }

    public ICollection<Task> Tasks { get; set; }

}
