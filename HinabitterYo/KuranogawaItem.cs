using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinabitterYo
{
    public class KuranogawaItem : IYoItem
    {
        public long ID { get; set; }
        public string Link { get; set; }
        public string From { get; set; }
        public Account FromAccount { get; set; }
    }
}
