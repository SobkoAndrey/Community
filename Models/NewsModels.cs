using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Community3.Models
{
    public class Post
    {
        [Required]
        public int PostId { get; set; }

        [Display(Name = "Заголовок")]
        [StringLength(50, ErrorMessage = "Заголовок новости не должен превышать 50 символов")]
        public string Name { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Текст")]
        [StringLength(2000, ErrorMessage = "Текст новости не должен превышать 2000 символов")]
        public string Text { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        [ForeignKey("Group")]
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public bool Confirm { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Audio> Audios { get; set; }
        public virtual ICollection<AppUser> AppUsersLiked { get; set; }

        public Post()
        {
            this.Images = new HashSet<Image>();
            this.Audios = new HashSet<Audio>();
            this.AppUsersLiked = new HashSet<AppUser>();
            this.Likes = new HashSet<Like>();
        }

        public List<string> GetUsersLikedIdList()
        {
            var list = new List<string>();
            foreach (var user in this.AppUsersLiked)
            {
                list.Add(user.Id);
            }
            return list;
        }

    }

    public class Like
    {
        [Required]
        public int LikeId { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public virtual Post Post { get; set; }
    }

}