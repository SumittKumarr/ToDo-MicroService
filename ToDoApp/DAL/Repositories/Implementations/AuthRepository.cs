using AuthService.Configurations;
using AuthService.DAL.DbContexts;
using AuthService.DAL.Entities;
using AuthService.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AuthService.DAL.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        
        public AuthRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            
            
        }
        public async Task<AuthResponse> UserRegister(UserRegistration userDetail)
        {
            var existingUser = await _userManager.FindByEmailAsync(userDetail.Email);
            if (existingUser != null)
            {
                AuthResponse res = new AuthResponse()
                {
                    Status = false,
                    Error = new List<string> {
                        "Email already exist"
                        }
                };

                return res;


            }
            var newUser = new IdentityUser() { Email = userDetail.Email, UserName = userDetail.Username };
            var IsCreated = await _userManager.CreateAsync(newUser, userDetail.Password);
            if (IsCreated.Succeeded)
            {
                var token = GenerateJwtToken(newUser);
                AuthResponse res = new AuthResponse()
                {
                    Token = token,
                    Status = true,

                };
                return res;

            }
            else
            {
                AuthResponse res = new AuthResponse()
                {
                    Status = false,
                    Error = IsCreated.Errors.Select(x => x.Description).ToList()


                };
                return res;
            }
        }
        public async Task<AuthResponse> UserLogin(Login userDetail)
        {
            var existingUser = await _userManager.FindByEmailAsync(userDetail.Email);
            if (existingUser == null)
            {
                AuthResponse res = new AuthResponse()
                {
                    Status = false,
                    Error = new List<string> {
                        "User does not exist"
                        }
                };

                return res;
            }

            var IsCreated = await _userManager.CheckPasswordAsync(existingUser, userDetail.Password);
            if (!IsCreated)
            {
                AuthResponse res = new AuthResponse()
                {
                    Status = false,
                    Error = new List<string> {
                        "Wrong password"
                        }
                };

                return res;
            }
            else
            {
                var token = GenerateJwtToken(existingUser);
                AuthResponse res = new AuthResponse()
                {
                    Status = true,
                    Token = token
                };

                return res;
                

            }
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("This is my supper secret key for jwt");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
                   new Claim("Id", user.Id),
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),
                   new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }


    }
}
