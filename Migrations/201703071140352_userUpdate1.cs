namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userUpdate1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Image_ImageId", "dbo.Images");
            DropForeignKey("dbo.AspNetUsers", "Audio_AudioId", "dbo.Audios");
            DropIndex("dbo.AspNetUsers", new[] { "Image_ImageId" });
            DropIndex("dbo.AspNetUsers", new[] { "Audio_AudioId" });
            CreateTable(
                "dbo.ApplicationUserAudios",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Audio_AudioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Audio_AudioId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Audios", t => t.Audio_AudioId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Audio_AudioId);
            
            CreateTable(
                "dbo.ApplicationUserImages",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Image_ImageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Image_ImageId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Images", t => t.Image_ImageId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Image_ImageId);
            
            AddColumn("dbo.Groups", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Groups", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Groups", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "Image_ImageId");
            DropColumn("dbo.AspNetUsers", "Audio_AudioId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Audio_AudioId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Image_ImageId", c => c.Int());
            DropForeignKey("dbo.ApplicationUserImages", "Image_ImageId", "dbo.Images");
            DropForeignKey("dbo.ApplicationUserImages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Groups", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserAudios", "Audio_AudioId", "dbo.Audios");
            DropForeignKey("dbo.ApplicationUserAudios", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserImages", new[] { "Image_ImageId" });
            DropIndex("dbo.ApplicationUserImages", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserAudios", new[] { "Audio_AudioId" });
            DropIndex("dbo.ApplicationUserAudios", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Groups", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "ApplicationUser_Id");
            DropColumn("dbo.Groups", "ApplicationUser_Id");
            DropTable("dbo.ApplicationUserImages");
            DropTable("dbo.ApplicationUserAudios");
            CreateIndex("dbo.AspNetUsers", "Audio_AudioId");
            CreateIndex("dbo.AspNetUsers", "Image_ImageId");
            AddForeignKey("dbo.AspNetUsers", "Audio_AudioId", "dbo.Audios", "AudioId");
            AddForeignKey("dbo.AspNetUsers", "Image_ImageId", "dbo.Images", "ImageId");
        }
    }
}
