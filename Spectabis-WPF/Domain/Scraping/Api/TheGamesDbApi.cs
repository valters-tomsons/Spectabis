using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Spectabis_WPF.Domain.Scraping.Api
{
    public class TheGamesDbApi : IScraperApi
    {
        private static string DefaultApiKey { get { return ScrapeArt.DecryptApiKey("NDRjODIyNTAwZDc2YjM5YTNiZmY3YjE5Mjg3MjBiYjIzNGViZWE2OTcwNWQ0MTY0MTcyYjJmMWQ2NTg5NjRhZg=="); } }
        private static string ApiKey {
            get {
                var config = Properties.Settings.Default.APIKey_TheGamesDb;
                if (string.IsNullOrEmpty(config))
                    return DefaultApiKey;
                return config;
            }
        }

        private const string BaseUrl = "https://api.thegamesdb.net/";

        public GameInfoModel GetDataFromApi(string title) {
            var apiKey = "?apikey=" + ApiKey;
            var filter = "&filter[platform]=11"; // Sony Playstation 2
            var name = "&name=" + title.Replace(" ", "+");

            TheGamesDbJson.Game game;
            using (var webClient = new WebClient()) {
                webClient.BaseAddress = BaseUrl;
                var response = webClient.DownloadString("/v1/Games/ByGameName" + apiKey + filter + name);
                var json = JsonConvert.DeserializeObject<TheGamesDbJson.Root>(response);
                if (json == null)
                    return null;

                if (json.Code != 200)
                    return null;

                if (json.Data == null || json.Data.Games.Any() == false)
                    return null;

                var sanitizedSearchFilename = SanitizeSeach(title);
                var orderedByRelevance = (
                    from item in json.Data.Games
                    let sanitizedSerachTitle = SanitizeSeach(item.GameTitle)
                    let levenshteinDistance = LevenshteinDistance.Compute(sanitizedSearchFilename, sanitizedSerachTitle)
                    let lengthDifference = sanitizedSearchFilename.Length - sanitizedSerachTitle.Length
                    orderby lengthDifference
                    orderby levenshteinDistance
                    select item
                ).ToArray();

                game = orderedByRelevance.FirstOrDefault();
            }

            TheGamesDbJson.BaseUrl baseUrl;
            TheGamesDbJson.BoxArt boxart;
            using (var webClient = new WebClient()) {
                webClient.BaseAddress = BaseUrl;
                var response = webClient.DownloadString("/v1/Games/Images" + apiKey + "&games_id=" + game.Id);
                var json = JsonConvert.DeserializeObject<TheGamesDbJson.Root>(response);

                if (json == null)
                    return null;

                if (json.Code != 200)
                    return null;

                if (json.Data == null || json.Data.Images.Any() == false)
                    return null;

                boxart = json.Data.Images.Values
                    .First()
                    .Where(p => p.Type == "boxart" && p.Side == "front")
                    .First();

                baseUrl = json.Data.BaseUrl;

                // sample url
                //boxart/front/12615-1.jpg
            }

            return new GameInfoModel {
                Id = game.Id,
                Title = game.GameTitle,
                OriginalUrl = "https://" + "thegamesdb.net/game.php?id=" + game.Id,
                ScrapeSource = ScrapeSource.TheGamesDB,
                ThumbnailUrl = baseUrl.Thumb + boxart.Filename
            };
        }

        private static string SanitizeSeach(string str) {
            return new[] { ":", "-", "_" }.Aggregate(str.ToLower(), (acc, p) => acc.Replace(p, " "));
        }


        private class TheGamesDbJson
        {
            public class Root
            {
                public int Code { get; set; }
                public string Status { get; set; }
                public Data Data { get; set; }
                [JsonProperty("extra_allowance")] public int ExtraAllowance { get; set; }
                [JsonProperty("remaining_monthly_allowance")] public int RemainingMonthlyAllowance { get; set; }
            }

            public class Data
            {
                public int Count { get; set; }
                public Game[] Games { get; set; }

                [JsonProperty("base_url")]
                public BaseUrl BaseUrl { get; set; }
                public Dictionary<string, BoxArt[]> Images { get; set; }
            }

            public class Game {
                public int Id { get; set; }
                public int Platform { get; set; }
                public int[] Developers { get; set; }
                [JsonProperty("game_title")] public string GameTitle { get; set; }
                [JsonProperty("release_date")] public DateTimeOffset? ReleaseDate { get; set; }
            }

            public class BaseUrl {
                public string Original { get; set; }
                public string Small { get; set; }
                public string Thumb { get; set; }
                public string Medium { get; set; }
                public string Large { get; set; }
                [JsonProperty("cropped_center_thumb")]
                public string CroppedCenterThumb { get; set; }
            }

            public class BoxArt {
                public int Id { get; set; }
                public string Type { get; set; }
                public string Side { get; set; }
                public string Filename { get; set; }
                public string Resolution { get; set; }
            }
        }
    }
}
