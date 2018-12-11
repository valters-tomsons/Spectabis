using Spectabis_WPF.Domain;
using System.Windows;

namespace Spectabis_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
	    public App(){
            //Copy spinner.gif to temporary files
            //Spectabis_WPF.Properties.Resources.spinner.Save(BaseDirectory + @"resources\_temp\spinner.gif");
            var ini = new IniFile(BaseDirectory + @"\advanced.ini");
            bool outputToConsole = true;

            if (ini.KeyExists("outputErrorToConsole", "Logging"))
                bool.TryParse(ini.Read("outputErrorToConsole", "Logging"), out outputToConsole);

            else
                ini.Write("outputErrorToConsole", "true", "Logging");

            Logger.Init(outputToConsole);

            return;
        }

		public string BaseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
    }
}
