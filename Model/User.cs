using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class User : BaseEntity
    {
        [StringLength(250)]
        public string UserName { get; set; }
        [StringLength(250)]
        public string Password { get; set; }
        [StringLength(250)]
        public string Password2 { get; set; }
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
        public User()
        {
            LastLoginTime = DateTime.Now;
        }
    }
}
