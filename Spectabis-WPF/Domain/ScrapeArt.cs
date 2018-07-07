using Spectabis_WPF.Domain.Scraping;
using System;
using System.IO;
using System.Net;

namespace Spectabis_WPF.Domain {
    class ScrapeArt {
        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public GameInfoModel Result;

        private static readonly IScraperApi[] Scrapers = new IScraperApi[] {
            new Scraping.Api.TheGamesDbApi(),
            new Scraping.Api.GiantBombApi()
        };

        public ScrapeArt(string title) {
            foreach (var scraper in Scrapers) {
                Result = scraper.GetDataFromApi(title);
                if (Result != null)
                    return;
            }
        }

        //Download art to game profile from an image link
        public static void SaveImageFromUrl(string fileName, string imgUrl) {
            using (WebClient client = new WebClient()) {
                client.Headers.Add("user-agent", "PCSX2 Spectabis frontend");

                try {
                    //Download image to temp folder and copy to profile folder
                    client.DownloadFile(imgUrl, BaseDirectory + @"\resources\_temp\" + fileName + ".jpg");
                    File.Copy(
                        BaseDirectory + @"\resources\_temp\" + fileName + ".jpg",
                        BaseDirectory + @"\resources\configs\" + fileName + @"\art.jpg",
                        true
                    );
                }
                catch {
                    Console.WriteLine("Failed to download boxart.");
                }
            }
        }
    }
}
