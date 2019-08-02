using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soapApi.Data;
using soapApi.Models;

namespace soapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepo;

        public UserController(DataContext context, IUserRepository userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }

        [EnableQuery()]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var userList = _userRepo.GetUsersForList(await GetUserId());
            return await _context.Users.ToListAsync();
        }

        [EnableQuery()]
        [HttpGet("allusers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {          
            return await _context.Users.ToListAsync();
        }

        [EnableQuery()]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }      

        private async Task<string> GetUserId()
        {
            return HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
