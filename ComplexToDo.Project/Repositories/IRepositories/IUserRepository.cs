using ComplexToDo.Project.Models;

namespace ComplexToDo.Project.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<bool> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> ConfirmUserEmailASync(ApplicationUser user, string token);
        Task<bool> ForgotPasswordAsync(ApplicationUser user);
        Task<bool> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<bool> ValidatePasswordAsync(ApplicationUser user, string password);
    }
}
