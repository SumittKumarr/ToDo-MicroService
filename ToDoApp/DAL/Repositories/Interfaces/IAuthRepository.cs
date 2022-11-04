using AuthService.Configurations;
using AuthService.DAL.Entities;



namespace AuthService.DAL.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthResponse> UserRegister(UserRegistration userDetail);
        Task<AuthResponse> UserLogin(Login userDetail);
    }
}
