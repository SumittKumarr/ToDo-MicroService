
using Microsoft.EntityFrameworkCore;
using ToDoService.DAL.Entities;

namespace ToDoService.DAL.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ToDo> Tasks { get; set; }
    }
}
