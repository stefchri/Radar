using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace RadarModels
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Avatar { get; set; }
        [Required]
        public long LocationId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string OpenHours { get; set; }
        [Required]
        public string Extra { get; set; }
        [Required]
        public CompanyType CompanyType { get; set; }
        public int? ParentId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }

        public virtual Location Location { get; set; }
        [ScriptIgnore]
        public virtual Company Parent { get; set; }
        public virtual List<Company> Branches { get; set; }

        [JsonIgnore]
        public virtual List<User> UsersFollowing { get; set; }

        public virtual List<Employee> Employees { get; set; }

        [JsonIgnore]
        public virtual List<Category> Categories { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual List<Rating> Ratings { get; set; }
    }

    public enum CompanyType { 
        Free,
        Basic,
        Advanced,
        Gold
    }
}
