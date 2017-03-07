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
        Female
    }

    public class AppUser : IdentityUser
    {

        public Gender Gender { get; set; }
        public string Photo { get; set; }
        public string Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Audio> Audios { get; set; }
        public virtual ICollection<AppUser> Friends { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

        public AppUser()
        {
            this.Images = new HashSet<Image>();
            this.Audios = new HashSet<Audio>();
            this.Friends = new HashSet<AppUser>();
            this.Groups = new HashSet<Group>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class Group
    {
        [Required]
        public int GroupId { get; set; }

        [Required]
        [MaxLength(50), MinLength(1)]
        public string Name { get; set; }

        [Required]
        public AppUser Owner { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<AppUser> GroupUsers { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Audio> Audios { get; set; }

        public Group()
        {
            this.GroupUsers = new HashSet<AppUser>();
            this.Images = new HashSet<Image>();
            this.Audios = new HashSet<Audio>();
        }
    }

    public class Audio
    {
        [Required]
        public int AudioId { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        [MaxLength(50), MinLength(1)]
        public string Name { get; set; }

        public virtual ICollection<AppUser> OwnersUsers { get; set; }
        public virtual ICollection<Group> OwnersGroups { get; set; }

        public Audio()
        {
            this.OwnersUsers = new HashSet<AppUser>();
            this.OwnersGroups = new HashSet<Group>();
        }
    }

    public class Image
    {
        [Required]
        public int ImageId { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        [MaxLength(50), MinLength(1)]
        public string Name { get; set; }

        public virtual ICollection<AppUser> OwnersUsers { get; set; }
        public virtual ICollection<Group> OwnersGroups { get; set; }

        public Image()
        {
            this.OwnersUsers = new HashSet<AppUser>();
            this.OwnersGroups = new HashSet<Group>();
        }
    }

    public class Message
    {
        public int MessageId { get; set; }
        public AppUser Sender { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
        public AppUser Recipient { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<Image> Images { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}