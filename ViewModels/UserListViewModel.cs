using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soapApi.ViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public bool IsFriends { get; set; }

        public bool IsRequested { get; set; }

    }
}
