namespace RadarORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RolechangeType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.roles", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.roles", "Type");
        }
    }
}
