using System;
using System.IO;
using System.Diagnostics;

namespace Spectabis_WPF.Domain
{
    class GameProfile
    {
        private static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string emuDir = Properties.Settings.Default.emuDir;
        private static int index = 0;

        //Create a game profile
        public static void Create(string _img, string _isoDir, string _title)
        {
            //sanitize game's title for folder creation
            _title = _title.Replace(@"/", string.Empty);
            _title = _title.Replace(@"\", string.Empty);
            _title = _title.Replace(@":", string.Empty);
            _title = _title.Replace(@"|", string.Empty);
            _title = _title.Replace(@"*", string.Empty);
            _title = _title.Replace(@"<", string.Empty);
            _title = _title.Replace(@">", string.Empty);

            //Create a folder for profile and add an index, if needed
            if(getIndex(BaseDirectory + @"\resources\configs\" + _title) != 0)
            {
                _title = _title + " (" + index + ")";
            }

            Directory.CreateDirectory(BaseDirectory + @"\resources\configs\" + _title);

            //Copies existing ini files from PCSX2
            //looks for inis in pcsx2 directory
            if (Directory.Exists(emuDir + @"\inis\"))
            {
                string[] inisDir = Directory.GetFiles(emuDir + @"\inis\");
                foreach (string inifile in inisDir)
                {
                    Debug.WriteLine(inifile + " found!");
                    if (File.Exists(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile)) == false)
                    {
                        string _destinationPath = Path.Combine(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile));
                        File.Copy(inifile, _destinationPath);
                    }
                }
            }
            else
            {
                string[] inisDirDoc = Directory.GetFiles((Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PCSX2\inis"));
                foreach (string inifile in inisDirDoc)
                {
                    if (File.Exists(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile)) == false)
                    {
                        string _destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile));
                        File.Copy(inifile, _destinationPath);
                    }
                }

            }

            //Create a blank Spectabis.ini file
            var gameIni = new IniFile(BaseDirectory + @"\resources\configs\" + _title + @"\spectabis.ini");
            gameIni.Write("isoDirectory", _isoDir, "Spectabis");
            gameIni.Write("nogui", "0", "Spectabis");
            gameIni.Write("fullscreen", "0", "Spectabis");
            gameIni.Write("fullboot", "0", "Spectabis");
            gameIni.Write("nohacks", "0", "Spectabis");

            //Copy tempart from resources and filestream it to game profile
            Properties.Resources.tempArt.Save(BaseDirectory + @"\resources\_temp\art.jpg");
            File.Copy(BaseDirectory + @"\resources\_temp\art.jpg", BaseDirectory + @"\resources\configs\" + _title + @"\art.jpg", true);
        }

        private static bool isFolder(string _dir)
        {
            if(Directory.Exists(_dir))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static int getIndex(string _dir)
        {
            index = 0;

            a: if (Directory.Exists(_dir))
            {
                index++;
                _dir = _dir + " (" + index + ")";
                goto a;
            }

            return index;
        }

    }
}
