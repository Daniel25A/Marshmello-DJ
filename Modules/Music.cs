using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.Webhook;
using Discord.WebSocket;
using Discord.Rest;
using Discord.Audio;
using Discord.API;
using Discord.Commands;
using Lavalink4NET;
using Lavalink4NET.Rest;
using Lavalink4NET.Player;
using BOT.Core;
using Lavalink4NET.Lyrics;

namespace BOT.Modules
{
    public class Music : ModuleBase<SocketCommandContext>
    {
        public IAudioService audio { get; set; }
        public LyricsService lyrics { get; set; }
        [Command("play")]
        [Alias("p")]
        public async Task PlayCommand([Remainder]string Name = "")
        {
            var author = Context.Message.Author;
            var IdServidor = Context.Guild.Id;
            var canaldevoz = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Users.Any(u => u.Id == Context.Message.Author.Id));
            var Listado = Utilidades.ServerMusics[IdServidor] ?? new Queue<TrackInfo>();
            MyPlayer player;
        
            if (Name == string.Empty)
            {
                await ReplyAsync($"{author.Mention}{CoreModule.emojierror}m.play ``[URL|Nombre]``");
                return;
            }
            if (canaldevoz == null)
            {
                await ReplyAsync($"{CoreModule.emojierror}{CoreModule.necesitaEntrar.Replace(CoreModule.identifUsuario, author.Username)}");
                return;
            }
            var Permiso = Context.Guild.CurrentUser.GetPermissions(canaldevoz);
            if (!Permiso.Connect)
            {
                await ReplyAsync($"{author.Mention} {CoreModule.emojierror}Necesito Permisos(``CanConnect``) en el Canal de Voz ``{canaldevoz.Name}``");
                return;
            }
            if (audio.GetPlayer<MyPlayer>(IdServidor) == null)
            {
                player = await audio.JoinAsync<MyPlayer>(IdServidor, canaldevoz.Id);
                await ReplyAsync($"{CoreModule.emojiantena}{CoreModule.entroCanal.Replace(CoreModule.idenfitCanalvoz, canaldevoz.Name)}");
            }
            else
            {
                player = audio.GetPlayer<MyPlayer>(IdServidor);
            }

