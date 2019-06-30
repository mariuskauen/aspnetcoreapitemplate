using Microsoft.EntityFrameworkCore;
using soapApi.Models;
using soapApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soapApi.Data
{
    public class FriendRepository : IFriendRepository
    {
        private readonly DataContext _context;

        public FriendRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<MyFriendRequestsViewModel>> GetMyRequests(string _userid)
        {
            MyFriendRequestsViewModel vm;
            List<MyFriendRequestsViewModel> friendRequests = new List<MyFriendRequestsViewModel>();
            var user = await _context.Users.Include(g => g.MyRequests).ThenInclude(MyRequests => MyRequests.Receiver).FirstOrDefaultAsync(x => x.Id == _userid);

            var myRequests = user.MyRequests.ToList();
            foreach (FriendRequest request in myRequests)
            {
                if (!request.Accepted)
                {
                    vm = new MyFriendRequestsViewModel()
                    {
                        FriendId = request.ReceiverId,
                        FriendName = request.Receiver.Username
                    };

                    friendRequests.Add(vm);
                }
            }
            return friendRequests;
        }

        public async Task<List<MyFriendRequestsViewModel>> GetOthersRequests(string _userid)
        {
            MyFriendRequestsViewModel vm;
            List<MyFriendRequestsViewModel> friendRequests = new List<MyFriendRequestsViewModel>();
            var user = await _context.Users.Include(g => g.OthersRequests).ThenInclude(OthersRequests => OthersRequests.Sender).FirstOrDefaultAsync(x => x.Id == _userid);

            var othersRequests = user.OthersRequests.ToList();
            foreach (FriendRequest request in othersRequests)
            {
                if (!request.Accepted)
                {
                    vm = new MyFriendRequestsViewModel()
                    {
                        FriendId = request.SenderId,
                        FriendName = request.Sender.Username
                    };

                    friendRequests.Add(vm);
                }
            }
            return friendRequests;
        }
    }
}
