using soapApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soapApi.Data
{
    public interface IUserRepository
    {
        Task<List<UserListViewModel>> GetUsersForList(string userId);
    }
}
