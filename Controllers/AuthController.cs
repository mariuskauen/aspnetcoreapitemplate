using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using soapApi.Data;
using soapApi.Models;
using soapApi.ViewModels;

namespace soapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository auth;
        private readonly IConfiguration config;
        public AuthController(IAuthRepository auth, IConfiguration config)
        {
            this.config = config;
            this.auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            vm.Username = vm.Username.ToLower();

            if (await auth.UserExists(vm.Username))
                return BadRequest("Username already exists!");

            var userToCreate = new User
            {
                Username = vm.Username,
                Firstname = "Marius",
                Lastname = "Skauen"
            };

            var createdUser = await auth.Register(userToCreate, vm.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            LoginResponseViewModel response = new LoginResponseViewModel();
            var userFromRepo = await auth.Login(vm.Username.ToLower(), vm.Password);

            if (userFromRepo == null)
            {
                response.StatusCode = "Unauthorized";
                return BadRequest();
            }
                

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            
            
            
                response.StatusCode = "Ok";
                response.Token = tokenHandler.WriteToken(token);

            return Ok(new { token = tokenHandler.WriteToken(token) });
            //return response;
        }
    }
}