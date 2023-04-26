using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TODO.Data;
using TODO.Models;

namespace TODO.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly string _jwtSecret;

        public AuthController(IApplicationUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtSecret = configuration.GetValue<string>("JwtSecret");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserRegistrationDto registrationDto)
        {
            if (await _userRepository.UserExists(registrationDto.Email))
            {
                return BadRequest("Username already exists.");
            }

            var newUser = new ApplicationUser
            {
                UserName = registrationDto.Email,
                Email = registrationDto.Email
            };

            var createdUser = await _userRepository.CreateUser(newUser, registrationDto.Password);

            var token = GenerateJwtToken(createdUser);

            return Ok(new { token });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]UserLoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsername(loginDto.Username);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var passwordIsValid = await _userRepository.CheckPassword(user, loginDto.Password);

            if (!passwordIsValid)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName)
    };

            var jwtSecret = Environment.GetEnvironmentVariable("JwtSecret");
            if (jwtSecret == null)
            {
                throw new Exception("JwtSecret environment variable is not set.");
            }

            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error generating JWT token");
                throw;
            }
        }

    }
}
