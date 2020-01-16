using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Poll
{
    public class Channel
    {
        public int MessageCounter { get; set; }
        public ulong ServerID { get; set; }
        public ulong ChannelID { get; set; }
        public DateTime time { get; set; }
        public static List<Channel> List = new List<Channel>();
        public List<Channel> Consulta = new List<Channel>();
        public List<Channel> GetInfo(string Fecha1, String Fecha2)
        {
            Consulta = List.Where(x => x.time >= DateTime.Parse(Fecha1) && x.time <= DateTime.Parse(Fecha2)).ToList();
            return Consulta;
        }

    }
}
