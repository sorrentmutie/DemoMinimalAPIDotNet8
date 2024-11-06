using System.ComponentModel.DataAnnotations;

namespace MyDbContext.Models;

public class ToDo
{
    public int Id { get; set; }
    [Required]
    [MaxLength(20)]
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}