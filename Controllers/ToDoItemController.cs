using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TODO.Dto;
using TODO.Interfaces;
using TODO.Models;
using TODO.Repository;

namespace TODO.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public TodoItemsController(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItemsAsync()
        {
            var todoItems = await _todoItemRepository.GetTodoItemsAsync();
            var todoItemDtos = new List<TodoItemDTO>();
            foreach (var todoItem in todoItems)
            {
                var todoItemDto = MapTodoItemToDto(todoItem);
                todoItemDtos.Add(todoItemDto);
            }
            return Ok(todoItemDtos);
        }
        
        [HttpGet("{id}")]
        
        public async Task<ActionResult<TodoItemDTO>> GetTodoItemAsync(int id)
        {
            var todoItem = await _todoItemRepository.GetTodoItemAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            var todoItemDto = MapTodoItemToDto(todoItem);
            return Ok(todoItemDto);
        }
        
        [HttpPost("CreatedAtAction")]
        
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem([FromBody] TodoItemDTO todoItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var todoItem = MapDtoToTodoItem(todoItemDto);
            await _todoItemRepository.CreateTodoItemAsync(todoItem);
            todoItemDto.Id = todoItem.Id;
            return CreatedAtAction(nameof(GetTodoItemAsync), new { id = todoItem.Id }, todoItemDto);
        }
       
        [HttpPut("{id}")]
        
        public async Task<IActionResult> UpdateTodoItemAsync(int id, TodoItemDTO todoItemDto)
        {
            if (id != todoItemDto.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var todoItem = MapDtoToTodoItem(todoItemDto);
            await _todoItemRepository.UpdateTodoItemAsync(todoItem);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeleteTodoItemAsync(int id)
        {
            var todoItem = await _todoItemRepository.GetTodoItemAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            await _todoItemRepository.DeleteTodoItemAsync(id);
            return NoContent();
        }

        private TodoItemDTO MapTodoItemToDto(ToDoItem todoItem)
        {
            return new TodoItemDTO
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                DueDate = todoItem.DueDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                IsCompleted = todoItem.IsCompleted
            };
        }

        private ToDoItem MapDtoToTodoItem(TodoItemDTO todoItemDto)
        {
            return new ToDoItem
            {
                Id = todoItemDto.Id,
                Title = todoItemDto.Title,
                Description = todoItemDto.Description,
                DueDate = DateTime.Parse(todoItemDto.DueDate),
                IsCompleted = todoItemDto.IsCompleted
            };
        }
    }
}
