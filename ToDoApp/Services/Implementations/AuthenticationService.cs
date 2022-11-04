using AuthService.Configurations;
using AuthService.DAL.Entities;
using AuthService.DAL.Repositories.Interfaces;
using AuthService.Services.Interfaces;


namespace AuthService.Services.Implementations
{
    public class AuthenticationService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthenticationService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async  Task<AuthResponse> UserRegistration(UserRegistration user)
        {
            AuthResponse res = await  _authRepository.UserRegister( user);
            return res;

        }

        public async Task<AuthResponse> UserLogin(Login user)
        {
            AuthResponse res = await _authRepository.UserLogin(user);
            return res;

        }
    }
}
