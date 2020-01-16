using BOT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOT.Core;
using Lavalink4NET.Player;

namespace BOT.Services
{
    public class MusicPlayer : IMusicPlayer
    {
        public Queue<TrackInfo> ListadodeMusicas;
        public Guid TestReturnInstanceOfAllServers;
        public int Contador { get; set; }
        public IMusicPlayer AddMusic(TrackInfo track)
        {
            return this;
        }

        public IEnumerable<LavalinkTrack> GetMusics()
        {
            return null;
        }
        public IMusicPlayer PlayMusic(MyPlayer player)
        {
            return this;
        }

        public IMusicPlayer QueueMusic()
        {
            return this;
        }
    }
}
