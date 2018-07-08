using Spectabis_WPF.Domain.Scraping;
using System;
using System.IO;
using System.Net;
using System.Linq;

namespace Spectabis_WPF.Domain {
    class ScrapeArt {
        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public GameInfoModel Result;

        private static readonly IScraperApi[] Scrapers = new IScraperApi[] {
            new Scraping.Api.IGDBApi(),
            new Scraping.Api.GiantBombApi(),
          //new Scraping.Api.TheGamesDbApi(),
          //new Scraping.Api.MobyGamesApi(),
          //new Scraping.Api.GoogleDatastore()
        };

        public ScrapeArt(string title) {
            foreach (var scraper in Scrapers) {
                Result = scraper.GetDataFromApi(title);
                if (Result == null)
                    continue;
                SaveImageFromUrl(title, Result.ThumbnailUrl);
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

        public static string EncryptApiKey(string plain) {
            var random = new Random(38946897);
            var s1 = new string(plain.OrderBy(p=>random.Next()).ToArray());
            var s2 = System.Text.Encoding.UTF8.GetBytes(s1);
            var s3 = System.Convert.ToBase64String(s2);
            return s3;
        }

        public static string DecryptApiKey(string encrypted) {
            var random = new Random(38946897);
            var s3 = System.Convert.FromBase64String(encrypted);
            var s2 = System.Text.Encoding.UTF8.GetString(s3);
            var seq = Enumerable.Range(0, s2.Length)
                .OrderBy(p=>random.Next())
                .ToArray();
            var s1 = new char[s2.Length];
            for (var i = 0; i < s1.Length; i++)
                s1[seq[i]] = s2[i];
            return new string(s1);
        }
    }
}
