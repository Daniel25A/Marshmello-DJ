using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT.Core
{
    public static class Data
    {
   
       static string PathJson = Environment.CurrentDirectory + @"\Data\";
        public static async Task SaveData<T>(T Data, string FileName = "Datos")
        {
            string archivo = JsonConvert.SerializeObject(Data);
             File.WriteAllText(PathJson + @"\" + FileName + ".json", archivo);
        }
        public static object Deserializar<T>(string FileName, out T Collection)
        {
            Collection = default(T);
            FileName = FileName + ".json";
            string path = PathJson + @"\" + FileName;
            try
            {
                if (!File.Exists(path))
                {
                    return null;
                }
                using (StreamReader lector = File.OpenText(path))
                {
                    var json = lector.ReadToEnd();
                    Collection = JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Collection;
        }
    }
}
