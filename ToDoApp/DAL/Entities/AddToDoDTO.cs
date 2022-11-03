using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.DAL.Entities
{
    public class AddToDoDTO
    {
        [Required(ErrorMessage = "ToDoName is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ToDoDescription is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }


        [Column(TypeName = "bit")]
        public bool IsCompleted { get; set; } = false;
    }
}
