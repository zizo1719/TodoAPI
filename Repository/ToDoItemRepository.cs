using Microsoft.EntityFrameworkCore;
using TODO.Data;
using TODO.Interfaces;
using TODO.Models;

namespace TODO.Repository
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly TodoItemDbContext _dbContext;

        public TodoItemRepository(TodoItemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ToDoItem> GetTodoItemAsync(int id)
        {
            return await _dbContext.TodoItems.FindAsync(id);
        }

        public async Task<List<ToDoItem>> GetTodoItemsAsync()
        {
            return await _dbContext.TodoItems.ToListAsync();
        }

        public async Task CreateTodoItemAsync(ToDoItem todoItem)
        {
            await _dbContext.TodoItems.AddAsync(todoItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTodoItemAsync(ToDoItem todoItem)
        {
            _dbContext.TodoItems.Update(todoItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTodoItemAsync(int id)
        {
            var todoItem = await _dbContext.TodoItems.FindAsync(id);
            _dbContext.TodoItems.Remove(todoItem);
            await _dbContext.SaveChangesAsync();
        }

        
    }
}
