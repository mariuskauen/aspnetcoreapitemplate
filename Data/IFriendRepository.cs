using soapApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soapApi.Data
{
    public interface IFriendRepository
    {
        Task<List<MyFriendRequestsViewModel>> GetOthersRequests(string _userid);

        Task<List<MyFriendRequestsViewModel>> GetMyRequests(string _userid);
    }
}
