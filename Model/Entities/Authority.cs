using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Authority : BaseEntity
    {
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        public string ParentName { get; set; }
        [StringLength(250)]
        public string ParentID { get; set; }
        [StringLength(250)]
        public string Action { get; set; }
        [StringLength(250)]
        public string ActionDesc { get; set; }
    }
}
