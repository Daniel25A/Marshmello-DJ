using Lavalink4NET;
using Lavalink4NET.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialBot
{
    interface IDiscordClientWrapper
    {
        Task InitializeAsync();

        ulong CurrentUserId { get; }
        int ShardCount { get; }

        Task SendVoiceUpdateAsync(ulong guildId, ulong? voiceChannelId, bool selfDeaf = false, bool selfMute = false);
        Task<IEnumerable<ulong>> GetChannelUsersAsync(ulong guildId, ulong voiceChannelId);


        event AsyncEventHandler<VoiceStateUpdateEventArgs> VoiceStateUpdated;


        event AsyncEventHandler<VoiceServer> VoiceServerUpdated;
    }
}
