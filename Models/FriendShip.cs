using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace soapApi.Models
{
    public class FriendShip
    {
        [ForeignKey("FriendOneId")]
        public User FriendOne { get; set; }

        public string FriendOneId { get; set; }

        [ForeignKey("FriendTwoId")]
        public User FriendTwo { get; set; }

        public string FriendTwoId { get; set; }

        public DateTime FriendsSince { get; set; }

        public bool IsFriends { get; set; }
    }
}
