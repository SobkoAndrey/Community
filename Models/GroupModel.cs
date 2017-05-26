using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Community3.Models
{
    public class Group
    {
        [Required]
        public int GroupId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        [Required]
        public virtual AppUser Owner { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }
        public virtual ICollection<AppUser> BlockUsers { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Audio> Audios { get; set; }

        public Group()
        {
            this.AppUsers = new HashSet<AppUser>();
            this.Images = new HashSet<Image>();
            this.Audios = new HashSet<Audio>();
            this.Posts = new HashSet<Post>();
            this.BlockUsers = new HashSet<AppUser>();
        }
    }
}