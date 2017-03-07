namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class df : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserAudios", newName: "AppUserAudios");
            RenameTable(name: "dbo.ImageUsers", newName: "ImageAppUsers");
            RenameColumn(table: "dbo.AppUserAudios", name: "User_Id", newName: "AppUser_Id");
            RenameColumn(table: "dbo.ImageAppUsers", name: "User_Id", newName: "AppUser_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "User_Id", newName: "AppUser_Id");
            RenameColumn(table: "dbo.Groups", name: "User_Id", newName: "AppUser_Id");
            RenameIndex(table: "dbo.Groups", name: "IX_User_Id", newName: "IX_AppUser_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_User_Id", newName: "IX_AppUser_Id");
            RenameIndex(table: "dbo.AppUserAudios", name: "IX_User_Id", newName: "IX_AppUser_Id");
            RenameIndex(table: "dbo.ImageAppUsers", name: "IX_User_Id", newName: "IX_AppUser_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ImageAppUsers", name: "IX_AppUser_Id", newName: "IX_User_Id");
            RenameIndex(table: "dbo.AppUserAudios", name: "IX_AppUser_Id", newName: "IX_User_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_AppUser_Id", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Groups", name: "IX_AppUser_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Groups", name: "AppUser_Id", newName: "User_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "AppUser_Id", newName: "User_Id");
            RenameColumn(table: "dbo.ImageAppUsers", name: "AppUser_Id", newName: "User_Id");
            RenameColumn(table: "dbo.AppUserAudios", name: "AppUser_Id", newName: "User_Id");
            RenameTable(name: "dbo.ImageAppUsers", newName: "ImageUsers");
            RenameTable(name: "dbo.AppUserAudios", newName: "UserAudios");
        }
    }
}
