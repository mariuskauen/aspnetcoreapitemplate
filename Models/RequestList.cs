using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soapApi.Models
{
    public class RequestList
    {
        public ICollection<FriendRequest> MyRequests { get; set; }

        public ICollection<FriendRequest> OthersRequests { get; set; }
    }
}
