namespace Community3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Group_GroupId", "dbo.Groups");
            DropIndex("dbo.Groups", new[] { "AppUser_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Group_GroupId" });
            CreateTable(
                "dbo.AppUsersGroups",
                c => new
                    {
                        GroupRefId = c.Int(nullable: false),
                        AppUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GroupRefId, t.AppUserId })
                .ForeignKey("dbo.Groups", t => t.GroupRefId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUserId, cascadeDelete: false)
                .Index(t => t.GroupRefId)
                .Index(t => t.AppUserId);
            
            DropColumn("dbo.Groups", "AppUser_Id");
            DropColumn("dbo.AspNetUsers", "Group_GroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Group_GroupId", c => c.Int());
            AddColumn("dbo.Groups", "AppUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.AppUsersGroups", "AppUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppUsersGroups", "GroupRefId", "dbo.Groups");
            DropIndex("dbo.AppUsersGroups", new[] { "AppUserId" });
            DropIndex("dbo.AppUsersGroups", new[] { "GroupRefId" });
            DropTable("dbo.AppUsersGroups");
            CreateIndex("dbo.AspNetUsers", "Group_GroupId");
            CreateIndex("dbo.Groups", "AppUser_Id");
            AddForeignKey("dbo.AspNetUsers", "Group_GroupId", "dbo.Groups", "GroupId");
            AddForeignKey("dbo.Groups", "AppUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
