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
using BOT.Services.Interfaces;
using BOT.Services;
using Microsoft.Extensions.DependencyInjection;
using BOT.Extenciones;

namespace BOT.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task cmdPing()
        {
            var currentClient = Context.Client;
            var ReplyMsg = CoreModule.Ping.Replace(CoreModule.identifUsuario, currentClient.CurrentUser.Username).Replace(CoreModule.identifping, currentClient.Latency.ToString());
            await ReplyAsync($"{CoreModule.emojiNet}{ReplyMsg}");
        }
        [Command("invite")]
        public async Task cmdInviteCommand()
        {
            await ReplyAsync("https://discordapp.com/api/oauth2/authorize?client_id=632045967016853515&permissions=8&scope=bot");
        }
      
        [Command("info")]
        public async Task cmdTestService()
        {
            var page = new StringBuilder();
            page.AppendLine($"Hola {Context.Message.Author.Mention}")
                .AppendLine("Soy un Bot Creado por **Daniel25A** en el lenguaje de Programación C#")
                .AppendLine("Mi Principal Función es la de Streamear la mejor calidad de Musica en tu Servidor..")
                .AppendLine("Lo Mejor de todo es que soy un **BOT** hecho exclusivamente para la Comunidad de habla Hispana...")
                .AppendLine("Si Notas algun Error Reportalo con el Programador:**RMX#3149**")
                .AppendLine("__**Agradecimientos**__")
                .AppendLine("``TrecTar#1597``(Argentina): Por ser la primera persona en agregarlo a su servidor y reportar algunos errores y/o Sugerencias")
                .AppendLine("``Coby_Kamikaze_Shōto#0796``(Brasil): Por dar la Idea del Bot y sugerir varios cambios y/o Comandos, Obrigado Friend :D");
                
            await ReplyAsync(page.ToString());
        }
    }
}
