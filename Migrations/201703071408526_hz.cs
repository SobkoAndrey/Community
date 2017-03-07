namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hz : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserAudios", newName: "UserAudios");
            RenameTable(name: "dbo.ImageApplicationUsers", newName: "ImageUsers");
            RenameColumn(table: "dbo.UserAudios", name: "ApplicationUser_Id", newName: "User_Id");
            RenameColumn(table: "dbo.ImageUsers", name: "ApplicationUser_Id", newName: "User_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "ApplicationUser_Id", newName: "User_Id");
            RenameColumn(table: "dbo.Groups", name: "ApplicationUser_Id", newName: "User_Id");
            RenameIndex(table: "dbo.Groups", name: "IX_ApplicationUser_Id", newName: "IX_User_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_ApplicationUser_Id", newName: "IX_User_Id");
            RenameIndex(table: "dbo.UserAudios", name: "IX_ApplicationUser_Id", newName: "IX_User_Id");
            RenameIndex(table: "dbo.ImageUsers", name: "IX_ApplicationUser_Id", newName: "IX_User_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ImageUsers", name: "IX_User_Id", newName: "IX_ApplicationUser_Id");
            RenameIndex(table: "dbo.UserAudios", name: "IX_User_Id", newName: "IX_ApplicationUser_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_User_Id", newName: "IX_ApplicationUser_Id");
            RenameIndex(table: "dbo.Groups", name: "IX_User_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Groups", name: "User_Id", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "User_Id", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.ImageUsers", name: "User_Id", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.UserAudios", name: "User_Id", newName: "ApplicationUser_Id");
            RenameTable(name: "dbo.ImageUsers", newName: "ImageApplicationUsers");
            RenameTable(name: "dbo.UserAudios", newName: "ApplicationUserAudios");
        }
    }
}
