namespace RadarORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBla : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.users", "Bio", c => c.String(maxLength: 500));
            AlterColumn("dbo.locations", "Box", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.locations", "Box", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.users", "Bio", c => c.String());
        }
    }
}
