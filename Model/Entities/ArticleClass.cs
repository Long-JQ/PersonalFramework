using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ArticleClass : BaseEntity
    {
        [StringLength(250)]
        [DisplayName("分类名")]
        [Required(ErrorMessage = "请输入分类")]
        public string ClassName { get; set; }

        [StringLength(250)]
        [DisplayName("父分类ID")]
        public string ParentID { get; set; }

        [StringLength(250)]
        [DisplayName("父分类")]
        public string ParentName { get; set; }

        [StringLength(250)]
        [DisplayName("分类路径")]
        public string ClassPath { get; set; }

    }
}
