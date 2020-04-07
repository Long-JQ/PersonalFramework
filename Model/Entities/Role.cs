using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Role : BaseEntity
    {
        [StringLength(250)]
        [Required(ErrorMessage = "请输入角色")]
        public string RoleName { get; set; }
    }
}
