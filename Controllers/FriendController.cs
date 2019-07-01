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


        [HttpPost("acceptfriend/{requestId}")]
        public async Task<IActionResult> AcceptFriend(string requestId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            User me = await _context.Users
                .Include(t => t.AddedFriends)
                .ThenInclude(s => s.FriendTwo)
                .Include(t => t.FriendsAdded)
                .ThenInclude(s => s.FriendOne)
                .Include(r => r.OthersRequests)
                .ThenInclude(g => g.Sender)
                .FirstOrDefaultAsync(z => z.Id == userId);

            FriendRequest request = me.OthersRequests.FirstOrDefault(l => l.Id == requestId);
            if (request == null)
                return BadRequest("No such request");
            if (!request.IsActive)
                return BadRequest("Not active request");

            User friend = request.Sender;          

            if (friend == null)
                return BadRequest("No such user");

            foreach(FriendShip f in me.AddedFriends)
            {
                if (f.FriendTwo == friend)
                {
                    f.IsFriends = true;
                    request.IsActive = false;
                    f.FriendsSince = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            foreach (FriendShip f in me.FriendsAdded)
            {
                if (f.FriendOne == friend)
                {
                    f.IsFriends = true;
                    request.IsActive = false;
                    f.FriendsSince = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }

            FriendShip fs = new FriendShip()
            {
                FriendOne = friend,
                FriendOneId = friend.Id,
                FriendTwo = me,
                FriendTwoId = me.Id,
                IsFriends = true,
                FriendsSince = DateTime.Now
            };

            friend.AddedFriends.Add(fs);
            me.FriendsAdded.Add(fs);
            request.IsActive = false;
            await _context.SaveChangesAsync();


            return Ok();
        }

        [HttpGet("getallmyfriends")]
        public async Task<List<FriendViewModel>> GetAllMyFriends()
        {

            return await _friend.GetAllMyFriends(await GetUserId());

            //List<FriendViewModel> friends = new List<FriendViewModel>();
            //string id = await GetUserId();
            //var user = _context.Users.Include(r => r.FriendsAdded).ThenInclude(g => g.FriendOne).Include(s => s.AddedFriends).ThenInclude(g => g.FriendTwo).First(f => f.Id == id);
            //foreach(FriendShip friend in user.AddedFriends)
            //{
            //    FriendViewModel vm = new FriendViewModel()
            //    {
            //        Id = friend.FriendTwoId,
            //        Username = friend.FriendTwo.Username
                    
            //    };

            //    friends.Add(vm);
            //}
            //foreach (FriendShip friend in user.FriendsAdded)
            //{
            //    FriendViewModel vm = new FriendViewModel()
            //    {
            //        Id = friend.FriendOneId,
            //        Username = friend.FriendOne.Username
            //    };
            //    friends.Add(vm);
            //}
            //return friends;
        }

        [HttpPost("addfriend/{receiverId}")]
        public async Task<IActionResult> SendFriendRequest(string receiverId)
        {
            User receiver = await _context.Users.FirstOrDefaultAsync(u => u.Id == receiverId);
            if (receiver == null)
                return BadRequest("No such user");

            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            User sender = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            List<FriendViewModel> myFriends = await _friend.GetAllMyFriends(userId);
            foreach (FriendViewModel friendVm in myFriends)
            {
                if (friendVm.Id == receiver.Id)
                    return BadRequest("You are already friends!");
            }

            FriendRequest request = new FriendRequest()
            {
                Id = Guid.NewGuid().ToString(),
                SenderId = sender.Id,
                Sender = sender,
                Receiver = receiver,
                ReceiverId = receiver.Id,
                IsActive = true                
            };

            sender.MyRequests.Add(request);
            receiver.OthersRequests.Add(request);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet("getmyrequests")]
        public async Task<List<MyFriendRequestsViewModel>> GetMyRequests()
        {                      
            return await _friend.GetMyRequests(GetUserId().Result);
        }
        [HttpGet("getothersrequests")]
        public async Task<List<MyFriendRequestsViewModel>> GetOthersRequests()
        {       
            return await _friend.GetOthersRequests(GetUserId().Result);
        }

        [HttpGet("getallrequests")]
        public async Task<AllRequests> GetAllRequests()
        {
            AllRequests requests = new AllRequests()
            {
                My = await _friend.GetMyRequests(GetUserId().Result),
                Others = await _friend.GetOthersRequests(GetUserId().Result)
            };
            return requests;
        }

        [HttpPost("deletemyfriend/{friendId}")]
        public async Task<IActionResult> DeleteFriend(string friendId)
        {
            var user = await _context.Users.Include(z => z.AddedFriends).Include(f => f.FriendsAdded).FirstOrDefaultAsync(f => f.Id == GetUserId().Result);
            var friendship = new FriendShip();

            foreach(FriendShip fs in user.AddedFriends)
            {
                if(fs.FriendTwoId == friendId)
                {
                    friendship = fs;
                }
            }
            foreach (FriendShip fs in user.FriendsAdded)
            {
                if (fs.FriendOneId == friendId)
                {
                    friendship = fs;
                }
            }
            friendship.IsFriends = false;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<string> GetUserId()
        {
            return HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        }
    }
}