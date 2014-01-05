namespace RadarORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.companies", "Email", c => c.String(maxLength: 255));
            AddColumn("dbo.companies", "ActivatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.companies", "ActivatedDate");
            DropColumn("dbo.companies", "Email");
        }
    }
}
