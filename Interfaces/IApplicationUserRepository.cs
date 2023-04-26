using Microsoft.AspNetCore.Identity;
using TODO.Models;

public interface IApplicationUserRepository
{
    Task<ApplicationUser> CreateUser(ApplicationUser user, string password);
    Task<bool> CheckPassword(ApplicationUser user, string password);
    Task<ApplicationUser> GetUserByUsername(string username);
    Task<bool> UserExists(string username);
}