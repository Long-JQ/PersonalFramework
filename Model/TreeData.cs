using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public struct TreeData
    {
        public string title;
        public string id;
        public string field;
        public List<TreeData> children;
        public string href;
        public bool spread;
        public bool @checked;
        public bool disabled;
    }
}
