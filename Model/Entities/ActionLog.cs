using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ActionLog : BaseEntity
    {
        [DisplayName("平台(用户,管理员)")]
        [StringLength(250)]
        public string Platform { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "请输入密码")]
        public string UID { get; set; }

        [StringLength(250)]
        public string UserName { get; set; }

        [DisplayName("来路")]
        [StringLength(250)]
        public string Source { get; set; }

        [DisplayName("访问地址")]
        [StringLength(250)]
        public string Location { get; set; }

        [DisplayName("行为内容")]
        [StringLength(250)]
        public string ActionContent { get; set; }

        [DisplayName("IP")]
        [StringLength(250)]
        public string IP { get; set; }

        [DisplayName("data")]
        [StringLength(250)]
        public string RequestData { get; set; }

        [DisplayName("url")]
        [StringLength(250)]
        public string RequestUrl { get; set; }

        public ActionLog()
        {

        }
    }
}
