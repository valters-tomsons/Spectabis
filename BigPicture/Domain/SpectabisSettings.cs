using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BigPicture.Model;

namespace BigPicture.Domain
{
    class SpectabisSettings
    {
        //Dunno if there's a better way, maybe there is

        public static string EmuDir {
            get
            {
                string xml = LatestXml();
                string dir = ReadValue(xml, "emuDir");
                return dir;
            }
            private set
            {

            }
        }

        private static string LatestXml()
        {
            //Extract the emuDir setting from latest Spectabis build xml
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string foo = $"{appdata}\\Spectabis_WPF\\";

            foreach (string path in Directory.GetDirectories(foo))
            {
                foo = path + "\\";
                break;
            }

            List<Version> versions = new List<Version>();
            foreach (string path in Directory.GetDirectories(foo))
            {
                string ver = path.Replace(foo, String.Empty);
                Version bar = Version.Parse(ver);
                versions.Add(bar);
            }

            string largest = versions.Max().ToString();
            foo += $"{largest}\\user.config";
            return foo;
        }

        //Read the value of painfully bad xml
        private static string ReadValue(string xmlPath, string value)
        {
            XmlReader reader = XmlReader.Create(xmlPath);
            bool isNodeFound = false;

            while(reader.Read())
            {
                switch(reader.NodeType)
                {
                    case XmlNodeType.Element:
                        while (reader.MoveToNextAttribute())
                            if(reader.Value == value)
                            {
                                isNodeFound = true;
                            }
                            break;
                    case XmlNodeType.Text:
                        if(isNodeFound)
                        {
                            return reader.Value;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        break;
                }
            }

            return null;
        }
    }
}
