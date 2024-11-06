namespace DemoAPI.Data;

public class ToDo
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsComplete { get; set; }
}
