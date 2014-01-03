using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarModels
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(255)]
        public string Gender { get; set; }

        [MaxLength(255)]
        public string Avatar { get; set; }

        [MaxLength(255)]
        [Required]
        public string Password { get; set; }

        [MaxLength(255)]
        [Required]
        public string Salt { get; set; }

        [Required]
        [MaxLength(500)]
        public string Bio { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<DateTime> LockedDate { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }

        [Required]
        public long LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual List<Role> Roles { get; set; }
        public virtual List<Company> CompaniesFollowing { get; set; }

        public virtual List<User> FollowingUsers { get; set; }

        [JsonIgnore]
        public virtual List<User> UsersFollowing { get; set; }


        public virtual List<Notification> Notifications { get; set; }
        public virtual List<Category> CategoriesInterestedIn { get; set; }
        public virtual List<Location> LocationsVisited { get; set; }
        public virtual List<Comment> CommentsPosted { get; set; }
        public virtual List<Message> SentMessages { get; set; }

        [JsonIgnore]
        public virtual List<Message> RecievedMessages { get; set; }
        [JsonIgnore]
        public virtual List<Rating> Ratings { get; set; }
        public virtual List<Employee> Employees { get; set; }

        [JsonIgnore]
        public virtual List<Post> PostsLiked { get; set; }
    }
}
