using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TaskFlow.Models
{
    public class TaskModel
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string? Description { get; set; }

        [Required]
        [JsonProperty("dueDate")]
        public DateTime DueDate { get; set; } = DateTime.Today;

        [JsonProperty("priorityLevel")]
        public string? PriorityLevel { get; set; }

        [JsonProperty("tag")]
        public string? Tag { get; set; }

        [JsonProperty("isCompleted")]
        public bool IsCompleted { get; set; } = false;
    }
}
