using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadarModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadarBAL.ORM
{
    public class RadarContext : DbContext
    {
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<Company> Companies { get; set; }
        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<Location> Locations { get; set; }
        public IDbSet<Message> Messages { get; set; }
        public IDbSet<Notification> Notifications { get; set; }
        public IDbSet<Post> Posts { get; set; }
        public IDbSet<Rating> Ratings { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            //PLURAL REMOVAL
            mb.Conventions.Remove<PluralizingTableNameConvention>();
            
            //Category
            mb.Entity<Category>().HasKey(d => d.CategoryId);
            mb.Entity<Category>()
                .Property(a => a.CategoryId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<Category>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<Category>().ToTable("categories");

            //Comment
            mb.Entity<Comment>().HasKey(d => d.CommentId);
            mb.Entity<Comment>()
                .Property(a => a.CommentId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<Comment>()
                .Property(a => a.Body)
                .IsRequired()
                .IsMaxLength();
            mb.Entity<Comment>()
                .Property(a => a.CreatedDate)
                .IsRequired();
            mb.Entity<Comment>()
                .Property(a => a.ModifiedDate)
                .IsOptional();
            mb.Entity<Comment>()
                .Property(a => a.DeletedDate)
                .IsOptional();
            mb.Entity<Comment>()
                .Property(c => c.UserId)
                .IsRequired();
            mb.Entity<Comment>()
                .HasRequired(c => c.User)
                .WithMany(c => c.CommentsPosted)
                .HasForeignKey(s => s.UserId).WillCascadeOnDelete(false);
            mb.Entity<Comment>()
                .Property(c => c.ParentId)
                .IsOptional();
            mb.Entity<Comment>()
                .HasOptional(a => a.Parent)
                .WithMany(a => a.Children)
                .HasForeignKey(s => s.ParentId).WillCascadeOnDelete(false);
            mb.Entity<Comment>().ToTable("comments");

            //Company
            mb.Entity<Company>().HasKey(d => d.CompanyId);
            mb.Entity<Company>()
                .Property(a => a.CompanyId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<Company>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<Company>()
                .Property(a => a.Avatar)
                .IsOptional()
                .HasMaxLength(255);
            mb.Entity<Company>()
                .Property(a => a.LocationId)
                .IsRequired();
            mb.Entity<Company>()
                .Property(a => a.Description)
                .IsRequired()
                .IsMaxLength();
            mb.Entity<Company>()
                .Property(a => a.OpenHours)
                .IsRequired()
                .IsMaxLength();
            mb.Entity<Company>()
                .Property(a => a.Extra)
                .IsRequired()
                .IsMaxLength();
            mb.Entity<Company>()
                .Property(a => a.CompanyType)
                .IsRequired();
            mb.Entity<Company>()
                .Property(a => a.ParentId)
                .IsOptional();
            mb.Entity<Company>()
                .Property(a => a.CreatedDate)
                .IsRequired();
            mb.Entity<Company>()
                .Property(a => a.ModifiedDate)
                .IsOptional();
            mb.Entity<Company>()
                .Property(a => a.DeletedDate)
                .IsOptional();
            mb.Entity<Company>()
                .HasRequired(a => a.Location)
                .WithMany(a => a.Companies)
                .HasForeignKey(s => s.LocationId).WillCascadeOnDelete(false);
            mb.Entity<Company>()
               .HasMany(a => a.Categories)
               .WithMany(a => a.Companies)
               .Map(m =>
               {
                   m.MapLeftKey("CompanyId");
                   m.MapRightKey("CategoryId");
                   m.ToTable("companies_have_categories");
               }
            );
            mb.Entity<Company>()
                .HasMany(a => a.Ratings)
                .WithMany(a => a.Companies)
                .Map(m =>
                {
                    m.MapLeftKey("CompanyId");
                    m.MapRightKey("RatingId");
                    m.ToTable("companies_have_ratings");
                }
             );
            mb.Entity<Company>()
                .HasOptional(a => a.Parent)
                .WithMany(a => a.Branches)
                .HasForeignKey(s => s.ParentId).WillCascadeOnDelete(false);
            mb.Entity<Company>().ToTable("companies");

            //Employee
            mb.Entity<Employee>().HasKey(d => d.EmployeeId);
            mb.Entity<Employee>()
                .Property(a => a.EmployeeId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<Employee>()
                .Property(a => a.CompanyId)
                .IsRequired();
            mb.Entity<Employee>()
                .Property(a => a.UserId)
                .IsRequired();
            mb.Entity<Employee>()
                .Property(a => a.RoleId)
                .IsRequired(); 
            mb.Entity<Employee>()
                .Property(a => a.CreatedDate)
                .IsRequired();
            mb.Entity<Employee>()
                .Property(a => a.ModifiedDate)
                .IsOptional();
            mb.Entity<Employee>()
                .Property(a => a.DeletedDate)
                .IsOptional();
            mb.Entity<Employee>()
                .HasRequired(a => a.Role)
                .WithMany(a => a.Employees)
                .HasForeignKey(s => s.RoleId).WillCascadeOnDelete(false);
            mb.Entity<Employee>()
                .HasRequired(a => a.User)
                .WithMany(a => a.Employees)
                .HasForeignKey(s => s.UserId).WillCascadeOnDelete(false);
            mb.Entity<Employee>()
                .HasRequired(a => a.Company)
                .WithMany(a => a.Employees)
                .HasForeignKey(s => s.CompanyId).WillCascadeOnDelete(false);
            mb.Entity<Employee>().ToTable("employees");

            //Locations
            mb.Entity<Location>().HasKey(d => d.LocationId);
            mb.Entity<Location>()
                .Property(a => a.LocationId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<Location>()
                .Property(a => a.Latitude)
                .IsRequired().HasPrecision(13,9);
            mb.Entity<Location>()
                .Property(a => a.Longitude)
                .IsRequired().HasPrecision(13, 9);
            mb.Entity<Location>()
                .Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<Location>()
                .Property(a => a.Number)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<Location>()
                .Property(a => a.Box)
                .HasMaxLength(255);
            mb.Entity<Location>()
                .Property(a => a.Zipcode)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<Location>()
                .Property(a => a.City)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<Location>()
                .Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<Location>().ToTable("locations");

            //Message
            mb.Entity<Message>().HasKey(d => d.MessageId);
            mb.Entity<Message>()
                .Property(a => a.MessageId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<Message>()
                .Property(a => a.Body)
                .IsRequired()
                .IsMaxLength();
            mb.Entity<Message>()
                .Property(a => a.SenderId)
                .IsRequired();
            mb.Entity<Message>()
                .Property(a => a.RecieverId)
                .IsRequired();
            mb.Entity<Message>()
                .Property(a => a.CreatedDate)
                .IsRequired();
            mb.Entity<Message>()
                .Property(a => a.ModifiedDate)
                .IsOptional();
            mb.Entity<Message>()
                .Property(a => a.DeletedDate)
                .IsOptional();
            mb.Entity<Message>()
                .HasRequired(a => a.Sender)
                .WithMany(a => a.SentMessages)
                .HasForeignKey(s => s.SenderId).WillCascadeOnDelete(false);
            mb.Entity<Message>()
                .HasRequired(a => a.Reviever)
                .WithMany(a => a.RecievedMessages)
                .HasForeignKey(s => s.RecieverId).WillCascadeOnDelete(false);
            mb.Entity<Message>().ToTable("messages");

            //Notification
            mb.Entity<Notification>().HasKey(d => d.NotificationId);
            mb.Entity<Notification>()
                .Property(a => a.NotificationId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<Notification>()
                .Property(a => a.Viewed)
                .IsOptional();
            mb.Entity<Notification>()
                .Property(a => a.UserId)
                .IsRequired();
            mb.Entity<Notification>()
                .HasRequired(a => a.User)
                .WithMany(a => a.Notifications)
                .HasForeignKey(s => s.UserId).WillCascadeOnDelete(false);
            mb.Entity<Notification>().ToTable("notifications");

            //Post
            mb.Entity<Post>().HasKey(d => d.PostId);
            mb.Entity<Post>()
                .Property(a => a.PostId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<Post>()
                .Property(a => a.Body)
                .IsRequired()
                .IsMaxLength();
            mb.Entity<Post>()
                .Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<Post>()
                .Property(a => a.SubTitle)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<Post>()
                .Property(a => a.CreatedDate)
                .IsRequired();
            mb.Entity<Post>()
                .Property(a => a.ModifiedDate)
                .IsOptional();
            mb.Entity<Post>()
                .Property(a => a.DeletedDate)
                .IsOptional();
            mb.Entity<Post>()
                .Property(a => a.CompanyId)
                .IsRequired();
            mb.Entity<Post>()
                .Property(a => a.EmployeeId)
                .IsRequired();
            mb.Entity<Post>()
                .HasRequired(a => a.Company)
                .WithMany(a => a.Posts)
                .HasForeignKey(s => s.CompanyId).WillCascadeOnDelete(false);
            mb.Entity<Post>()
                .HasRequired(a => a.Employee)
                .WithMany(a => a.Posts)
                .HasForeignKey(s => s.EmployeeId).WillCascadeOnDelete(false);
            mb.Entity<Post>().ToTable("posts");

            //Rating
            mb.Entity<Rating>().HasKey(d => d.RatingId);
            mb.Entity<Rating>()
                .Property(a => a.RatingId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<Rating>()
                .Property(a => a.Score)
                .IsRequired();
            mb.Entity<Rating>()
                .Property(a => a.UserId)
                .IsRequired();
            mb.Entity<Rating>()
                .HasRequired(a => a.User)
                .WithMany(a => a.Ratings)
                .HasForeignKey(s => s.UserId).WillCascadeOnDelete(false);
            mb.Entity<Rating>().ToTable("ratings");

            //Role
            mb.Entity<Role>().HasKey(d => d.RoleId);
            mb.Entity<Role>()
                .Property(a => a.RoleId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<Role>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<Role>()
                .Property(a => a.Type)
                .IsRequired();
            mb.Entity<Role>().ToTable("roles");

            //User
            mb.Entity<User>().HasKey(d => d.UserId);
            mb.Entity<User>()
                .Property(a => a.UserId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mb.Entity<User>()
                .Property(a => a.Username)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<User>()
                .Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<User>()
                .Property(a => a.Password)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<User>()
                .Property(a => a.Salt)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<User>()
                .Property(a => a.DateOfBirth)
                .IsRequired();
            mb.Entity<User>()
                .Property(a => a.Avatar)
                .IsOptional()
                .HasMaxLength(255);
            mb.Entity<User>()
                .Property(a => a.Bio)
                .IsOptional()
                .HasMaxLength(500);
            mb.Entity<User>()
                .Property(a => a.Gender)
                .IsRequired()
                .HasMaxLength(255);
            mb.Entity<User>()
                .Property(a => a.CreatedDate)
                .IsRequired();
            mb.Entity<User>()
                .Property(a => a.ModifiedDate)
                .IsOptional();
            mb.Entity<User>()
                .Property(a => a.DeletedDate)
                .IsOptional();
            mb.Entity<User>()
                .Property(a => a.LockedDate)
                .IsOptional();
            mb.Entity<User>()
                .Property(a => a.ApprovedDate)
                .IsOptional();
            mb.Entity<User>()
                .Property(a => a.LocationId)
                .IsRequired();
            mb.Entity<User>()
                .HasRequired(a => a.Location)
                .WithMany(a => a.Users)
                .HasForeignKey(s => s.LocationId).WillCascadeOnDelete(false);
            mb.Entity<User>()
                .HasMany(a => a.Roles)
                .WithMany(a => a.Users)
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                    m.ToTable("users_have_roles");
                }
            );
            mb.Entity<User>()
                .HasMany(a => a.CompaniesFollowing)
                .WithMany(a => a.UsersFollowing)
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("CompanyId");
                    m.ToTable("users_follow_companies");
                }
             );
            mb.Entity<User>()
                .HasMany(a => a.UsersFollowing)
                .WithMany(a => a.FollowingUsers)
                .Map(m =>
                {
                    m.MapLeftKey("FollowingId");
                    m.MapRightKey("FolloweeId");
                    m.ToTable("users_follow_users");
                }
             );
            mb.Entity<User>()
                .HasMany(a => a.CategoriesInterestedIn)
                .WithMany(a => a.UsersInterestedIn)
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("CategoryId");
                    m.ToTable("users_interestedin_categories");
                }
             );
            mb.Entity<User>()
                .HasMany(a => a.LocationsVisited)
                .WithMany(a => a.UsersBeenHere)
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("LocationId");
                    m.ToTable("users_beento_locations");
                }
             );
            mb.Entity<User>()
                .HasMany(a => a.PostsLiked)
                .WithMany(a => a.Likes)
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("PostId");
                    m.ToTable("users_liked_posts");
                }
             );
            mb.Entity<User>().ToTable("users");
        }
    }
}
