using ToDoService.DAL.Entities;

namespace ToDoService.DAL.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<bool> AddToDoInDatabaseAsync(ToDo todo);
        Task<List<ToDoResponseDTO>> GetAllUserTasksFromDatabaseAsync(string userId);
        Task<ToDoResponseDTO> UpdateTaskInDatabaseAsync(UpdateTaskDTO updateTask);
        Task<bool> DeleteTaskFromDatabase(string taskName);
    }
}
