namespace RadarORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubtitleNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.posts", "SubTitle", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.posts", "SubTitle", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
