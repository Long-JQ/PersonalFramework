using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Admin:BaseEntity
    {
        [DisplayName("管理员名字")]
        [Required(ErrorMessage = "请输入管理员名字")]
        [StringLength(250)]
        public string AdminName { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "请输入管理员名字")]
        public string Password { get; set; }
        [StringLength(250)]
        public string Salt { get; set; }
        [StringLength(250)]
        public string RoleName { get; set; }
        [StringLength(250)]
        public string RoleID { get; set; }
        [StringLength(250)]
        public string Token { get; set; }
        [StringLength(250)]
        public string LastLoginIP { get; set; }
        public DateTime LastLoginTime { get; set; }
        public Admin()
        {
            LastLoginTime = DateTime.Now;
        }
    }
}
