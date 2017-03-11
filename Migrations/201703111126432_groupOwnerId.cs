namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class groupOwnerId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Groups", name: "Owner_Id", newName: "OwnerId");
            RenameIndex(table: "dbo.Groups", name: "IX_Owner_Id", newName: "IX_OwnerId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Groups", name: "IX_OwnerId", newName: "IX_Owner_Id");
            RenameColumn(table: "dbo.Groups", name: "OwnerId", newName: "Owner_Id");
        }
    }
}
