using System.Collections.Generic;

namespace Spectabis_WPF.Domain
{
    class SupportedGames
    {
        //File types that Spectabis supports
        public static List<string> GameFiles = new List<string>() {
            "iso", "bin", "cso", "gz", "chd",
            "ISO", "BIN", "CSO", "GZ", "CHD"

        };

        //File types that can be scraped for game serial
        public static List<string> ScrappingFiles = new List<string>() {
            "iso",
            "ISO"
        };
    }
}
