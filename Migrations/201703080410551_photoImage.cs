namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photoImage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ImageAppUsers", "Image_ImageId", "dbo.Images");
            DropForeignKey("dbo.ImageAppUsers", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ImageAppUsers", new[] { "Image_ImageId" });
            DropIndex("dbo.ImageAppUsers", new[] { "AppUser_Id" });
            AddColumn("dbo.AspNetUsers", "Image_ImageId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Photo_ImageId", c => c.Int());
            AddColumn("dbo.Images", "AppUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Image_ImageId");
            CreateIndex("dbo.AspNetUsers", "Photo_ImageId");
            CreateIndex("dbo.Images", "AppUser_Id");
            AddForeignKey("dbo.AspNetUsers", "Image_ImageId", "dbo.Images", "ImageId");
            AddForeignKey("dbo.Images", "AppUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Photo_ImageId", "dbo.Images", "ImageId");
            DropColumn("dbo.AspNetUsers", "Photo");
            DropTable("dbo.ImageAppUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ImageAppUsers",
                c => new
                    {
                        Image_ImageId = c.Int(nullable: false),
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Image_ImageId, t.AppUser_Id });
            
            AddColumn("dbo.AspNetUsers", "Photo", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "Photo_ImageId", "dbo.Images");
            DropForeignKey("dbo.Images", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Image_ImageId", "dbo.Images");
            DropIndex("dbo.Images", new[] { "AppUser_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Photo_ImageId" });
            DropIndex("dbo.AspNetUsers", new[] { "Image_ImageId" });
            DropColumn("dbo.Images", "AppUser_Id");
            DropColumn("dbo.AspNetUsers", "Photo_ImageId");
            DropColumn("dbo.AspNetUsers", "Image_ImageId");
            CreateIndex("dbo.ImageAppUsers", "AppUser_Id");
            CreateIndex("dbo.ImageAppUsers", "Image_ImageId");
            AddForeignKey("dbo.ImageAppUsers", "AppUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ImageAppUsers", "Image_ImageId", "dbo.Images", "ImageId", cascadeDelete: true);
        }
    }
}
