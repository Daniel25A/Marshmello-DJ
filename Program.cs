using Microsoft.Extensions.DependencyInjection; 
using Lavalink4NET;                             
using Discord;                                  
using Lavalink4NET.DiscordNet;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using BOT.Poll;
using BOT.Core;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Lavalink4NET.Player;
using Lavalink4NET.Lyrics;
using BOT.Eventos;
using BOT.Services.Interfaces;
using BOT.Services;

namespace BOT
{
   public class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        private DiscordSocketClient _client;
        private CommandService _commands;
        public static IServiceProvider _services;
        public string botPrefix = "m.";
        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                    .AddSingleton(_client)
                    .AddSingleton(_commands)
                     .AddSingleton<LyricsOptions>()
                     .AddSingleton<LyricsService>()
                    .AddSingleton<IAudioService, LavalinkNode>()
                    .AddSingleton<IDiscordClientWrapper, DiscordClientWrapper>()
                    .AddSingleton<IMusicPlayer>(new MusicPlayer()) //Aqui Inyecto mi dependencia..
                    .AddSingleton(new LavalinkNodeOptions{
                        RestUri = "http://localhost:2333",
                        WebSocketUri = "ws://localhost:2333",
                        Password = "youshallnotpass",
                        AllowResuming = true,
                        BufferSize = 1024 * 1024 ,
                        DisconnectOnStop = false,
                        ReconnectStrategy = ReconnectStrategies.DefaultStrategy,
                        DebugPayloads = true
                    })

                .BuildServiceProvider();
            var discordClient = _services.GetRequiredService<DiscordSocketClient>();
            var audioService = _services.GetRequiredService<IAudioService>();
            var lyricsService = _services.GetRequiredService<LyricsService>();
            var music = _services.GetRequiredService<IMusicPlayer>();
            await RegisterCommandsAsync();
         
            string TokenBot = "-- your token here --";
            await _client.LoginAsync(TokenType.Bot, TokenBot);
            //Handlers
            _client.Log += _client_Log;
            _client.Ready += () => (new Eventos.OnReady()).OnReadyHandler(_client);
            _client.ReactionAdded += (a, b, c) => new OnReactionAdd().OnReacctionAddHandler(b, c);
            _client.JoinedGuild += (guild) => (new Eventos.OnGuildJoin()).OnGuildJoinHandler(guild);
            await _client.StartAsync();
        
            discordClient.Ready += () => audioService.InitializeAsync();
            await Task.Delay(-1);

        }



        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;
            int argPos = 0;
            if (message.HasStringPrefix(botPrefix, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
