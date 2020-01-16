using BOT.Core;
using BOT.Modules;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Eventos
{
    public class OnReactionAdd
    {
        public async Task OnReacctionAddHandler(ISocketMessageChannel message, SocketReaction reaction)
        {
            var Guild = (reaction.Channel as SocketTextChannel).Guild;
            if (reaction.MessageId != Utilidades.MessageSearch[Guild.Id]) return;
                var Valores = Utilidades.Tracks[Guild.Id];
            if (Valores.Count == 0)
                 return;
            var User = Guild.GetUser(reaction.UserId);
            if (User.IsBot)
                return;
      
            var ListMusica = Utilidades.ServerMusics[Guild.Id];
            var Canal = Guild.GetTextChannel(reaction.Channel.Id);
            if (reaction.Emote.Name.Contains("cancelar"))
            {
                Utilidades.EnBusqueda[Guild.Id] = false;
                await Canal.DeleteMessageAsync(reaction.MessageId);
            }
            await Canal.DeleteMessageAsync(reaction.MessageId);
            var Track = Valores[Utilidades.NumerosIconos[reaction.Emote.Name]];
            var Mensaje = Utilidades.GetInfoTrack(Track);
            ListMusica.Enqueue(new TrackInfo()
            {
                Canal = Canal,
                guild = Guild,
                Request = User.Username,
                track = Track,
                volume = 10
            });
            Console.WriteLine($"{ListMusica.Count} Datos Existentes");
            await Canal.SendMessageAsync(Mensaje);
            Utilidades.EnBusqueda[Guild.Id] = false;
            Console.WriteLine((reaction.Channel as SocketTextChannel).Guild.Id + " Reaction Add");
        }
    }
}
