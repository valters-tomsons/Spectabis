using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Spectabis_WPF.Domain
{
    class GetGameName
    {
        public static string emuDir { get { return Path.GetDirectoryName(Properties.Settings.Default.emuDir); } }

        //Returns a game name, using PCSX2 database file
        public static string GetName(string _path)
        {
            string GameIndex = "./GameIndex.dbf";
            var serial = "";

            //Get the serial number of the game
            if (SupportedGames.ScrappingFiles.Any(a => _path.EndsWith(a)))
            {
                serial = GetSerial.GetSerialNumber(_path);
            }

            // See if serial is in file name, set if found
            if(string.IsNullOrWhiteSpace(serial))
            {
                var fileName = Path.GetFileName(_path);

                var normalizedBuilder = new StringBuilder(fileName.ToUpperInvariant());

                normalizedBuilder.Replace(".", string.Empty);
                normalizedBuilder.Replace("_", string.Empty);
                normalizedBuilder.Replace("-", string.Empty);
                normalizedBuilder.Replace(" ", string.Empty);

                var finalName = normalizedBuilder.ToString();

                var serialMatch = GetRegions.regionList.FirstOrDefault(x => finalName.Contains(x));

                if(serialMatch != null)
                {
                    var result = Regex.Match(finalName, $"{serialMatch}[(0-9)]*");

                    if(result.Success)
                    {
                        serial = result.Value.Replace(serialMatch, serialMatch + "-");
                    }
                }
            }

            // Match serial to game database
            if (!string.IsNullOrWhiteSpace(serial))
            {
                //Reads the GameIndex file by line
                using (var reader = new StreamReader(GameIndex))
                {
                    bool serialFound = false;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        //Forges a GameIndex.dbf entry
                        //If forged line appears in GameIndex.dbf stop and read the next line
                        if (line.Contains("Serial = " + serial))
                        {
                            serialFound = true;
                        }
                        //The next line which contains name associated with gameserial
                        else if (serialFound)
                        {
                            //Cleans the data
                            return line.Replace("Name   = ", String.Empty);
                        }
                    }
                }
            }

            //We didn't find a name for the game, just return the file name
            return Path.GetFileNameWithoutExtension(_path);
        }
    }
}
