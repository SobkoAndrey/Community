namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class groupImageId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Groups", name: "Image_ImageId1", newName: "ImageId");
            RenameIndex(table: "dbo.Groups", name: "IX_Image_ImageId1", newName: "IX_ImageId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Groups", name: "IX_ImageId", newName: "IX_Image_ImageId1");
            RenameColumn(table: "dbo.Groups", name: "ImageId", newName: "Image_ImageId1");
        }
    }
}
