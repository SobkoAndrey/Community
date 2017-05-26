using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Community3.Models
{
    public class ChatRoom
    {
        [Required]
        public int ChatRoomId { get; set; }

        public virtual ICollection<AppUser> AppUsers { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public ChatRoom()
        {
            this.AppUsers = new HashSet<AppUser>();
            this.Messages = new HashSet<Message>();
        }
    }

    public class Message
    {
        [Required]
        public int MessageId { get; set; }


        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public virtual AppUser Sender { get; set; }

        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }

        [ForeignKey("Recipient")]
        public string RecipientId { get; set; }
        public virtual AppUser Recipient { get; set; }
    }

}