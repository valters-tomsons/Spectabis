using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectabis_WPF.Domain
{
    class Playtime
    {
        private static string BaseDirectory = Views.MainWindow.BaseDirectory;

        //Gets total playtime
        public static TimeSpan GetPlaytime(string game)
        {
            TimeSpan time = new TimeSpan();

            //Read playtime minutes from file and convert to time
            IniFile spectabis = new IniFile(BaseDirectory + @"\resources\configs\" + game + @"\spectabis.ini");
            string Readtime = null;

            //If key doesn't exist, set playtime to 0
            if(spectabis.KeyExists("playtime", "Spectabis"))
            {
                Readtime = spectabis.Read("playtime", "Spectabis");
            }
            else
            {
                Readtime = "0";
            }
            
            //Convert minutes to TimeSpan
            time = TimeSpan.FromMinutes(Convert.ToDouble(Readtime));

            Debug.WriteLine($"ReadTime String: {Readtime}, ParsedTime: {time}, ReadExists: {File.Exists(BaseDirectory + @"\resources\configs\" + game + @"\spectabis.ini")}, TimeKey Exists: {spectabis.KeyExists("playtime", "Spectabis")}");

            return time;
        }

        //Adds current session playtime to total playtime
        public static void AddPlaytime(string game, TimeSpan lenght)
        {
            TimeSpan time = new TimeSpan();

            var ReadTime = GetPlaytime(game);

            //Add up time
            time = ReadTime + lenght;

            //Save new time to file
            IniFile spectabis = new IniFile(BaseDirectory + @"\resources\configs\" + game + @"\spectabis.ini");

            //Save Timespan in minutes
            spectabis.Write("playtime",$"{time.Minutes.ToString()}","Spectabis");

            Debug.WriteLine($"Readtime: {ReadTime}, NewTime: {time}, WriteExists: {File.Exists(BaseDirectory + @"\resources\configs\" + game + @"\spectabis.ini")}");

            return;
        }



    }
}
