
using System.ComponentModel.DataAnnotations;

namespace TODO.Models
   
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [RegularExpression(@"^\d{1,2}\/\d{1,2}\/\d{2}\s\d{1,2}:\d{2}$",
            ErrorMessage = "The DueDate field must be in the format 'MM/DD/YY hh:mm'")]
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public List<UserTodoItem> UserTodoItems { get; set; }

    }

}
