using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace RadarModels
{
    public class Comment
    {
        public long CommentId { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        [Required]
        public int UserId { get; set; }
        public long? ParentId { get; set; }

        public virtual User User { get; set; }
        [ScriptIgnore]
        public virtual Comment Parent { get; set; }
        public virtual List<Comment> Children { get; set; }
    }
}
