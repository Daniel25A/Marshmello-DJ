using BOT.Core;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Eventos
{
    public class OnGuildJoin
    {
        public Task OnGuildJoinHandler(SocketGuild guild)
        {
            if (!Utilidades.ServerMusics.ContainsKey(guild.Id))
                Utilidades.ServerMusics.Add(guild.Id, new Queue<TrackInfo>());
            if (!Utilidades.NowPlaying.ContainsKey(guild.Id))
                Utilidades.NowPlaying.Add(guild.Id, new TrackInfo());
            if (!MyPlayer.lastChannels.ContainsKey(guild.Id))
                MyPlayer.lastChannels.Add(guild.Id, null);
            if (!Utilidades.SkipCounter.ContainsKey(guild.Id))
                Utilidades.SkipCounter.Add(guild.Id, 0);
            if (!Utilidades.VotesUsers.ContainsKey(guild.Id))
                Utilidades.VotesUsers.Add(guild.Id, new List<ulong>());
            if (!Utilidades.Tracks.ContainsKey(guild.Id))
                Utilidades.Tracks.Add(guild.Id, new Dictionary<int, Lavalink4NET.Player.LavalinkTrack>());
            if (!Utilidades.EnBusqueda.ContainsKey(guild.Id))
                Utilidades.EnBusqueda.Add(guild.Id, false);
            if (!Utilidades.MessageSearch.ContainsKey(guild.Id))
                Utilidades.MessageSearch.Add(guild.Id, 0);
            if (!Utilidades.ServerManager.ContainsKey(guild.Id))
                Utilidades.ServerManager.Add(guild.Id, new Servidor());
            Console.WriteLine($"-- Se Cargo la Configuracion para {guild.Name}--");
            return Task.CompletedTask;
        }
    }
}