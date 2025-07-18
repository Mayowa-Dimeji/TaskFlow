using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models;

public class TaskModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; } = DateTime.Today;
    public string? Priority { get; set; }
    public string? Tag { get; set; }

    public bool IsCompleted { get; set; } = false;
}
