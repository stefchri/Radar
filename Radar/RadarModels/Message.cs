using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RadarModels
{
    public class Message
    {
        public long MessageId { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        [Required]
        public int SenderId { get; set; }
        [Required]
        public int RecieverId { get; set; }

        [JsonIgnore]
        public virtual User Sender { get; set; }

        public virtual User Reviever { get; set; }
    }
}
