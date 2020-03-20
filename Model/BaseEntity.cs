using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BaseEntity
    {
        [Key]
        public Guid ID { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        [StringLength(250)]
        public string SortCode { get; set; }
    }
}
