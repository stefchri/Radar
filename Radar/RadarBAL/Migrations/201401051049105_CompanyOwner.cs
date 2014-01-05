namespace RadarORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.companies", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.companies", "UserId");
            AddForeignKey("dbo.companies", "UserId", "dbo.users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.companies", "UserId", "dbo.users");
            DropIndex("dbo.companies", new[] { "UserId" });
            DropColumn("dbo.companies", "UserId");
        }
    }
}
