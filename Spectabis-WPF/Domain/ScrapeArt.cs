using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TheGamesDBAPI;

namespace Spectabis_WPF.Domain
{
    class ScrapeArt
    {
        private string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public string ImagePath = null;

        public ScrapeArt(string _name)
        {
            if (Properties.Settings.Default.artDB == "TheGamesDB")
            {
                try
                {
                    string _title;
                    string _imgdir;

                    foreach (GameSearchResult game in GamesDB.GetGames(_name, "Sony Playstation 2"))
                    {

                        //Gets game's database ID
                        Game newGame = GamesDB.GetGame(game.ID);

                        _title = _name.Replace(@"/", string.Empty);
                        _title = _title.Replace(@"\", string.Empty);
                        _title = _title.Replace(@":", string.Empty);


                        //Sets image to variable
                        _imgdir = "http://thegamesdb.net/banners/" + newGame.Images.BoxartFront.Path;

                        //Downloads the image
                        using (WebClient client = new WebClient())
                        {
                            try
                            {
                                client.DownloadFile(_imgdir, BaseDirectory + @"\resources\_temp\" + _name + ".jpg");
                                File.Copy(BaseDirectory + @"\resources\_temp\" + _name + ".jpg", BaseDirectory + @"\resources\configs\" + _name + @"\art.jpg", true);
                                ImagePath = BaseDirectory + @"\resources\configs\" + _name + @"\art.jpg";

                                Debug.WriteLine("Downloaded boxart for " + _name);
                            }
                            catch
                            {
                                Debug.WriteLine("Could not download the boxart");
                            }
                        }
                        break;
                    }
                }
                catch
                {
                    Debug.WriteLine("Failed to connect to TheGamesDB");
                }
            }
        }
    }
}
