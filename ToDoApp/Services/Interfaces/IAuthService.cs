

using AuthService.Configurations;
using AuthService.DAL.Entities;

namespace AuthService.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> UserRegistration(UserRegistration user);
        Task<AuthResponse> UserLogin(Login user);
    }
}
