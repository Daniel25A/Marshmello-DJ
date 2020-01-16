using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Modules
{
    public class CoreModule
    {
        //identificadores
        public static string identifUsuario = "%usuario%";
        public static string identifmusicInfo = "%musicinfo%";
        public static string idenfitCanalvoz = "%canal%";
        public static string identifping = "%ping%";
        //
        public static string entroCanal = "Me Conecte al Canal ``%canal%``";
        public static string necesitaEntrar = "**%usuario%**, Necesitas estar conectado a un canal de voz para usar este comando";
        public static string musicAdd = "Adicione ``%musicinfo%`` en la fila.";
        public static string playNow = "Reproduciento Ahora: ``%musicinfo%``. Pedido por **%usuario%**";
        public static string FilaVacia = "La Fila de Reproducción en este servidor esta vacia.";
        public static string NoPlay = "Ninguna Musica Esta Siendo Reproducida Ahora.";
        public static string NoPause = "Ninguna Musica ha Sido Pausada.";
        public static string Canal = "Me Desconecte del Canal de Voz y Limpie todas las Musicas.";
        public static string Ping = "La Latencia Actual de **%usuario%** Es de: ``%ping%`` ms.";
        public static string yaVoto = "Usted ya ha Votado para Saltar la Musica: %musicinfo%";
        public static string Replay = "Se ha Vuelto a Reproducir de Nuevo la Musica:  **%musicinfo%**";
        public static string Pause = "Se ha Pausado la Musica:  **%musicinfo%**";
        public static string Resume = "Se ha Vuelto a Reproducir la Musica:  **%musicinfo%**";
        //  public static string SkipMsg = "Se Salto la Musica Actual.";
        public static string errorMsg = "Ocurrio Un error Inesperado, Vuelva a Intertarlo.";
        public static string Skip = "Se Hizo Skip Hasta la Canción:``%musicinfo%``";
        public static string NoSePuedeAlterar = "No se puede Realizar esta Operación, Verifique que su acción sea Valida.";
        //Emojis
        public static string emojiresume = "<:resume:658421375312330783>";
        public static string emojipause = "<:pause:658421375274450956>";
        public static string emojiantena = "<:conectado:657777217819902032>";
        public static string emojierror = "<:error:657777707110629385>";
        public static string emojiadd = "<:agregado:657777217874427904>";
        public static string emojiTocando = "<:tocando:657777217945731102>";
        public static string okemoji = "<:okemoji:658138696029306888>";
        public static string emojiadvertencia = "<:advertencia:658406297892290583>";
        public static string emojiAuricular = "<:auriculares:658330680455069706>";
        public static string emojisiguiente = "<:siguienteskip:658341975812079626>";
        public static string emojiNet = "<:ping:658392841499967491>";
        public static string emojiTool = "<:otros:658395600420864000>";
        public static string emojiAlready = "<:already:658404499806879797>";
        public static string emojiLoadMusic = "<a:loadmusic:653399136737165323>";
        public static string nowPlayingEmoji = "<:musicnow:659049594180599819>";
        public static string TiempoEmoji = "<:time:659051104327172108>";
        public static string infoLetraEmoji = "<:infoletra:659061408557694976>";
        public static string ResultEmoji = "<:result:659267159234707456>";
        public static string[] numeros = { "<:1_:659221409943257118>", "<:2_:659221409989656576>", "<:3_:659221410262024221>",
                "<:4_:659221410291384360>", "<:5_:659221409947713536>" };
    }
}
