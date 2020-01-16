using Discord.WebSocket;
using System.Threading.Tasks;

namespace BOT.Eventos
{
    public class OnMessage
    {
        public  Task MessageReceived(SocketMessage argMessage, DiscordSocketClient _client)
        {
            return Task.CompletedTask;
        }
    }
}
