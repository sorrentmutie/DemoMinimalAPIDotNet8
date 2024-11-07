using System.ComponentModel.DataAnnotations;

namespace DemoAPI.Data;

public class ToDo
{
    public int Id { get; set; }
    [Required()]
    [MaxLength(20)]
    public string? Name { get; set; } 
    public bool IsComplete { get; set; }
}
