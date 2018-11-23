using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectabis_WPF.Domain
{
    public static class Logger
    {
        public static bool OutputToConsole { get; set; }
        public static string LogfilePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + @"\resources\logs\";

        public static void Init(bool toConsole = true) => OutputToConsole = toConsole;

        public static void Log(params string[] message)
        {
            string logfile = LogfilePath + $"{DateTime.Now.ToString("MMddyyyy")}_logfile.txt";

            Directory.CreateDirectory(LogfilePath);

            if (OutputToConsole)
                Console.WriteLine(message);

            else
                File.AppendAllLines(logfile, message.Select(x => $"{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")}:\t {x}"));
        }

        public static void LogException(Exception e, string message = "")
        {
            Log(message,
                "Exception:\t" + e.Message,
                "Inner:\t" + e.InnerException?.Message ?? "--",
                "Stack:\t" + e.StackTrace);
        }
    }
}
