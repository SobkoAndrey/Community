namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class models : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audios",
                c => new
                    {
                        AudioId = c.Int(nullable: false, identity: true),
                        Path = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.AudioId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreationDate = c.DateTime(nullable: false),
                        Owner_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id, cascadeDelete: true)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        Path = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ImageId);
            
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
            
            CreateTable(
                "dbo.ImageGroups",
                c => new
                    {
                        Image_ImageId = c.Int(nullable: false),
                        Group_GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Image_ImageId, t.Group_GroupId })
                .ForeignKey("dbo.Images", t => t.Image_ImageId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_GroupId, cascadeDelete: true)
                .Index(t => t.Image_ImageId)
                .Index(t => t.Group_GroupId);
            
            AddColumn("dbo.AspNetUsers", "Image_ImageId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Group_GroupId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Audio_AudioId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Image_ImageId");
            CreateIndex("dbo.AspNetUsers", "Group_GroupId");
            CreateIndex("dbo.AspNetUsers", "Audio_AudioId");
            AddForeignKey("dbo.AspNetUsers", "Image_ImageId", "dbo.Images", "ImageId");
            AddForeignKey("dbo.AspNetUsers", "Group_GroupId", "dbo.Groups", "GroupId");
            AddForeignKey("dbo.AspNetUsers", "Audio_AudioId", "dbo.Audios", "AudioId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Recipient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Audio_AudioId", "dbo.Audios");
            DropForeignKey("dbo.AspNetUsers", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Image_ImageId", "dbo.Images");
            DropForeignKey("dbo.ImageGroups", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.ImageGroups", "Image_ImageId", "dbo.Images");
            DropForeignKey("dbo.GroupAudios", "Audio_AudioId", "dbo.Audios");
            DropForeignKey("dbo.GroupAudios", "Group_GroupId", "dbo.Groups");
            DropIndex("dbo.ImageGroups", new[] { "Group_GroupId" });
            DropIndex("dbo.ImageGroups", new[] { "Image_ImageId" });
            DropIndex("dbo.GroupAudios", new[] { "Audio_AudioId" });
            DropIndex("dbo.GroupAudios", new[] { "Group_GroupId" });
            DropIndex("dbo.Messages", new[] { "Sender_Id" });
            DropIndex("dbo.Messages", new[] { "Recipient_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Audio_AudioId" });
            DropIndex("dbo.AspNetUsers", new[] { "Group_GroupId" });
            DropIndex("dbo.AspNetUsers", new[] { "Image_ImageId" });
            DropIndex("dbo.Groups", new[] { "Owner_Id" });
            DropColumn("dbo.AspNetUsers", "Audio_AudioId");
            DropColumn("dbo.AspNetUsers", "Group_GroupId");
            DropColumn("dbo.AspNetUsers", "Image_ImageId");
            DropTable("dbo.ImageGroups");
            DropTable("dbo.GroupAudios");
            DropTable("dbo.Messages");
            DropTable("dbo.Images");
            DropTable("dbo.Groups");
            DropTable("dbo.Audios");
        }
    }
}
