using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace RadarModels
{
    public class Role
    {
        public int RoleId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public RoleType Type { get; set; }

        [ScriptIgnore]
        public virtual List<User> Users { get; set; }
        public virtual List<Employee> Employees { get; set; }
    }
    public enum RoleType
    {
        User,
        Company
    }
}
