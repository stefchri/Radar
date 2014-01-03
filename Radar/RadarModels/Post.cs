using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RadarModels
{
    public class Post
    {
        public long PostId { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        [MaxLength(255)]
        public string SubTitle { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int EmployeeId { get; set; }

        [JsonIgnore]
        public virtual Company Company { get; set; }

        [JsonIgnore]
        public virtual Employee Employee { get; set; }

        public virtual List<User> Likes { get; set; }
    }
}
