using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Community3.Models
{

    public enum Gender
    {
        Male,
        Female,
        Empty
    }

    public class AppUser : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }

        public Gender Gender { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Audio> Audios { get; set; }
        public virtual ICollection<AppUser> Friends { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<ChatRoom> ChatRooms { get; set; }
        public virtual ICollection<AppUser> Candidates { get; set; }

        public AppUser()
        {
            this.Images = new HashSet<Image>();
            this.Audios = new HashSet<Audio>();
            this.Friends = new HashSet<AppUser>();
            this.Groups = new HashSet<Group>();
            this.Posts = new HashSet<Post>();
            this.ChatRooms = new HashSet<ChatRoom>();
            this.Candidates = new HashSet<AppUser>();
        }
    }
}