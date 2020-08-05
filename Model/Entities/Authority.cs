using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("权限名")]
        public string Action { get; set; }

        [StringLength(250)]
        [DisplayName("权限描述")]
        public string ActionDesc { get; set; }

        [StringLength(250)]
        [DisplayName("权限类型")]
        public string Type { get; set; }
    }
}
