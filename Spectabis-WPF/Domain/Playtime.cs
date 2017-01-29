using System;
using System.Collections.Generic;
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

            //Read playtime from file and convert to time
            IniFile spectabis = new IniFile(BaseDirectory + @"/resources/" + game + "/spectabis.ini");
            long SavedTime = Convert.ToInt16(spectabis.Read("playtime", "Spectabis"));
            time = TimeSpan.FromMinutes(SavedTime);

            return time;
        }

        //Adds current session playtime to total playtime
        public static void AddPlaytime(string game, TimeSpan lenght)
        {
            TimeSpan time = new TimeSpan();

            //Read playtime from file and convert to time
            IniFile spectabis = new IniFile(BaseDirectory + @"/resources/" + game + "/spectabis.ini");
            long SavedTime = Convert.ToInt16(spectabis.Read("playtime", "Spectabis"));

            //Add up time
            time = TimeSpan.FromMinutes(SavedTime) + lenght;

            //Save new time to file
            spectabis.Write("playtime",$"{time.ToString()}","Spectabis");

            return;
        }



    }
}
