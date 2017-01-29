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
        private string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
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

                foreach (GameSearchResult game in GamesDB.GetGames(title, "Sony Playstation 2"))
                {

                    //Gets game's database ID
                    Game newGame = GamesDB.GetGame(game.ID);

                    _title = title.Replace(@"/", string.Empty);
                    _title = _title.Replace(@"\", string.Empty);
                    _title = _title.Replace(@":", string.Empty);


                    //Sets image to variable
                    _imgdir = "http://thegamesdb.net/banners/" + newGame.Images.BoxartFront.Path;

                    //Downloads the image
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            client.DownloadFile(_imgdir, BaseDirectory + @"\resources\_temp\" + title + ".jpg");
                            File.Copy(BaseDirectory + @"\resources\_temp\" + title + ".jpg", BaseDirectory + @"\resources\configs\" + title + @"\art.jpg", true);

                            Debug.WriteLine("Downloaded boxart for " + title);
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

        private void GiantBomb()
        {
            //Variables
            string ApiKey = Properties.Settings.Default.APIKey_GiantBomb;
            var giantBomb = new GiantBombApi.GiantBombRestClient(ApiKey);

            //list for game results
            List<GiantBombApi.Model.Game> resultGame = new List<GiantBombApi.Model.Game>();

            var PlatformFilter = new Dictionary<string, object>() { { "platform", "PlayStation 2" } };

            //Search for game in DB, get its id, then get the image url
            try
            {
                resultGame = giantBomb.SearchForGames(title).ToList();
            }
            catch
            {
                Debug.WriteLine("Failed to connect to Giantbomb");
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

                            Debug.WriteLine("Using GiantBomb API");
                            Debug.WriteLine("ApiKey = " + ApiKey);
                            Debug.WriteLine("Game ID: " + resultGame.First().Id);
                            Debug.WriteLine(_imgdir);

                            //Downloads the image
                            using (WebClient client = new WebClient())
                            {
                                //GiantBomb throws 403 if user-agent is less than 5 characters
                                client.Headers.Add("user-agent", "PCSX2 Spectabis frontend");

                                try
                                {
                                    client.DownloadFile(_imgdir, BaseDirectory + @"\resources\_temp\" + title + ".jpg");
                                    File.Copy(BaseDirectory + @"\resources\_temp\" + title + ".jpg", BaseDirectory + @"\resources\configs\" + title + @"\art.jpg", true);
                                    return;
                                }
                                catch
                                {
                                    Debug.WriteLine("Failed to connect to Giantbomb");
                                    return;
                                }
                            }
                        }
                    }
                }

            }
            catch
            {
                Debug.WriteLine("Failed to connect to Giantbomb");
            }
        }
    }
}
