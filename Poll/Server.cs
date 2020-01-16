using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Poll
{
    public class Server:IDisposable
    {
        public int NewMembersCounter { get; set; }
        DateTime time { get; set; }
        public static List<Server> List = new List<Server>();
        public ulong ServerID { get; set; }
        public int LeaveMembersCounter { get; set; }
        public List<Server> Consulta = new List<Server>();
        public List<Server> GetInfo(string Fecha1,String Fecha2,ulong ServerID)
        {
            Consulta = List.Where(x => x.time >= DateTime.Parse(Fecha1) && x.time <= DateTime.Parse(Fecha2) && x.ServerID== ServerID).ToList();
            return Consulta;
        }

        public void Dispose()
        {
            if (Consulta.Count > 0)
            {
                Consulta.Clear();
            }
        }
    }
}
