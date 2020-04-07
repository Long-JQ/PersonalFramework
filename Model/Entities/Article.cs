using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Article : BaseEntity
    {
        [StringLength(250)]
        [DisplayName("标题")]
        [Required(ErrorMessage = "请输入标题")]
        public string Title { get; set; }

        [StringLength(250)]
        [DisplayName("副标题")]
        public string SubTitle { get; set; }

        [StringLength(250)]
        [DisplayName("作者")]
        [Required(ErrorMessage = "请输入作者")]
        public string Author { get; set; }

        [DisplayName("内容")]
        [Required(ErrorMessage = "请输入内容")]
        public string Content { get; set; }

        [StringLength(250)]
        [DisplayName("来源")]
        public string Source { get; set; }

        [StringLength(250)]
        [DisplayName("阅读量")]
        public string ReadCount { get; set; }
        
        [DisplayName("显示")]
        public bool IsShow { get; set; }
        
        [DisplayName("置顶")]
        public bool IsTop { get; set; }
        
        [DisplayName("公告")]
        public bool IsNotice { get; set; }

        [StringLength(250)]
        [DisplayName("附件")]
        public string Attachment { get; set; }

        [StringLength(250)]
        [DisplayName("文章分类ID")]
        [Required(ErrorMessage = "请选择文章分类")]
        public string ClassID { get; set; }

        public Article()
        {

        }

    }
}
