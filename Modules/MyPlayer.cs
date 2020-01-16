namespace BOT {
    using System.Threading.Tasks;
    using Lavalink4NET.Events;
    using Lavalink4NET.Player;
    using Lavalink4NET;
    using System;
    using BOT.Core;
    using System.Collections.Generic;
    using Discord.WebSocket;
    using BOT.Modules;
    using System.Linq;
    using Lavalink4NET.Payloads.Player;

    public sealed class MyPlayer : LavalinkPlayer
    {
        public MyPlayer(LavalinkSocket lavalinkSocket, IDiscordClientWrapper client, ulong guildId,bool disconneconStop)
            : base(lavalinkSocket, client, guildId, disconneconStop)
        {
        }
        public static Dictionary<ulong, SocketTextChannel> lastChannels = new Dictionary<ulong, SocketTextChannel>();
        public override async Task OnTrackEndAsync(TrackEndEventArgs arg)
        {
            if (arg.MayStartNext)
            {
                Utilidades.ClearSkipData(arg.Player.GuildId);
                var list = Utilidades.ServerMusics[arg.Player.GuildId];
                if (list.Count > 0)
                {
                    var musicNext = list.Dequeue();
                    await musicNext.Canal.SendMessageAsync(Utilidades.GetMusicInfo(musicNext.track, musicNext.Request));
                    lastChannels[arg.Player.GuildId] = musicNext.Canal;
                    Utilidades.NowPlaying[arg.Player.GuildId] = musicNext;
                   await arg.Player.PlayAsync(musicNext.track);
                }
                else
                {
                   await lastChannels[arg.Player.GuildId].SendMessageAsync($"{CoreModule.emojiadvertencia} Se Acabaran Las Musicas, Por lo tanto me desconecte del Canal.");
                    Console.WriteLine($"{Utilidades.GetTimeFormat()} Acabaron las Musicas");
                    Utilidades.NowPlaying[arg.Player.GuildId] = null;
                    lastChannels[arg.Player.GuildId] = null;
                    await arg.Player.DestroyAsync();
                }
            }
        }
       
    }
}