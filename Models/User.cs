using System.Collections.Generic;

namespace soapApi.Models
{
    public class User
    {
        public User()
        {
            MyRequests = new List<FriendRequest>();
            OthersRequests = new List<FriendRequest>();
            AddedFriends = new List<FriendShip>();
            FriendsAdded = new List<FriendShip>();

        }
        public string Id { get; set; }

        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public virtual ICollection<FriendRequest> MyRequests { get; set; }

        public virtual ICollection<FriendRequest> OthersRequests { get; set; }

        public virtual ICollection<FriendShip> AddedFriends { get; set; }

        public virtual ICollection<FriendShip> FriendsAdded { get; set; }

    }
}