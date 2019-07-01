using Microsoft.EntityFrameworkCore;
using soapApi.Models;
using soapApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soapApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<List<UserListViewModel>> GetUsersForList(string userId)
        {
            List<UserListViewModel> userList = new List<UserListViewModel>();
            UserListViewModel vm = new UserListViewModel();
            var users = await _context.Users.ToListAsync();
            var user = await _context.Users.Include(f => f.AddedFriends).Include(g => g.FriendsAdded).Include(h => h.MyRequests).Include(j => j.OthersRequests).FirstOrDefaultAsync(s => s.Id == userId);

            foreach (User u in users)
            {
                
                vm.Id = user.Id;
                vm.Firstname = user.Firstname;
                vm.Lastname = user.Lastname;

            }

            return userList;
        }
    }
}
