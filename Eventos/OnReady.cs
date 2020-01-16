using BOT.Core;
using Discord.WebSocket;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Eventos
{
    public class OnReady
    {
        public async Task OnReadyHandler(DiscordSocketClient _client)
        {
            foreach(var x in _client.Guilds)
            {
                if (!Utilidades.ServerMusics.ContainsKey(x.Id))
                    Utilidades.ServerMusics.Add(x.Id, new Queue<TrackInfo>());
                if (!Utilidades.NowPlaying.ContainsKey(x.Id))
                    Utilidades.NowPlaying.Add(x.Id, new TrackInfo());
                if (!MyPlayer.lastChannels.ContainsKey(x.Id))
                    MyPlayer.lastChannels.Add(x.Id, null);
                if (!Utilidades.SkipCounter.ContainsKey(x.Id))
                    Utilidades.SkipCounter.Add(x.Id, 0);
                if (!Utilidades.VotesUsers.ContainsKey(x.Id))
                    Utilidades.VotesUsers.Add(x.Id, new List<ulong>());
                if (!Utilidades.Tracks.ContainsKey(x.Id))
                    Utilidades.Tracks.Add(x.Id, new Dictionary<int, Lavalink4NET.Player.LavalinkTrack>());
                if (!Utilidades.EnBusqueda.ContainsKey(x.Id))
                    Utilidades.EnBusqueda.Add(x.Id, false);
                if (!Utilidades.MessageSearch.ContainsKey(x.Id))
                    Utilidades.MessageSearch.Add(x.Id, 0);
                if (!Utilidades.ServerManager.ContainsKey(x.Id))
                    Utilidades.ServerManager.Add(x.Id, new Servidor());
            }
           await _client.SetStatusAsync(Discord.UserStatus.DoNotDisturb);
            Console.WriteLine("-- Las Conficuraciones de los Servidores fueron cargadas --");
            Console.WriteLine($"=== BOT ON ====");
            Console.WriteLine($"Total Users: {_client.Guilds.Sum(x => x.Users.Count)}");
            Console.WriteLine($"GUILDS {_client.Guilds.Count}");
        }
    }
}
