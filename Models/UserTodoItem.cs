namespace TODO.Models
{
    public class UserTodoItem
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int TodoItemId { get; set; }
        public ToDoItem TodoItem { get; set; }
    }
}
