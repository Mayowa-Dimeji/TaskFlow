namespace TaskFlow.Models
{
    public class UpdateTaskDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public string? PriorityLevel { get; set; }
        public List<string>? Tags { get; set; }
        public DateTime? DueDate { get; set; }
    }

}