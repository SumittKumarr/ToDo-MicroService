using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ToDoService.DAL.Entities
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ToDoName is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ToDoDescription is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }

        
        [Column(TypeName = "bit")]
        public bool IsCompleted { get; set; } = false;

        public bool IsDeleted { get; set; }
        public string userId { get; set; }
    }
}
