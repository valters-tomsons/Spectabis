using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectabis_WPF.Domain
{
    class IllegalCharacters
    {
        //Illegal chars for Windows
        public static List<char> IllegalDirectory = new List<char>() {
            '/', '\\', ':', '|', '*', '<', '>', '?', '"'
        };
    }
}
