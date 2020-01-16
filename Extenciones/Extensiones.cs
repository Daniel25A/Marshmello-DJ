using BOT.Core;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Extenciones
{
    public static class Extensiones
    {
        public static Servidor ManagerServer (this SocketCommandContext context)
        {
            return Utilidades.ServerManager[context.Guild.Id];
        }
    }
}
