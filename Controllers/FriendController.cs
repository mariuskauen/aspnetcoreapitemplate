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
using soapApi.ViewModels;

namespace soapApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IFriendRepository _friend;

        public FriendController(DataContext context, IFriendRepository friend)
        {
            _context = context;
            _friend = friend;
        }

        [HttpPost("addfriend/{receiverId}")]
        public async Task<IActionResult> SendFriendRequest(string receiverId)
        {
            User receiver = await _context.Users.FirstOrDefaultAsync(u => u.Id == receiverId);
            if (receiver == null)
                return BadRequest("No such user");

            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            User sender = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            FriendRequest request = new FriendRequest()
            {
                Id = Guid.NewGuid().ToString(),
                SenderId = sender.Id,
                Sender = sender,
                Receiver = receiver,
                ReceiverId = receiver.Id,
                Accepted = false                
            };

            sender.MyRequests.Add(request);
            receiver.OthersRequests.Add(request);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet("getmyrequests")]
        public async Task<List<MyFriendRequestsViewModel>> GetMyRequests()
        {          
            string _userid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            
            return await _friend.GetMyRequests(_userid);
        }
        [HttpGet("getothersrequests")]
        public async Task<List<MyFriendRequestsViewModel>> GetOthersRequests()
        {
           string _userid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            
            return await _friend.GetOthersRequests(_userid);
        }

        [HttpGet("getallrequests")]
        public async Task<AllRequests> GetAllRequests()
        {
            string _userid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            AllRequests requests = new AllRequests()
            {
                My = await _friend.GetMyRequests(_userid),
                Others = await _friend.GetOthersRequests(_userid)
            };

            return requests;
        }
    }
}