using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RadarModels
{
    public class Rating
    {
        public int RatingId { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual List<Company> Companies { get; set; }

        public virtual User User { get; set; }
    }
}
