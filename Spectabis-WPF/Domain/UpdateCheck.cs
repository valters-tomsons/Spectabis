using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Spectabis_WPF.Domain
{
    class UpdateCheck
    {
        private static Version CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;

        //Get latest version from spectabis.github.io
        private static Version FetchLatest()
        {
            Console.WriteLine("Fetching latest version...");

            try
            {
                using (WebClient client = new WebClient())
                {
                    using (Stream VersionFile = client.OpenRead("https://raw.githubusercontent.com/Spectabis/spectabis.github.io/master/update/version.txt"))
                    {
                        using (StreamReader reader = new StreamReader(VersionFile))
                        {
                            var _fetched = reader.ReadToEnd();
                            Console.WriteLine($"Fetched Version: {_fetched}");
                            return Version.Parse(_fetched);
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Could not fetch version number.");
                Version none = Version.Parse("0.0.0.0");
                return none;
            }
            
        }

        //Check, if there is a new update
        public static bool isNewUpdate()
        {
            Version fetched = FetchLatest();

            if(fetched == Version.Parse("0.0.0.0"))
            {
                return false;
            }

            if(CurrentVersion == fetched)
            {
                Console.WriteLine("Current version is the latest version!");
                return false;
            }
            else if(CurrentVersion > fetched)
            {
                Console.WriteLine("Current version is later than the public release!");
                return false;
            }
            else
            {
                Console.WriteLine("Current version is older than in spectabis.github.io");
                return true;
            }
        }
    }
}
