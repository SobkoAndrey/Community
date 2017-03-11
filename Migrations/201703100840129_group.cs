namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class group : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ImageGroups", "Image_ImageId", "dbo.Images");
            DropForeignKey("dbo.ImageGroups", "Group_GroupId", "dbo.Groups");
            DropIndex("dbo.ImageGroups", new[] { "Image_ImageId" });
            DropIndex("dbo.ImageGroups", new[] { "Group_GroupId" });
            AddColumn("dbo.Groups", "Description", c => c.String());
            AddColumn("dbo.Groups", "Image_ImageId", c => c.Int());
            AddColumn("dbo.Groups", "Image_ImageId1", c => c.Int());
            AddColumn("dbo.Images", "Group_GroupId", c => c.Int());
            CreateIndex("dbo.Groups", "Image_ImageId");
            CreateIndex("dbo.Groups", "Image_ImageId1");
            CreateIndex("dbo.Images", "Group_GroupId");
            AddForeignKey("dbo.Groups", "Image_ImageId", "dbo.Images", "ImageId");
            AddForeignKey("dbo.Groups", "Image_ImageId1", "dbo.Images", "ImageId");
            AddForeignKey("dbo.Images", "Group_GroupId", "dbo.Groups", "GroupId");
            DropTable("dbo.ImageGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ImageGroups",
                c => new
                    {
                        Image_ImageId = c.Int(nullable: false),
                        Group_GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Image_ImageId, t.Group_GroupId });
            
            DropForeignKey("dbo.Images", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "Image_ImageId1", "dbo.Images");
            DropForeignKey("dbo.Groups", "Image_ImageId", "dbo.Images");
            DropIndex("dbo.Images", new[] { "Group_GroupId" });
            DropIndex("dbo.Groups", new[] { "Image_ImageId1" });
            DropIndex("dbo.Groups", new[] { "Image_ImageId" });
            DropColumn("dbo.Images", "Group_GroupId");
            DropColumn("dbo.Groups", "Image_ImageId1");
            DropColumn("dbo.Groups", "Image_ImageId");
            DropColumn("dbo.Groups", "Description");
            CreateIndex("dbo.ImageGroups", "Group_GroupId");
            CreateIndex("dbo.ImageGroups", "Image_ImageId");
            AddForeignKey("dbo.ImageGroups", "Group_GroupId", "dbo.Groups", "GroupId", cascadeDelete: true);
            AddForeignKey("dbo.ImageGroups", "Image_ImageId", "dbo.Images", "ImageId", cascadeDelete: true);
        }
    }
}
