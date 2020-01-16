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
using BOT.Core;

namespace BOT.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task cmdHelpCommand()
        {
            var authorBot = Context.Client.GetUser(600338139961229344);
            //"``play`` | ``queue`` | ``stop`` | ``skip`` | ``replay`` | ``pause`` | ``resume`` | ``minute`` | ``nowplaying``"
            var page = new StringBuilder().Append("``play`` | ").Append("``queue`` | ").Append("``stop`` | ").Append("``skip`` | ")
                .Append("``replay`` | ").Append("``pause`` | ").Append("``resume`` | ").Append("``minute`` | ").Append("``nowplaying``");
            var embed = new EmbedBuilder()
                .WithColor(Utilidades.EmbedColor)
                .WithAuthor(Context.Client.CurrentUser.Username, Context.Client.CurrentUser.GetAvatarUrl())
                .WithTitle($"**[Prefijos]: **``m.`` ``<comando>`` ``<argumentos>``")
                .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl())
                .WithFooter($"Creado por {authorBot.Username}#{authorBot.Discriminator}", authorBot.GetAvatarUrl())

                .AddField($"{CoreModule.emojiAuricular}**Musica:** ({page.ToString().Split('|').Length})", page.ToString())
                .AddField($"{CoreModule.emojiTool}**Otros:** (4)", "``ping`` | ``lyrics`` | ``search`` | ``info``| ``invite``");
            await ReplyAsync(null, false, embed.Build());
        }
    }
}
