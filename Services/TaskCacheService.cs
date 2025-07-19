using TaskFlow.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace TaskFlow.Services
{
    public class TaskCacheService(HttpClient http, ITokenService tokenService, TaskService taskService)
    {
        private readonly HttpClient _http = http;
        private readonly TaskService _taskService = taskService;
        private readonly ITokenService _tokenService = tokenService;

        public List<TaskModel> CachedTasks { get; private set; } = new();


        public event Action? OnTasksChanged;

        public async Task LoadTasksAsync()
        {
            var token = await _tokenService.GetToken();
            if (string.IsNullOrWhiteSpace(token)) return;

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var tasks = await _taskService.GetAllTasksAsync();

            if (tasks is not null)
            {
                CachedTasks = tasks;
                OnTasksChanged?.Invoke();
            }
        }

        public void AddTask(TaskModel task)
        {
            CachedTasks.Add(task);
            OnTasksChanged?.Invoke();
        }

        public void UpdateTask(TaskModel task)
        {
            var index = CachedTasks.FindIndex(t => t.Id == task.Id);
            if (index >= 0)
            {
                CachedTasks[index] = task;
                OnTasksChanged?.Invoke();
            }
        }

        public void RemoveTask(string taskId)
        {
            CachedTasks.RemoveAll(t => t.Id == taskId);
            OnTasksChanged?.Invoke();
        }
        public async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            var success = await _taskService.UpdateTaskAsync(task);
            if (success)
            {
                UpdateTask(task);
            }
            return success;
        }

        public List<TaskModel> GetFiltered(string filter)
        {
            var now = DateTime.UtcNow;
            return filter switch
            {
                "Today" => CachedTasks.Where(t => t.DueDate.Date == now.Date).ToList(),
                "This Week" => CachedTasks.Where(t => t.DueDate >= now && t.DueDate < now.AddDays(7)).ToList(),
                "Completed" => CachedTasks.Where(t => t.IsCompleted).ToList(),
                "Pending" => CachedTasks.Where(t => !t.IsCompleted).ToList(),
                _ => CachedTasks
            };
        }
        public TaskStats GetStats()
        {
            var now = DateTime.UtcNow;
            return new TaskStats
            {
                Total = CachedTasks.Count,
                Completed = CachedTasks.Count(t => t.IsCompleted),
                Pending = CachedTasks.Count(t => !t.IsCompleted),
                Overdue = CachedTasks.Count(t => !t.IsCompleted && t.DueDate < now)
            };
        }

        public Task<TaskStats> GetStatsAsync() => Task.FromResult(GetStats());

    }

}