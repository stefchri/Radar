using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RadarModels
{
    public class Notification
    {
        public long NotificationId { get; set; }
        [Required]
        public int UserId { get; set; }
        public Nullable<DateTime> Viewed { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
