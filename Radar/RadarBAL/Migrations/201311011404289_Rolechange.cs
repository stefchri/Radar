namespace RadarORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rolechange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.users", "RoleId", "dbo.roles");
            DropIndex("dbo.users", new[] { "RoleId" });
            CreateTable(
                "dbo.users_have_roles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            AddColumn("dbo.users", "LockedDate", c => c.DateTime());
            AddColumn("dbo.users", "ApprovedDate", c => c.DateTime());
            AlterColumn("dbo.users", "Avatar", c => c.String(maxLength: 255));
            DropColumn("dbo.users", "RoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.users", "RoleId", c => c.Int(nullable: false));
            DropForeignKey("dbo.users_have_roles", "RoleId", "dbo.roles");
            DropForeignKey("dbo.users_have_roles", "UserId", "dbo.users");
            DropIndex("dbo.users_have_roles", new[] { "RoleId" });
            DropIndex("dbo.users_have_roles", new[] { "UserId" });
            AlterColumn("dbo.users", "Avatar", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.users", "ApprovedDate");
            DropColumn("dbo.users", "LockedDate");
            DropTable("dbo.users_have_roles");
            CreateIndex("dbo.users", "RoleId");
            AddForeignKey("dbo.users", "RoleId", "dbo.roles", "RoleId");
        }
    }
}
