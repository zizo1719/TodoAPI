using TODO.Models;

namespace TODO.Interfaces
{
    public interface ITodoItemRepository
    {
        Task<ToDoItem> GetTodoItemAsync(int id);
        Task<List<ToDoItem>> GetTodoItemsAsync();
        Task CreateTodoItemAsync(ToDoItem todoItem);
        Task UpdateTodoItemAsync(ToDoItem todoItem);
        Task DeleteTodoItemAsync(int id);
    }
}
