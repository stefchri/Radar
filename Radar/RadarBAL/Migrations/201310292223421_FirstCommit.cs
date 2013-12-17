namespace RadarORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstCommit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Avatar = c.String(maxLength: 255),
                        LocationId = c.Long(nullable: false),
                        Description = c.String(nullable: false),
                        OpenHours = c.String(nullable: false),
                        Extra = c.String(nullable: false),
                        CompanyType = c.Int(nullable: false),
                        ParentId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.locations", t => t.LocationId)
                .ForeignKey("dbo.companies", t => t.ParentId)
                .Index(t => t.LocationId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.companies", t => t.CompanyId)
                .ForeignKey("dbo.roles", t => t.RoleId)
                .ForeignKey("dbo.users", t => t.UserId)
                .Index(t => t.CompanyId)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.posts",
                c => new
                    {
                        PostId = c.Long(nullable: false, identity: true),
                        Body = c.String(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255),
                        SubTitle = c.String(nullable: false, maxLength: 255),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                        CompanyId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.companies", t => t.CompanyId)
                .ForeignKey("dbo.employees", t => t.EmployeeId)
                .Index(t => t.CompanyId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 255),
                        Email = c.String(nullable: false, maxLength: 255),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false, maxLength: 255),
                        Avatar = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 255),
                        Salt = c.String(nullable: false, maxLength: 255),
                        Bio = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                        LocationId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.locations", t => t.LocationId)
                .ForeignKey("dbo.roles", t => t.RoleId)
                .Index(t => t.LocationId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.comments",
                c => new
                    {
                        CommentId = c.Long(nullable: false, identity: true),
                        Body = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                        UserId = c.Int(nullable: false),
                        ParentId = c.Long(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.comments", t => t.ParentId)
                .ForeignKey("dbo.users", t => t.UserId)
                .Index(t => t.ParentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.locations",
                c => new
                    {
                        LocationId = c.Long(nullable: false, identity: true),
                        Latitude = c.Decimal(nullable: false, precision: 13, scale: 9),
                        Longitude = c.Decimal(nullable: false, precision: 13, scale: 9),
                        Street = c.String(nullable: false, maxLength: 255),
                        Number = c.String(nullable: false, maxLength: 255),
                        Box = c.String(nullable: false, maxLength: 255),
                        Zipcode = c.String(nullable: false, maxLength: 255),
                        City = c.String(nullable: false, maxLength: 255),
                        Country = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.notifications",
                c => new
                    {
                        NotificationId = c.Long(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Viewed = c.DateTime(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ratings",
                c => new
                    {
                        RatingId = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RatingId)
                .ForeignKey("dbo.users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.messages",
                c => new
                    {
                        MessageId = c.Long(nullable: false, identity: true),
                        Body = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                        SenderId = c.Int(nullable: false),
                        RecieverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.users", t => t.RecieverId)
                .ForeignKey("dbo.users", t => t.SenderId)
                .Index(t => t.RecieverId)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.companies_have_categories",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.CategoryId })
                .ForeignKey("dbo.companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.users_interestedin_categories",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CategoryId })
                .ForeignKey("dbo.users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.users_follow_companies",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CompanyId })
                .ForeignKey("dbo.users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.users_beento_locations",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        LocationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.LocationId })
                .ForeignKey("dbo.users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.users_liked_posts",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        PostId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PostId })
                .ForeignKey("dbo.users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.users_follow_users",
                c => new
                    {
                        FollowingId = c.Int(nullable: false),
                        FolloweeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FollowingId, t.FolloweeId })
                .ForeignKey("dbo.users", t => t.FollowingId)
                .ForeignKey("dbo.users", t => t.FolloweeId)
                .Index(t => t.FollowingId)
                .Index(t => t.FolloweeId);
            
            CreateTable(
                "dbo.companies_have_ratings",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        RatingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.RatingId })
                .ForeignKey("dbo.companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.ratings", t => t.RatingId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.RatingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.companies_have_ratings", "RatingId", "dbo.ratings");
            DropForeignKey("dbo.companies_have_ratings", "CompanyId", "dbo.companies");
            DropForeignKey("dbo.companies", "ParentId", "dbo.companies");
            DropForeignKey("dbo.companies", "LocationId", "dbo.locations");
            DropForeignKey("dbo.employees", "UserId", "dbo.users");
            DropForeignKey("dbo.employees", "RoleId", "dbo.roles");
            DropForeignKey("dbo.users_follow_users", "FolloweeId", "dbo.users");
            DropForeignKey("dbo.users_follow_users", "FollowingId", "dbo.users");
            DropForeignKey("dbo.users", "RoleId", "dbo.roles");
            DropForeignKey("dbo.messages", "SenderId", "dbo.users");
            DropForeignKey("dbo.messages", "RecieverId", "dbo.users");
            DropForeignKey("dbo.ratings", "UserId", "dbo.users");
            DropForeignKey("dbo.users_liked_posts", "PostId", "dbo.posts");
            DropForeignKey("dbo.users_liked_posts", "UserId", "dbo.users");
            DropForeignKey("dbo.notifications", "UserId", "dbo.users");
            DropForeignKey("dbo.users_beento_locations", "LocationId", "dbo.locations");
            DropForeignKey("dbo.users_beento_locations", "UserId", "dbo.users");
            DropForeignKey("dbo.users", "LocationId", "dbo.locations");
            DropForeignKey("dbo.users_follow_companies", "CompanyId", "dbo.companies");
            DropForeignKey("dbo.users_follow_companies", "UserId", "dbo.users");
            DropForeignKey("dbo.comments", "UserId", "dbo.users");
            DropForeignKey("dbo.comments", "ParentId", "dbo.comments");
            DropForeignKey("dbo.users_interestedin_categories", "CategoryId", "dbo.categories");
            DropForeignKey("dbo.users_interestedin_categories", "UserId", "dbo.users");
            DropForeignKey("dbo.posts", "EmployeeId", "dbo.employees");
            DropForeignKey("dbo.posts", "CompanyId", "dbo.companies");
            DropForeignKey("dbo.employees", "CompanyId", "dbo.companies");
            DropForeignKey("dbo.companies_have_categories", "CategoryId", "dbo.categories");
            DropForeignKey("dbo.companies_have_categories", "CompanyId", "dbo.companies");
            DropIndex("dbo.companies_have_ratings", new[] { "RatingId" });
            DropIndex("dbo.companies_have_ratings", new[] { "CompanyId" });
            DropIndex("dbo.companies", new[] { "ParentId" });
            DropIndex("dbo.companies", new[] { "LocationId" });
            DropIndex("dbo.employees", new[] { "UserId" });
            DropIndex("dbo.employees", new[] { "RoleId" });
            DropIndex("dbo.users_follow_users", new[] { "FolloweeId" });
            DropIndex("dbo.users_follow_users", new[] { "FollowingId" });
            DropIndex("dbo.users", new[] { "RoleId" });
            DropIndex("dbo.messages", new[] { "SenderId" });
            DropIndex("dbo.messages", new[] { "RecieverId" });
            DropIndex("dbo.ratings", new[] { "UserId" });
            DropIndex("dbo.users_liked_posts", new[] { "PostId" });
            DropIndex("dbo.users_liked_posts", new[] { "UserId" });
            DropIndex("dbo.notifications", new[] { "UserId" });
            DropIndex("dbo.users_beento_locations", new[] { "LocationId" });
            DropIndex("dbo.users_beento_locations", new[] { "UserId" });
            DropIndex("dbo.users", new[] { "LocationId" });
            DropIndex("dbo.users_follow_companies", new[] { "CompanyId" });
            DropIndex("dbo.users_follow_companies", new[] { "UserId" });
            DropIndex("dbo.comments", new[] { "UserId" });
            DropIndex("dbo.comments", new[] { "ParentId" });
            DropIndex("dbo.users_interestedin_categories", new[] { "CategoryId" });
            DropIndex("dbo.users_interestedin_categories", new[] { "UserId" });
            DropIndex("dbo.posts", new[] { "EmployeeId" });
            DropIndex("dbo.posts", new[] { "CompanyId" });
            DropIndex("dbo.employees", new[] { "CompanyId" });
            DropIndex("dbo.companies_have_categories", new[] { "CategoryId" });
            DropIndex("dbo.companies_have_categories", new[] { "CompanyId" });
            DropTable("dbo.companies_have_ratings");
            DropTable("dbo.users_follow_users");
            DropTable("dbo.users_liked_posts");
            DropTable("dbo.users_beento_locations");
            DropTable("dbo.users_follow_companies");
            DropTable("dbo.users_interestedin_categories");
            DropTable("dbo.companies_have_categories");
            DropTable("dbo.roles");
            DropTable("dbo.messages");
            DropTable("dbo.ratings");
            DropTable("dbo.notifications");
            DropTable("dbo.locations");
            DropTable("dbo.comments");
            DropTable("dbo.users");
            DropTable("dbo.posts");
            DropTable("dbo.employees");
            DropTable("dbo.companies");
            DropTable("dbo.categories");
        }
    }
}
