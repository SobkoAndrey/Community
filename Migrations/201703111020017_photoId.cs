namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photoId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PhotoId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PhotoId");
        }
    }
}
