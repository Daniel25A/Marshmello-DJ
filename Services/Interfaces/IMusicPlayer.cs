using BOT.Core;
using Lavalink4NET.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Services.Interfaces
{
   public interface IMusicPlayer
    {
        IMusicPlayer AddMusic(TrackInfo track);
        IMusicPlayer QueueMusic();
        IMusicPlayer PlayMusic(MyPlayer player);
        IEnumerable<LavalinkTrack> GetMusics();
    }
}
