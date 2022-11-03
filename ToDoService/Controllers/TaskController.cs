using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net;
using ToDoService.DAL.Entities;
using ToDoService.Services.Interfaces;
using System.Net.Http;

namespace ToDoService.Controllers
{
    [Route("todo/")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskServices _taskServices;

        public TaskController(ITaskServices taskServices)
        {
            _taskServices = taskServices;
        }

        [HttpPost]
        [Route("Task")]
        public async Task<ActionResult> AddTaskInDatabaseAsync([FromBody] ToDo todo)
        {
            todo.userId = ControllerContext.HttpContext.Items["userId"].ToString();
            
            bool ok;
            try
            {
                ok = await _taskServices.AddToDo(todo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.StackTrace);
            };

            return Ok(ok);
        
        
        }
        [HttpGet]
        [Route("tasks")]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetAllTasksFromDatabase()
        {
            var userId = ControllerContext.HttpContext.Items["userId"].ToString();
            List<ToDoResponseDTO> userTasks;
            try
            {
                userTasks = await _taskServices.GetAllTasks(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.StackTrace);
            };

            return Ok(userTasks);

        }

        [HttpPut]
        [Route("task")]
        public async Task<ActionResult<ToDoResponseDTO>> UpdateTask(UpdateTaskDTO updatedTask)
        {
            ToDoResponseDTO task;
            try
            {
                task = await _taskServices.UpdateTaskInDatabaseAsync(updatedTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.StackTrace);
            };

            return Ok(task);

        }

        [HttpDelete]
        [Route("task")]
        public async Task<ActionResult> DeleteTask(string taskName)
        {
            
            try
            {
                await _taskServices.DeleteTaskFromDatabase(taskName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.StackTrace);
            };

            return Ok();
        }





    }
}
