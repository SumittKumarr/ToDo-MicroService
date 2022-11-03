

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ToDoService.DAL.DbContexts;
using ToDoService.DAL.Entities;
using ToDoService.DAL.Repositories.Interfaces;

namespace ToDoService.DAL.Repositories.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddToDoInDatabaseAsync(ToDo todo)
        {
            var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Name == todo.Name);
            if (existingTask == null)
            {
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ToDoResponseDTO>> GetAllUserTasksFromDatabaseAsync(string userId)
        {
            var userTasks = await _context.Tasks.Where(t => t.userId == userId && t.IsDeleted == false).ProjectTo<ToDoResponseDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return userTasks;
        }

        public async Task<ToDoResponseDTO> UpdateTaskInDatabaseAsync(UpdateTaskDTO updateTask)
        {
            var existingTask = await _context.Tasks.FirstOrDefaultAsync(p => p.Name == updateTask.Name);
            if(existingTask != null)
            {
                var task = _mapper.Map<ToDo>(updateTask);
                _context.Add(task);
                await _context.SaveChangesAsync();
                var taskModel = _mapper.Map<ToDoResponseDTO>(task);
                return taskModel;

            }
            return null;

            
        }

        public async Task<bool> DeleteTaskFromDatabase(string taskName)
        {
            var existingTask = await _context.Tasks.FirstOrDefaultAsync(p => p.Name == taskName);
            if(existingTask != null)
            {
                existingTask.IsDeleted = true;
                _context.Add(existingTask);
                await _context.SaveChangesAsync();
               return true;

            }
            return false;
            
        }
    }

    
}
