using ToDoService.DAL.Entities;

namespace ToDoService.Services.Interfaces
{
    public interface ITaskServices
    {
        Task<bool> AddToDo(ToDo todo);
        Task<List<ToDoResponseDTO>> GetAllTasks(string userId);
        Task<ToDoResponseDTO> UpdateTaskInDatabaseAsync(UpdateTaskDTO updateTask);
        Task DeleteTaskFromDatabase(string taskName);
    }
        
}
