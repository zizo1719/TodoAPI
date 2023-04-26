using Microsoft.AspNetCore.Identity;

namespace TODO.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        public List<UserTodoItem> UserTodoItems { get; set; }

    }
}
