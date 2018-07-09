using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spectabis_WPF.Domain.Scraping.Api {
    public class MobyGamesApi : IScraperApi {
        private static string DefaultApiKey { get { return ScrapeArt.DecryptApiKey("PWJGMkNjbnFHQm05S1k2Vz15N2xRZXhu"); } }
        private static string ApiKey {
            get {
                var config = Properties.Settings.Default.APIKey_MobyGames;
                if (string.IsNullOrEmpty(config))
                    return DefaultApiKey;
                return config;
            }
        }

        private const string BaseUrl = "https://api.mobygames.com/v1/";

        public GameInfoModel GetDataFromApi(string title) {
            var qStrKey = "?api_key="+ApiKey;
            var qStrTitle = "&title=" + title.Replace(" ", "+");
            var qStrLimit = "&limit=1";
            var qStrPlatform = "&platform=7"; // id 7 is PS2

            MobyGamesJson.Game[] parsed;
            using (var req = new WebClient { BaseAddress = BaseUrl }) {
                try {
                    // http://www.mobygames.com/info/api
                    var response = req.DownloadString("games" + qStrKey + qStrTitle + qStrLimit + qStrPlatform);
                    var json = JsonConvert.DeserializeObject<MobyGamesJson.GamesEndpoint>(response);
                    parsed = json.Games;
                }
                catch {
                    if (Debugger.IsAttached)
                        throw;
                    return null;
                }

                var first = parsed.FirstOrDefault();
                if (first == null)
                    return null;

                return new GameInfoModel {
                    Id = (int)first.GameId,
                    Title = first.Title,
                    InfoSource = ScrapeSource.MobyGames,
                    ThumbnailUrl = first.SampleCover?.Image,
                };
            }
        }

        // https://app.quicktype.io/#l=cs&r=json2csharp
        private static class MobyGamesJson {
            public class GamesEndpoint {
                public Game[] Games { get; set; }
            }

            public class Game {
                public string Description { get; set; }
                public string Title { get; set; }

                [JsonProperty("game_id")]
                public long GameId { get; set; }

                [JsonProperty("moby_url")]
                public string MobyUrl { get; set; }

                [JsonProperty("sample_cover")]
                public ImageInfo SampleCover { get; set; }
            }

            public class ImageInfo {
                public string Image { get; set; } // fullsize image

                [JsonProperty("thumbnail_image")]
                public string ThumbnailImage { get; set; } // small thumbnail ~120x150
            }
        }
    }
}
