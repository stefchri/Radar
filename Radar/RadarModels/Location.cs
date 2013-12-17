using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RadarModels
{
    public class Location
    {
        public long LocationId { get; set; }

        [Required]
        public Decimal Latitude { get; set; }

        [Required]
        public Decimal Longitude { get; set; }

        [MaxLength(255)]
        [Required]
        public String Street { get; set; }

        [MaxLength(255)]
        [Required]
        public String Number { get; set; }

        [MaxLength(255)]
        public String Box { get; set; }

        [MaxLength(255)]
        [Required]
        public String Zipcode { get; set; }

        [MaxLength(255)]
        [Required]
        public String City { get; set; }

        [MaxLength(255)]
        [Required]
        public String Country { get; set; }

        public virtual List<User> Users { get; set; }
        public virtual List<User> UsersBeenHere { get; set; }
        public virtual List<Company> Companies { get; set; }
    }
}
