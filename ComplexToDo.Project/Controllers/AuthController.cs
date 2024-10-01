using ComplexToDo.Project.Models;
using ComplexToDo.Project.Repositories.IRepositories;
using ComplexToDo.Project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Project.Services;

namespace ComplexToDo.Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
        public AuthController(IUserRepository userRepository, JwtService jwtService, EmailService emailService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Fullname = model.Name
            };

            var result = await _userRepository.CreateUserAsync(user, model.Password);
            if (!result)
            {
                return BadRequest("User registration failed");
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new {Token = token});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            try
            {
                var passwordValid = await _userRepository.ValidatePasswordAsync(user, model.Password);
                if (!passwordValid)
                {
                    return BadRequest("invalid credentials");
                }

                var token = _jwtService.GenerateToken(user);
                return Ok(new { Token = token, Name = user.Fullname });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest("There is no such user");
            }

            var res = await _userRepository.ForgotPasswordAsync(user);

            if (res)
            {
                return Ok(new { Message = "Password reset link has been sent to your email." });
            }
            else
            {
                return StatusCode(500, "An error occurred while sending the password reset email.");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            // Find the user by their email
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest("Invalid user email.");
            }


            var result = await _userRepository.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result)
            {
                return BadRequest("Password reset failed. Please ensure the token is valid and the new password meets the requirements.");
            }

            return Ok("Password reset successful.");
        }

    }
}
