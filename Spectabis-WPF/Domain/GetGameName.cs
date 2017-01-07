using System;
using System.IO;

namespace Spectabis_WPF.Domain
{
    class GetGameName
    {
        private static string emuDir = Properties.Settings.Default.emuDir;

        //Returns a game name, using PCSX2 database file
        public static string GetName(string _gameserial)
        {
            string GameIndex = emuDir + @"\GameIndex.dbf";
            string GameName = "UNKNOWN";

            //Reads the GameIndex file by line
            using (var reader = new StreamReader(GameIndex))
            {

                bool serialFound = false;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    //Forges a GameIndex.dbf entry
                    //If forged line appears in GameIndex.dbf stop and read the next line
                    if (line.Contains("Serial = " + _gameserial))
                    {
                        serialFound = true;
                    }
                    //The next line which contains name associated with gameserial
                    else if (serialFound == true)
                    {
                        //Cleans the data
                        GameName = line.Replace("Name   = ", String.Empty);
                        return GameName;
                    }
                }
                return GameName;
            }
        }
    }
}
