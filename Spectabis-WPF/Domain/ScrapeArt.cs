using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheGamesDBAPI;

namespace Spectabis_WPF.Domain
{
    class ScrapeArt
    {
        private static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string title = null;
        

        public ScrapeArt(string _name)
        {
            title = _name;

            if (Properties.Settings.Default.artDB == "TheGamesDB")
            {
                TheGamesDB();
            }

            if(Properties.Settings.Default.artDB == "GiantBomb")
            {
                GiantBomb();
            }
        }

        private void TheGamesDB()
        {
            try
            {
                string _title;
                string _imgdir;

                //Search all games with given title and filter only PS2 titles
                foreach (GameSearchResult game in GamesDB.GetGames(title, "Sony Playstation 2"))
                {

                    //Gets game's database ID
                    Game newGame = GamesDB.GetGame(game.ID);

                    _title = title.Replace(@"/", string.Empty);
                    _title = _title.Replace(@"\", string.Empty);
                    _title = _title.Replace(@":", string.Empty);


                    //Creates a link for image url
                    _imgdir = "http://thegamesdb.net/banners/" + newGame.Images.BoxartFront.Path;

                    //Downloads the image
                    DownloadArt(title, _imgdir);

                    break;
                }
            }
            catch
            {
                Console.WriteLine("Failed to connect to TheGamesDB");
            }
        }

        private void GiantBomb()
        {
            //Variables
            string ApiKey = Properties.Settings.Default.APIKey_GiantBomb;
            var giantBomb = new GiantBombApi.GiantBombRestClient(ApiKey);

            //list for game results
            List<GiantBombApi.Model.Game> resultGame = new List<GiantBombApi.Model.Game>();

            var PlatformFilter = new Dictionary<string, object>() { { "platform", "PlayStation 2" } };

            //Search the DB for a game ID
            try
            {
                resultGame = giantBomb.SearchForGames(title).ToList();
            }
            catch
            {
                Console.WriteLine("Failed to connect to Giantbomb");
                return;
            }

            GiantBombApi.Model.Game FinalGame;

            try
            {
                //loops through each game in resultGame list
                foreach (GiantBombApi.Model.Game game in resultGame)
                {
                    //Gets game ID and makes a list of platforms it's available for
                    FinalGame = giantBomb.GetGame(game.Id);
                    List<GiantBombApi.Model.Platform> platforms = new List<GiantBombApi.Model.Platform>(FinalGame.Platforms);

                    //If game platform list contains "PlayStation 2", then start scrapping
                    foreach (var gamePlatform in platforms)
                    {
                        if (gamePlatform.Name == "PlayStation 2")
                        {
                            string _imgdir = FinalGame.Image.SmallUrl;

                            Console.WriteLine("Using GiantBomb API");
                            //Console.WriteLine("ApiKey = " + ApiKey);
                            Console.WriteLine("Game ID: " + resultGame.First().Id);
                            Console.WriteLine(_imgdir);

                            DownloadArt(title, _imgdir);
                            return;
                        }
                    }
                }

            }
            catch
            {
                Console.WriteLine("Failed to connect to Giantbomb");
            }
        }

        //Download art to game profile from an image link
        private static void DownloadArt(string title, string _imgdir)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("user-agent", "PCSX2 Spectabis frontend");

                try
                {
                    //Download image to temp folder and copy to profile folder
                    client.DownloadFile(_imgdir, BaseDirectory + @"\resources\_temp\" + title + ".jpg");
                    File.Copy(BaseDirectory + @"\resources\_temp\" + title + ".jpg", BaseDirectory + @"\resources\configs\" + title + @"\art.jpg", true);
                    return;
                }
                catch
                {
                    Console.WriteLine("Failed to download boxart.");
                    return;
                }
            }
        }
    }
}