            var track = await audio.GetTrackAsync(Name, SearchMode.YouTube);
            var musicinfo = $"{track.Title} ({Utilidades.FormatMinutes((int)track.Duration.TotalSeconds)})";
            await ReplyAsync($"{CoreModule.emojiadd}{CoreModule.musicAdd.Replace(CoreModule.identifmusicInfo, musicinfo)}");
            if (player.State == PlayerState.Playing || player.State==PlayerState.Paused)
            {
                Listado.Enqueue(new TrackInfo()
                {
                    guild = Context.Guild,
                    Canal = Context.Channel as SocketTextChannel,
                    Request = author.Username,
                    track = track,
                    volume = 10
                });
                return;
            }
            await ReplyAsync($"{CoreModule.emojiTocando}{CoreModule.playNow.Replace(CoreModule.identifmusicInfo, musicinfo).Replace(CoreModule.identifUsuario, author.Username)}");
            await player.PlayAsync(track);
            Utilidades.NowPlaying[IdServidor] = (new TrackInfo()
            {
                Canal = Context.Channel as SocketTextChannel,
                guild = Context.Guild,
                Request = author.Username
            ,
                track = track,
                volume = 10
            });
            MyPlayer.lastChannels[IdServidor] = Context.Channel as SocketTextChannel;
        }
        [Command("pause")]
        [Alias("pa")]
        public async Task cmdPauseCommand()
        {
            var player = audio.GetPlayer<MyPlayer>(Context.Guild.Id);
            if (player == null || (player!=null && player.State==PlayerState.Paused))
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.NoPlay}");
                return;
            }
            await player.PauseAsync();
            await ReplyAsync($"{CoreModule.emojipause}{CoreModule.Pause.Replace(CoreModule.identifmusicInfo, player.CurrentTrack.Title)}");
        }
        [Command("replay")]
        [Alias("rp")]
        public async Task cmdReplayCommand()
        {
            var player = audio.GetPlayer<MyPlayer>(Context.Guild.Id);
            if (player == null || (player != null && player.State == PlayerState.Destroyed))
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.NoPlay}");
                return;
            }
            await player.ReplayAsync();
            await ReplyAsync($"{CoreModule.emojiAlready}{CoreModule.Replay.Replace(CoreModule.identifmusicInfo, player.CurrentTrack.Title)}");
        }
        [Command("resume")]
        [Alias("re")]
        public async Task cmdResumeCommand()
        {
            var player = audio.GetPlayer<MyPlayer>(Context.Guild.Id);
            if (player == null || (player != null && player.State == PlayerState.Playing))
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.NoPause}");
                return;
            }
            await player.ResumeAsync();
            await ReplyAsync($"{CoreModule.emojiresume}{CoreModule.Resume.Replace(CoreModule.identifmusicInfo, player.CurrentTrack.Title)}");
        }
        [Command("minute")]
        [Alias("m")]
        public async Task cmdTestManipularTiempo(string time = "")
        {
            var syntax = $"{CoreModule.emojiadvertencia} m.minute ``[minuto:segundo]``\nEjemplo de Uso: ``time`` 2:33 | 1:10 | 3:00";
            if (time == string.Empty)
            {
                await ReplyAsync(syntax);
                return;
            }
            var player = audio.GetPlayer<MyPlayer>(Context.Guild.Id);
            if (player == null || (player != null && (player.State == PlayerState.Destroyed || player.State == PlayerState.Paused)))
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.NoSePuedeAlterar}");
                return;
            }
            int Segundos;
            Utilidades.getSegundos(time, out Segundos);
            if(Segundos==-1 || Segundos > player.CurrentTrack.Duration.TotalSeconds)
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.NoSePuedeAlterar}");
                return;
            }
            var track = player.CurrentTrack.WithPosition(new TimeSpan(0, 0, Segundos));
            await player.PlayAsync(track);
            await ReplyAsync($"{CoreModule.okemoji} Se Adelanto Correctamente la Musica al Minuto ``{Utilidades.FormatMinutes(Segundos)}``");
        }
        [Command("queue")]
        [Alias("q")]
        public async Task QueueCommand()
        {
            var ListadoMusic = Utilidades.ServerMusics[Context.Guild.Id];
            var EmbedQueue = new EmbedBuilder();
            var player = audio.GetPlayer<MyPlayer>(Context.Guild.Id);
            if (player == null || ListadoMusic.Count == 0)
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.FilaVacia}");
                return;
            }
            EmbedQueue.WithAuthor(Context.Guild.Name, Context.Guild.IconUrl)
                .WithFooter($"Duración:{Utilidades.FormatMinutes((int)ListadoMusic.Sum(x => x.track.Duration.TotalSeconds))} | Entradas: {ListadoMusic.Count}")
                .WithColor(Utilidades.EmbedColor)
                .WithThumbnailUrl(Context.Guild.CurrentUser.GetAvatarUrl());
            var page = new StringBuilder();
            page.AppendLine("**Fila De Reproducción**");
            page.AppendLine($"Tocando Ahora:**[{player.CurrentTrack.Title}]({player.CurrentTrack.Source})** ``[{Utilidades.FormatMinutes((int)player.CurrentTrack.Duration.TotalSeconds)}]``");
            int Contador = 0;
            foreach (var Track in ListadoMusic)
            {
                Contador++;
                page.AppendLine($"``»`` ``{Contador}`` **[{Track.track.Title}]({Track.track.Source})** ``[{Utilidades.FormatMinutes((int)Track.track.Duration.TotalSeconds)}]``");
            }
            EmbedQueue.WithDescription(page.ToString());
            await ReplyAsync(null, false, EmbedQueue.Build());
        }
        [Command("skip")]
        [Alias("s")]
        public async Task cmdSkip()
        {
            var author = Context.Message.Author;
            var IdServidor = Context.Guild.Id;
            var Listado = Utilidades.ServerMusics[IdServidor];
            var player = audio.GetPlayer<MyPlayer>(Context.Guild.Id);
            var Votaciones = Utilidades.VotesUsers[IdServidor];
            if (player == null)
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.NoPlay}");
                return;
            }
            var canaldevoz = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Users.Any(u => u.Id == Context.Guild.CurrentUser.Id));
            if (canaldevoz == null)
            {
                await ReplyAsync($"{CoreModule.emojierror}{CoreModule.errorMsg}");
                return;
            }
            if (!canaldevoz.Users.Any(x => x.Id == author.Id))
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia} {author.Mention} Necesitas estar en el canal donde se esta reproduciendo la musica actual para utilizar este **Comando**");
                return;
            }
            if (Listado.Count == 0)
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.FilaVacia}");
                return;
            }
            if (Votaciones.Contains(author.Id))
            {
                await ReplyAsync($"{CoreModule.emojiAlready}{CoreModule.yaVoto.Replace(CoreModule.identifmusicInfo, player.CurrentTrack.Title)}");
                return;
            }
            int cantidadUsuarios = canaldevoz.Users.Where(x => x.IsBot == false).Count();
            string mensajeQueue=string.Empty;
            if (cantidadUsuarios > 1)
                mensajeQueue = $"{CoreModule.emojiAuricular} ¿Desean Saltar la Musica Actual? %votos%/{cantidadUsuarios}";
            if (cantidadUsuarios == 1 || (author as SocketGuildUser).GuildPermissions.Administrator)
            {
                var nextTrack = Listado.Dequeue();
                await player.PlayAsync(nextTrack.track);
                var musicinfo = $"{nextTrack.track.Title} ({Utilidades.FormatMinutes((int)nextTrack.track.Duration.TotalSeconds)})";
                await ReplyAsync($"{CoreModule.emojisiguiente}{CoreModule.Skip.Replace(CoreModule.identifmusicInfo, musicinfo)}");
                await ReplyAsync($"{CoreModule.emojiTocando}{CoreModule.playNow.Replace(CoreModule.identifmusicInfo, musicinfo).Replace(CoreModule.identifUsuario, nextTrack.Request)}");
                Utilidades.ClearSkipData(IdServidor);
            }
            else
            {
                var Votos = ++Utilidades.SkipCounter[IdServidor];
                Votaciones.Add(author.Id);
                if (Votos < cantidadUsuarios)
                {
                    await ReplyAsync(mensajeQueue.Replace("%votos%", Votos.ToString()));
                }
                else if (Votos >= cantidadUsuarios)
                {
                    var nextTrack = Listado.Dequeue();
                    await player.PlayAsync(nextTrack.track);
                    var musicinfo = $"{nextTrack.track.Title} ({Utilidades.FormatMinutes((int)nextTrack.track.Duration.TotalSeconds)})";
                    await ReplyAsync($"{CoreModule.emojisiguiente}{CoreModule.Skip.Replace(CoreModule.identifmusicInfo, musicinfo)}");
                    await ReplyAsync($"{CoreModule.emojiTocando}{CoreModule.playNow.Replace(CoreModule.identifmusicInfo, musicinfo).Replace(CoreModule.identifUsuario, nextTrack.Request)}");
                    Utilidades.ClearSkipData(IdServidor);
                }
            }
        }
        [Command("nowplaying")]
        [Alias("np")]
        public async Task cmdNowPlaying()
        {
            var serverId = Context.Guild.Id;
            var player = audio.GetPlayer<MyPlayer>(Context.Guild.Id);
            if(player==null || (player!=null && player.State == PlayerState.Destroyed))
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.NoPlay}");
                return;
            }
            int Init = (int)player.TrackPosition.TotalSeconds;
            int End = (int)player.CurrentTrack.Duration.TotalSeconds;
            var embed = new EmbedBuilder()
                .WithColor(Utilidades.EmbedColor)
                .WithDescription("**Estado Actual de la Musica**\n\n" + Utilidades.FormatearPista(Init, End));
            await ReplyAsync($"{CoreModule.nowPlayingEmoji} Reproduciendo Ahora:\n**{player.CurrentTrack.Title}**\n{CoreModule.TiempoEmoji}Tiempo: ``[{Utilidades.FormatMinutes(Init)}]``/``[{Utilidades.FormatMinutes(End)}]``",
                false, embed.Build());
        }
        [Command("lyrics")]
        [Alias("lri")]
        public async Task cmdLyrics([Remainder]string parametro="")
        {
            var serverId = Context.Guild.Id;
            var syntax = $"{CoreModule.emojierror} ``m.lyrics`` Author - Nombre de la Musica";
            if(parametro==string.Empty || !parametro.Contains("-"))
            {
                await ReplyAsync(syntax);
                return;
            }
            var author = parametro.Split('-');
            var letra = await lyrics.GetLyricsAsync(author[0], author[1]);
            if (string.IsNullOrEmpty(letra))
            {
                await ReplyAsync($"{CoreModule.emojierror} No se Encontraron Resultados para: ``{parametro}``");
                return;
            }
            string result = "";
            if (letra.Length > 1800)
            {
                result = letra.Substring(0, 1800) +"...";
            }
            else
            {
                result = letra;
            }
            var page = new StringBuilder()
                .AppendLine("**__Letra De La Canción__**\n")
                .AppendLine(result);
            await ReplyAsync($"{CoreModule.infoLetraEmoji}Lyrics para:\n``{parametro}``\n" + page.ToString());
        }
        [Command("search")]
        [Alias("se")]
        public async Task BuscarCommand([Remainder]string parametro = "")
        {
            var serverId = Context.Guild.Id;
            var player = audio.GetPlayer<MyPlayer>(Context.Guild.Id);
            if (player == null || (player != null && player.State == PlayerState.Destroyed))
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.NoPlay}");
                return;
            }
            if (Utilidades.EnBusqueda[serverId])
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}Complete La Busqueda y Luego Continue");
                return;
            }
            var msgLoad = await ReplyAsync($"{CoreModule.emojiantena} Buscando Resultados para: ``{parametro}``");
            List<LavalinkTrack> Tracks = audio.GetTracksAsync(parametro, SearchMode.YouTube).Result.Take(5).ToList();
            var Valores = Utilidades.Tracks[Context.Guild.Id];
            Valores.Clear();
            if (Tracks.Count <= 0)
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia} No hay Resultados para: ``[{parametro}]``");
                return;
            }
            var page = new StringBuilder();
            int contador = 0;
            foreach(var x in Tracks)
            {
                page.AppendLine($"{CoreModule.numeros[contador]} ``[{Utilidades.FormatMinutes((int)x.Duration.TotalSeconds)}]`` **[{x.Title}]({x.Source})** ");
                contador++;
                Valores.Add(contador, x);
            }
            await msgLoad.DeleteAsync();
            var msg = await ReplyAsync($"{CoreModule.ResultEmoji} Resultados Para: ``[{parametro}]``", false, new EmbedBuilder().WithColor(Utilidades.EmbedColor).
                WithDescription(page.ToString())
                .WithThumbnailUrl(Context.Guild.CurrentUser.GetAvatarUrl())
                .WithAuthor(Context.Guild.Name, Context.Guild.IconUrl)
                .Build());
            List<IEmote> Listado= new List<IEmote>();
            
            foreach(var x in CoreModule.numeros)
            {
                Listado.Add(Emote.Parse(x));
            }

            await msg.AddReactionsAsync(Listado.ToArray(), new RequestOptions());
            await msg.AddReactionAsync(Emote.Parse("<:cancelar:659465947685453824>"), new RequestOptions());
            Utilidades.EnBusqueda[serverId] = true;
            Utilidades.MessageSearch[serverId] = msg.Id;
        }
        [Command("stop")]
        [Alias("stp")]
        public async Task stopCommand()
        {
            var serverId = Context.Guild.Id;
            var player = audio.GetPlayer<MyPlayer>(Context.Guild.Id);
            if (player == null || player.State == PlayerState.Destroyed)
            {
                await ReplyAsync($"{CoreModule.emojiadvertencia}{CoreModule.FilaVacia}");
                return;
            }
            var Musicas =  Utilidades.ServerMusics[serverId];
            var NowPlaying = Utilidades.NowPlaying[serverId];
            var LasChannel = MyPlayer.lastChannels[serverId];
            Utilidades.Limpiar(ref Musicas, ref NowPlaying, ref LasChannel);
            await player.StopAsync(true);
            await player.DestroyAsync();
            await ReplyAsync($"{CoreModule.okemoji}{CoreModule.Canal}");
        }
    }
}
