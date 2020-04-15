using Spectabis_WPF.Domain;
using System;
using System.IO;
using System.Windows;

namespace Spectabis_WPF
{
    public partial class App : Application {

        static App() {
            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (TestReadWritePermission() == false) {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                BaseDirectory = Path.Combine(appData, "Spectabis");
            }
        }

        public static readonly string BaseDirectory;

        public App() {
            //Copy spinner.gif to temporary files
            //Spectabis_WPF.Properties.Resources.spinner.Save(BaseDirectory + @"resources\_temp\spinner.gif");
            var ini = new IniFile(Path.Combine(BaseDirectory, "advanced.ini"));
            bool outputToConsole = true;

            if (ini.KeyExists("outputErrorToConsole", "Logging"))
                bool.TryParse(ini.Read("outputErrorToConsole", "Logging"), out outputToConsole);

            else
                ini.Write("outputErrorToConsole", "true", "Logging");

            Logger.Init(outputToConsole);
        }

        // there exists no method to check permissions other than try catch
        private static bool TestReadWritePermission() {
            var text = "Hello World!";
            var path = Path.Combine(BaseDirectory, "helloworld.txt");

            if (File.Exists(path)) {
                try {
                    File.Delete(path);
                }
                catch {
                    return false;
                }
            }

            try {
                File.WriteAllText(path, text);
            }
            catch {
                return false;
            }

            try {
                return File.ReadAllText(path) == text;
            }
            catch {
                return false;
            }
            finally {
                File.Delete(path); // if this fails, then we're screwed
            }
        }

    }
}
