using System;
using System.IO;

namespace Spectabis_WPF.Domain
{
    class GameProfile
    {
        private static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string emuDir = Properties.Settings.Default.emuDir;
        private static int index = 0;
        private static string GlobalController = BaseDirectory + @"resources\configs\#global_controller\LilyPad.ini";

        //Creates a folder and a blank file for Global Controller settings
        public static void CreateGlobalController()
        {
            if (File.Exists(GlobalController) == false)
            {
                Directory.CreateDirectory(BaseDirectory + @"resources\configs\#global_controller\");
                File.Create(GlobalController);
                Console.WriteLine("Created global controller profile file");
            }
        }

        //Copy global controller profile to a game profile
        public static void CopyGlobalProfile(string game)
        {
            if(Properties.Settings.Default.globalController == true)
            {
                Console.WriteLine("Global settings copied to " + game);
                File.Copy(GlobalController, BaseDirectory + @"\resources\configs\" + game + @"\LilyPad.ini", true);
            }
            else
            {
                Console.WriteLine("Global settings are not copied to " + game);
            }
        }

        //Creates a game profile and returns the created title, because of indexation
        public static string Create(string _img, string _isoDir, string _title)
        {
            //sanitize game's title for folder creation
            Console.WriteLine("Sanitizing Game Title");
            _title = _title.ToSanitizedString();

            //Create a folder for profile and add an index, if needed
            if (getIndex(BaseDirectory + @"\resources\configs\" + _title) != 0)
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
                    Console.WriteLine(inifile + " found!");
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
                        string _destinationPath = Path.Combine(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile));
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

            
            if(_img == null || File.Exists(_img))
            {
                //Copy tempart from resources
                Properties.Resources.tempArt.Save(BaseDirectory + @"\resources\_temp\art.jpg");
                File.Copy(BaseDirectory + @"\resources\_temp\art.jpg", BaseDirectory + @"\resources\configs\" + _title + @"\art.jpg", true);
            }
            else
            {
                File.Copy(_img, BaseDirectory + @"\resources\configs\" + _title + @"\art.jpg", true);
            }
            
            return _title;
            
        }

        public static void Delete(string _title)
        {
            if(isFolder(BaseDirectory + @"\resources\configs\" + _title))
            {
                Directory.Delete(BaseDirectory + @"\resources\configs\" + _title, true);
            }
        }

        public static string Rename(string _in, string _out)
        {

            string input = BaseDirectory + @"\resources\configs\" + _in;
            string output = BaseDirectory + @"\resources\configs\" + _out;

            if (isFolder(input))
            {
                if (getIndex(BaseDirectory + @"\resources\configs\" + _out) != 0)
                {
                    output = BaseDirectory + @"\resources\configs\" + _out + " (" + index + ")";
                    Directory.Move(input, output);
                    return _out + " (" + index + ")";
                }

                Directory.Move(input, output);
                return _out;
            }

            return null;
        }

        //--

        //No idea why i did this
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

            //Enumerate folder index
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
