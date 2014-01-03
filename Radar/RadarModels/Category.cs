using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace RadarModels
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        public String Name { get; set; }

        public virtual List<Company> Companies { get; set; }
        [JsonIgnore]
        public virtual List<User> UsersInterestedIn { get; set; }
    }
}
