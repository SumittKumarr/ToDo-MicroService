using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApp.Configurations;
using ToDoApp.DAL.Entities;

namespace ToDoApp.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        public AuthController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;

        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(UserRegistration userDetail)
        {
            if(ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(userDetail.Email);
                if(existingUser != null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Error = new List<string> {
                        "Email already exist"
                        }
                
                    });

                    
                }
                var newUser = new IdentityUser() { Email = userDetail.Email, UserName = userDetail.Username };
                var IsCreated = await _userManager.CreateAsync(newUser, userDetail.Password);
                if(IsCreated.Succeeded)
                {
                    var token = GenerateJwtToken(newUser);
                    return Ok(new Response()
                    {
                        Token = token,
                        Status = true,
                 
                    });

                }
                else
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Error = IsCreated.Errors.Select(x => x.Description).ToList()
                        

                    });
                }

            }
            return BadRequest(new Response()
            {
                Status = false,
                Error = new List<string> {
                    "Invalid Payload"
                }
            });



        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(Login userDetail)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(userDetail.Email);
                if (existingUser == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Error = new List<string> {
                        "User does not exist"
                        }
                    });
                }

                var IsCreated = await _userManager.CheckPasswordAsync(existingUser, userDetail.Password);
                if (!IsCreated)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Error = new List<string>
                        {
                            "Wrong Password"
                        }
                    });
                }
                else
                {
                    var token = GenerateJwtToken(existingUser);
                    return Ok(new Response()
                    {
                        Token = token,
                        Status = true,

                    });
                    
                }
            }
            return BadRequest(new Response()
            {
                Status = false,
                Error = new List<string> {
                    "Invalid Payload"
                }
            });



        }

        [HttpGet]
        [Route("IsTokenValid")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> IsTokenValid()
        {
            var token = Request.Headers.Authorization.FirstOrDefault().Split(" ")[1];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string userId = jwt.Claims.First(c => c.Type == "Id").Value;

            return Ok(userId);
        }




        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_jwtConfig.Key);
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
