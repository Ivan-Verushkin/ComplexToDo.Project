using ComplexToDo.Project.Data;
using ComplexToDo.Project.Models;
using ComplexToDo.Project.Repositories.IRepositories;
using ComplexToDo.Project.Services;
using Microsoft.AspNetCore.Identity;

namespace ComplexToDo.Project.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly EmailService emailService;

        public UserRepository(UserManager<ApplicationUser> userManager, EmailService emailService) {
            this.userManager = userManager;
            this.emailService = emailService;
        }
        public async Task<bool> ConfirmUserEmailASync(ApplicationUser user, string token)
        {
            var result = await userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> ForgotPasswordAsync(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("User email cannot be null or empty");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetUrl = $"http://localhost:3000/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";
            
            await emailService.SendEmailAsync(user.Email, "Reset Your Password",
                $"<h1>Password Reset</h1><p>To reset your password, click <a href='{resetUrl}'>here</a>.</p>");

            return true;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<bool> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                // Log each error (you can also return these messages to the controller)
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                }
                return false;
            }

            return true;
        }


        public async Task<bool> ValidatePasswordAsync(ApplicationUser user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }
    }
}
