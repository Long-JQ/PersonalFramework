using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class BaseEntity
    {
        [Key]
        public string ID { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        [StringLength(250)]
        public string SortCode { get; set; }
        public BaseEntity()
        {
            ID = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
            SortCode = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
