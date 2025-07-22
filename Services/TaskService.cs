using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskFlow.Models;

namespace TaskFlow.Services
{

    public class TaskService
    {
        private readonly HttpClient Http;
        private readonly ITokenService TokenService;
        private TaskStats? _cachedStats;

        public TaskService(HttpClient http, ITokenService tokenService)
        {
            Http = http;
            TokenService = tokenService;
        }

        public async Task<List<TaskModel>> GetAllTasksAsync()
        {
            var token = await TokenService.GetToken();
            if (string.IsNullOrWhiteSpace(token)) return new();

            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var tasks = await Http.GetFromJsonAsync<List<TaskModel>>("https://taskmanager-func-xyz-evghhpandvc6fvek.ukwest-01.azurewebsites.net/api/tasks");
            return tasks ?? new();
        }

        public async Task<TaskStats?> GetTaskStatsAsync(bool forceRefresh = false)
        {
            if (!forceRefresh && _cachedStats is not null)
                return _cachedStats;

            try
            {
                var token = await TokenService.GetToken();
                if (string.IsNullOrWhiteSpace(token)) return null;

                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var tasks = await Http.GetFromJsonAsync<List<TaskModel>>(
                    "https://taskmanager-func-xyz-evghhpandvc6fvek.ukwest-01.azurewebsites.net/api/tasks"
                );

                if (tasks is null) return null;

                var now = DateTime.UtcNow;
                _cachedStats = new TaskStats
                {
                    Total = tasks.Count,
                    Completed = tasks.Count(t => t.IsCompleted),
                    Pending = tasks.Count(t => !t.IsCompleted),
                    Overdue = tasks.Count(t => !t.IsCompleted && t.DueDate < now)
                };

                return _cachedStats;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to fetch task stats: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            try
            {
                var token = await TokenService.GetToken();
                if (string.IsNullOrWhiteSpace(token)) return false;

                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var updatePayload = new UpdateTaskDto
                {
                    Title = task.Title,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    PriorityLevel = task.PriorityLevel,
                    Tags = task.Tag is not null ? [task.Tag] : null,
                    DueDate = task.DueDate
                };

                var response = await Http.PutAsJsonAsync($"https://taskmanager-func-xyz-evghhpandvc6fvek.ukwest-01.azurewebsites.net/api/tasks/{task.Id}", updatePayload);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error updating task: {ex.Message}");
                return false;
            }
        }


        public void ClearCache() => _cachedStats = null;
    }
}