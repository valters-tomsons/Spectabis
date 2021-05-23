using System;
using System.IO;
using System.Linq;

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
