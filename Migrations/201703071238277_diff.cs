namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class diff : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserImages", newName: "ImageApplicationUsers");
            DropPrimaryKey("dbo.ImageApplicationUsers");
            AddPrimaryKey("dbo.ImageApplicationUsers", new[] { "Image_ImageId", "ApplicationUser_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ImageApplicationUsers");
            AddPrimaryKey("dbo.ImageApplicationUsers", new[] { "ApplicationUser_Id", "Image_ImageId" });
            RenameTable(name: "dbo.ImageApplicationUsers", newName: "ApplicationUserImages");
        }
    }
}
