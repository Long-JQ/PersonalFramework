using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Test : BaseEntity
    {
        [StringLength(250)]
        [Required(ErrorMessage = "请输入角色")]
        public string Name { get; set; }
        public string AuthorityID { get; set; }
    }
}
