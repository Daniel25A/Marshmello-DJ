using BOT.Modules;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Lavalink4NET.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Core
{
    public class Utilidades
    {
        public static Dictionary<ulong, Queue<TrackInfo>> ServerMusics = new Dictionary<ulong, Queue<TrackInfo>>();
        public static Dictionary<ulong, int> SkipCounter = new Dictionary<ulong, int>();
        public static Dictionary<ulong, int> MemberAudioConter = new Dictionary<ulong, int>();
        public static Dictionary<ulong, List<ulong>> VotesUsers = new Dictionary<ulong, List<ulong>>();
        public static Dictionary<ulong, TrackInfo> RepeatMusic = new Dictionary<ulong, TrackInfo>();
        public static Dictionary<ulong, TrackInfo> NowPlaying = new Dictionary<ulong, TrackInfo>();
        public static Dictionary<ulong, int> ServerVolumes = new Dictionary<ulong, int>();
        public static Dictionary<ulong, Dictionary<int, LavalinkTrack>> Tracks = new Dictionary<ulong, Dictionary<int, LavalinkTrack>>();
        public static Dictionary<ulong, bool> EnBusqueda = new Dictionary<ulong, bool>();
        public static Dictionary<ulong, ulong> MessageSearch = new Dictionary<ulong, ulong>();
        public static Dictionary<ulong, Servidor> ServerManager = new Dictionary<ulong, Servidor>();
        public int Contador = 0;
        public static Dictionary<string, int> NumerosIconos = new Dictionary<string, int>() {
            {"1_",1 },
             {"2_",2 },
              {"3_",3 },
               {"4_",4 },
                {"5_",5 }
        };
        public static Color EmbedColor = new Color(150, 6, 162);
        public static string GetTimeFormat()
        {
            return $"[{DateTime.Now.ToShortDateString()}][{DateTime.Now.ToShortTimeString()}]";
        }
        public static void getSegundos(string Formato, out int Segundos)
        {
            Segundos = -1;
            var Arreglo = Formato.Split(':');
            const int OneMinute = 60;
            if (Arreglo.Length < 2)
            {
                Console.WriteLine("Error a la Conversion");
            }
            else
            {
                int minutos, segundos;
                if (int.TryParse(Arreglo[0], out minutos) && int.TryParse(Arreglo[1], out segundos))
                {
                    if (segundos > 59)
                    {
                        Console.WriteLine("Error a la Conversion.. El segundo debe ser menor o igual a 59");
                        return;
                    }
                    Segundos = (OneMinute * minutos) + segundos;
                }
            }
        }
        public static void Limpiar(ref Queue<TrackInfo> Listado,ref TrackInfo info, ref SocketTextChannel LastChannel)
        {
            if (Listado.Count > 0)
                Listado.Clear();
            if (info != null)
                info = null;
            if (LastChannel != null)
                LastChannel = null;
        }
        public static string GetMusicInfo(LavalinkTrack track,string author)
        {
             
            var musicinfo = $"{track.Title} ({Utilidades.FormatMinutes((int)track.Duration.TotalSeconds)})";
            return $"{CoreModule.emojiTocando}{CoreModule.playNow.Replace(CoreModule.identifmusicInfo, musicinfo).Replace(CoreModule.identifUsuario, author)}";
        }
        public static string GetInfoTrack(LavalinkTrack track)
        {

            var musicinfo = $"{track.Title} ({Utilidades.FormatMinutes((int)track.Duration.TotalSeconds)})";
            return $"{CoreModule.emojiTocando}{CoreModule.musicAdd.Replace(CoreModule.identifmusicInfo, musicinfo)}";
        }
        public static void ClearSkipData(ulong serverId)
        {
            Utilidades.VotesUsers[serverId].Clear();
            Utilidades.SkipCounter[serverId] = 0;
            Console.WriteLine("----- Se Limpio los Datos del Skip ---------");
        }
        public static string FormatMinutes(int seconds)
        {
            const int minuto = 60;
            int resto = seconds % minuto;
            int minutos = seconds / minuto;
            Console.WriteLine($"{GetTimeFormat()} {seconds}");
            string formato = string.Empty;
            if (resto > 9)
                formato = $"{minutos}:{resto}";
            else
                formato = $"{minutos}:0{resto}";
            return formato;
        }
        public static string FormatearPista(int nowP, int Length)
        {
            string IconoEspacios = "▬";
            string IconoProgreso = "⏺";
            string PistaResult = string.Empty;
            int AbsoluteValue = 0;
            //Agarro el Porcentaje del Valor de Length en NowP es decir.. Si Length vale 100 y nowP vale 20.. el valor sera de 20%
            float calculo = ((float)nowP / (float)Length) * 100;
            //Para no Forzar Tanto a la maquina.. Le quito una Cifra..
            if ((int)calculo < 10)
                AbsoluteValue = 0;
            else
                AbsoluteValue = int.Parse(calculo.ToString().Substring(0, 1));
            //Itero el Valor.. Si no es igual Agrego ▬ y si es agrego  ▶️
            for (int i = 0; i <=10; i++)
            {
                if (i == AbsoluteValue)
                    PistaResult += IconoProgreso;
                else
                    PistaResult += IconoEspacios;
            }
            //retorno el valor
            return "▶️" + PistaResult + "🔉";
        }
        public static string GetGrafico(int valor,int repeat)
        {
            //▰▱
            string text = "";
            int i = 0;
            int x1 = repeat < 10 ? 10 : repeat;
            for (i = 0; i < valor; i += x1/10)
            {
                text += "█";
            }
            for (int x = 0; x < (x1); x+= x1 / 10)
            {
                text += "░";
            }
            string result = $"**{valor.ToString()}%** " + text;
            return result;
        }
      
    }
    public class TrackInfo
    {
        public LavalinkTrack track { get; set; }
        public SocketGuild guild { get; set; }
        public int volume { get; set; } = 10;
        public string Request { get; set; } = "None";
        public SocketTextChannel Canal { get; set; }
    }
}
