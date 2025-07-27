using Newtonsoft.Json;

namespace TaskFlow.Models
{
    public class UpdateTaskDto
    {
        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("isCompleted")]
        public bool? IsCompleted { get; set; }

        [JsonProperty("priorityLevel")]
        public string? PriorityLevel { get; set; }

        [JsonProperty("tags")]
        public string? Tags { get; set; }

        [JsonProperty("dueDate")]
        public DateTime? DueDate { get; set; }
    }
}
