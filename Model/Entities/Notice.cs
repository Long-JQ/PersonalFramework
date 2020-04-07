using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Notice : BaseEntity
    {
        [StringLength(250)]
        [DisplayName("标题")]
        [Required(ErrorMessage = "请输入标题")]
        public string Title { get; set; }

        [StringLength(250)]
        [DisplayName("平台")]
        public string Platform { get; set; }

        [DisplayName("内容")]
        [Required(ErrorMessage = "请输入内容")]
        public string Content { get; set; }
        
        [DisplayName("显示")]
        public bool IsShow { get; set; }
        
        [DisplayName("置顶")]
        public bool IsTop { get; set; }
        
        [StringLength(250)]
        [DisplayName("附件")]
        public string Attachment { get; set; }
        
        public Notice()
        {

        }

    }
}
