using System.Collections.Generic;
using System.Linq;

namespace soapApi.Models
{
    public class User
    {
        public User()
        {
            MyRequests = new HashSet<FriendRequest>();
            OthersRequests = new HashSet<FriendRequest>();
            FriendsOne = new HashSet<FriendShip>();
            FriendsTwo = new HashSet<FriendShip>();
            Friends = FriendsOne.Concat(FriendsTwo).Distinct();

        }
        public string Id { get; set; }

        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public virtual ICollection<FriendRequest> MyRequests { get; set; }

        public virtual ICollection<FriendRequest> OthersRequests { get; set; }

        public ICollection<FriendShip> FriendsOne { get; set; }

        public ICollection<FriendShip> FriendsTwo { get; set; }

        public IEnumerable<FriendShip> Friends { get; set; }

    }
}