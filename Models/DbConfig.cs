using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Community3.Models
{
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
                .WillCascadeOnDelete(false);

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