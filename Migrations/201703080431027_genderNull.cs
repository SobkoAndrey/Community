namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class genderNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: false));
        }
    }
}
