using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Poll
{
    public class User
    {
       public ulong userID { get; set; }
        public int xp { get; set; }
        public string Alias { get; set; }
        public DateTime time { get; set; }
        public ulong ServerID { get; set; }
        public List<UserMessage> Messages { get; set; }
        public User()
        {
            Messages = new List<UserMessage>();
        }
        public static List<User> List = new List<User>();
        public List<UserMessage> Consulta = new List<UserMessage>();
        public List<UserMessage> GetInfo(string Fecha1, String Fecha2)
        {
            Consulta = Messages.Where(x => x.time >= DateTime.Parse(Fecha1) && x.time <= DateTime.Parse(Fecha2)).ToList();
            return Consulta;
        }

    }
    public class UserMessage
    {
        public DateTime time { get; set; }
        public int counter { get; set; }


    }
}
