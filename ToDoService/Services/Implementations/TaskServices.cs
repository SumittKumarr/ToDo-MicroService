

using ToDoService.DAL.Entities;
using ToDoService.DAL.Repositories.Interfaces;
using ToDoService.Services.Interfaces;

namespace ToDoApp.Services.Implementations
{
    public class TaskServices : ITaskServices
    {
        private readonly ITaskRepository _taskRepository;
        public TaskServices(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task<bool> AddToDo(ToDo todo)
        {
            var ok = _taskRepository.AddToDoInDatabaseAsync(todo).Result;
            return Task.FromResult(ok);
        }

        public Task<List<ToDoResponseDTO>> GetAllTasks(string userId)
        {
            var userTasks = _taskRepository.GetAllUserTasksFromDatabaseAsync(userId);
            return userTasks;
        }

        public Task<ToDoResponseDTO> UpdateTaskInDatabaseAsync(UpdateTaskDTO updateTask)
        {
            var task = _taskRepository.UpdateTaskInDatabaseAsync( updateTask);
            return task;
        }

        public Task DeleteTaskFromDatabase(string taskName)
        {
           var ok =  _taskRepository.DeleteTaskFromDatabase(taskName);
           return Task.CompletedTask;
        }
    }
}
