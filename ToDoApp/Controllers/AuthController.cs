using AuthService.Services.Implementations;
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
using AuthService.Configurations;
using AuthService.DAL.Entities;
using AuthService.Services.Interfaces;

namespace AuthService.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        public AuthController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor, IAuthService authService)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _authService = authService;

        }


        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(UserRegistration userDetail)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var response = await _authService.UserRegistration(userDetail);
                    return BadRequest(new Response()
                    {
                        Status = response.Status,
                        Error = response.Error,
                        Token = response.Token
                
                    });

                }
                return BadRequest(new Response()
                {
                    Status = false,
                    Error = new List<string> {
                    "Invalid Payload"
                }
                });




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.StackTrace);
            };

        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(Login userDetail)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _authService.UserLogin(userDetail);
                    return BadRequest(new Response()
                    {
                        Status = response.Status,
                        Error = response.Error,
                        Token = response.Token

                    });

                }
                return BadRequest(new Response()
                {
                    Status = false,
                    Error = new List<string> {
                    "Invalid Payload"
                }
                });




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.StackTrace);
            };
            


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




        
    }
}
