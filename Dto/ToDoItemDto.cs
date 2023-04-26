using System;
using System.ComponentModel.DataAnnotations;

namespace TODO.Dto
{
    public class TodoItemDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [RegularExpression(@"^\d{1,2}\/\d{1,2}\/\d{2}\s\d{1,2}:\d{2}$",
            ErrorMessage = "The DueDate field must be in the format 'MM/DD/YY hh:mm'")]
        public string DueDate { get; set; }

        [Required]
        public bool IsCompleted { get; set; }
    }
}
