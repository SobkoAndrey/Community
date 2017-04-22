namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audios",
                c => new
                    {
                        AudioId = c.Int(nullable: false, identity: true),
                        Path = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.AudioId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        ImageId = c.Int(),
                        OwnerId = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        AppUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.ImageId)
                .Index(t => t.OwnerId)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 15),
                        Surname = c.String(nullable: false, maxLength: 20),
                        Gender = c.Int(),
                        PhotoId = c.Int(),
                        Location = c.String(),
                        Description = c.String(),
                        Birthday = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        AppUser_Id = c.String(maxLength: 128),
                        Group_GroupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id)
                .ForeignKey("dbo.Images", t => t.PhotoId)
                .ForeignKey("dbo.Groups", t => t.Group_GroupId)
                .Index(t => t.PhotoId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.AppUser_Id)
                .Index(t => t.Group_GroupId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        Path = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Label = c.String(),
                        AppUserId = c.String(maxLength: 128),
                        GroupId = c.Int(),
                        AppUser_Id = c.String(maxLength: 128),
                        Group_GroupId = c.Int(),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUserId)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id)
                .ForeignKey("dbo.Groups", t => t.Group_GroupId)
                .Index(t => t.AppUserId)
                .Index(t => t.GroupId)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Group_GroupId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        Recipient_Id = c.String(maxLength: 128),
                        Sender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.AspNetUsers", t => t.Recipient_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id)
                .Index(t => t.Recipient_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AppUserAudios",
                c => new
                    {
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                        Audio_AudioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.Audio_AudioId })
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Audios", t => t.Audio_AudioId, cascadeDelete: true)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Audio_AudioId);
            
            CreateTable(
                "dbo.GroupAudios",
                c => new
                    {
                        Group_GroupId = c.Int(nullable: false),
                        Audio_AudioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_GroupId, t.Audio_AudioId })
                .ForeignKey("dbo.Groups", t => t.Group_GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Audios", t => t.Audio_AudioId, cascadeDelete: true)
                .Index(t => t.Group_GroupId)
                .Index(t => t.Audio_AudioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Messages", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Recipient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Groups", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Images", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "ImageId", "dbo.Images");
            DropForeignKey("dbo.GroupAudios", "Audio_AudioId", "dbo.Audios");
            DropForeignKey("dbo.GroupAudios", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.AspNetUsers", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "PhotoId", "dbo.Images");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Images", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Images", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Images", "AppUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Groups", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppUserAudios", "Audio_AudioId", "dbo.Audios");
            DropForeignKey("dbo.AppUserAudios", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.GroupAudios", new[] { "Audio_AudioId" });
            DropIndex("dbo.GroupAudios", new[] { "Group_GroupId" });
            DropIndex("dbo.AppUserAudios", new[] { "Audio_AudioId" });
            DropIndex("dbo.AppUserAudios", new[] { "AppUser_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Messages", new[] { "Sender_Id" });
            DropIndex("dbo.Messages", new[] { "Recipient_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Images", new[] { "Group_GroupId" });
            DropIndex("dbo.Images", new[] { "AppUser_Id" });
            DropIndex("dbo.Images", new[] { "GroupId" });
            DropIndex("dbo.Images", new[] { "AppUserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Group_GroupId" });
            DropIndex("dbo.AspNetUsers", new[] { "AppUser_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "PhotoId" });
            DropIndex("dbo.Groups", new[] { "AppUser_Id" });
            DropIndex("dbo.Groups", new[] { "OwnerId" });
            DropIndex("dbo.Groups", new[] { "ImageId" });
            DropTable("dbo.GroupAudios");
            DropTable("dbo.AppUserAudios");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Messages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Images");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Groups");
            DropTable("dbo.Audios");
        }
    }
}
