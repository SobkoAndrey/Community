namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeImage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Images", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Images", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
