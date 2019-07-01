using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace soapApi.Models
{
    public class FriendRequest
    {
        public string Id { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        public string SenderId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }

        public string ReceiverId { get; set; }

        public bool IsActive { get; set; }
    }
}
