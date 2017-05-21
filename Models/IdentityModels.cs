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
        [Required]
        [MaxLength(15), MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Surname { get; set; }

        public string FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }

        public Gender Gender { get; set; }

        //[ForeignKey("Photo")]
        //public int? PhotoId { get; set; }
        //public virtual Image Photo { get; set; }

        public string Location { get; set; }

        [UIHint("MultilineText")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [RegularExpression(@"\d\d\.\d\d\.\d\d\d\d", ErrorMessage = "Некорректная дата")]
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
        [Display(Name = "Название")]
        public string Name { get; set; }

        [UIHint("MultilineText")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        //[ForeignKey("Image")]
        //public int? ImageId { get; set; }

        //public virtual Image Image { get; set; }

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

    public class Audio
    {
        [Required]
        public int AudioId { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string Name { get; set; }

        public string Label { get; set; }

        
        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public virtual Post Post { get; set; }

        //public virtual ICollection<AppUser> OwnersUsers { get; set; }
        //public virtual ICollection<Group> OwnersGroups { get; set; }

        //public Audio()
        //{
        //    this.OwnersUsers = new HashSet<AppUser>();
        //    this.OwnersGroups = new HashSet<Group>();
        //}
    }

    public class Image
    {
        [Required]
        public int ImageId { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string Name { get; set; }

        public string Label { get; set; }

        //[ForeignKey("AppUser")]
        //public string AppUserId { get; set; }
        //public virtual AppUser AppUser { get; set; }

        //[ForeignKey("Group")]
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public virtual Post Post { get; set; }

        //public virtual ICollection<AppUser> AppUsers { get; set; }
    }

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

    public class Post
    {
        [Required]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string Name { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        [UIHint("MultilineText")]
        public string Text { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        [ForeignKey("Group")]
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

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

    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Group>()
                .HasMany<AppUser>(_ => _.AppUsers)
                .WithMany(_ => _.Groups)
                .Map(_ =>
                {
                    _.MapLeftKey("GroupRefId");
                    _.MapRightKey("AppUserId");
                    _.ToTable("AppUsersGroups");
                });

            modelBuilder.Entity<AppUser>()
                .HasMany(_ => _.Friends)
                .WithMany();

            modelBuilder.Entity<AppUser>()
                .HasMany(_ => _.Candidates)
                .WithMany()
                .Map(_ =>
                {
                    _.MapLeftKey("AppUserRefId");
                    _.MapRightKey("CandidateAppUserId");
                    _.ToTable("AppUsersCandidates");
                });

            modelBuilder.Entity<AppUser>()
                .HasMany(_ => _.Images)
                .WithOptional()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<AppUser>()
                .HasMany(_ => _.Audios)
                .WithOptional()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Group>()
                .HasMany(_ => _.Images)
                .WithOptional()
                .HasForeignKey(_ => _.GroupId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Like>()
                .HasOptional(_ => _.AppUser)
                .WithMany()
                .WillCascadeOnDelete();

                
        }

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