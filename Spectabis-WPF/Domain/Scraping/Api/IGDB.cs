using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Spectabis_WPF.Domain.Scraping.Api {
    public class IGDBApi : IScraperApi {
        //private static string DefaultApiKey { get { return ScrapeArt.DecryptApiKey("ZDQ5NjA0YTJiMGM4MWRiODFiNzYwNzdmMzFlYzU1NDA="); } }

        // Spectabis2 : klsdmfslksadsd@yandex.com : 123456
        private static string DefaultApiKey { get { return ScrapeArt.DecryptApiKey("MTNhODRhMDhkNWNhMDRlNjU5ZTczYWRlMTVkNWM1Y2Q="); } }
        private static string ApiKey {
            get {
                var config = Properties.Settings.Default.APIKey_IGDB;
                if (string.IsNullOrEmpty(config))
                    return DefaultApiKey;
                return config;
            }
        }
        private const string BaseUrl = "https://api-v3.igdb.com/";

        public GameInfoModel GetDataFromApi(string title) {
            using (new SecureTLSDumbfuckery())
            using (var req = new WebClient()) {
                req.Headers["user-key"] = ApiKey;
                req.Headers["Accept"] = "application/json";
                req.BaseAddress = BaseUrl;
                var response = req.UploadString("/games", "search \"" + title + "\"; fields id,name,url,cover.image_id; where platforms = (8); limit 2;");
                //var response = req.UploadString("/games", "search \"" + title + "\"; fields id,name,url,platforms.name,cover.image_id; where platforms = (8); limit 5;");
                var json = JsonConvert.DeserializeObject<GameJson[]>(response);
                if (json == null || json.Length == 0)
                    return null;

                var orderedByRelevance = (
                    from item in json
                    let sanitizedSerachTitle = item.Name
                    let levenshteinDistance = LevenshteinDistance.Compute(title, sanitizedSerachTitle)
                    let lengthDifference = title.Length - sanitizedSerachTitle.Length
                    orderby lengthDifference
                    orderby levenshteinDistance
                    select item
                );

                var first = orderedByRelevance.FirstOrDefault();
                var thumbnail = "https://"+"images.igdb.com/igdb/image/upload/t_cover_big/" + first.Cover.ImageId + ".jpg";
                return new GameInfoModel {
                    Id = first.Id,
                    Title = first.Name,
                    OriginalUrl = first.Url,
                    ScrapeSource = ScrapeSource.IGDB,
                    ThumbnailUrl = thumbnail
                };
            }
        }

        private class GameJson {
            public int Id { get; set; }
            public CoverJson Cover { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }

            
        }

        public class CoverJson {
            public int Id { get; set; }
            [JsonProperty("image_id")] public string ImageId { get; set; }
        }

    }
}
