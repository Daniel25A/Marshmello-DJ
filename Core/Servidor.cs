using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Core
{
    public class Servidor
    {
        public List<TopMusic> Estacion40 = new List<TopMusic>();
    }
   public class TopMusic
    {
        public string Name { get;  set; }
        public string URL { get;  set; }
        public Int32 Score { get; set; }
    }
}
