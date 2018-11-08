using Newtonsoft.Json;
using System;
using System.Linq;

namespace Spectabis_WPF.Domain.Scraping.Api {
    public class IGDBApi : IScraperApi {
        private static string DefaultApiKey { get { return ScrapeArt.DecryptApiKey("ZDQ5NjA0YTJiMGM4MWRiODFiNzYwNzdmMzFlYzU1NDA="); } }
        private static string ApiKey {
            get {
                var config = Properties.Settings.Default.APIKey_IGDB;
                if (string.IsNullOrEmpty(config))
                    return DefaultApiKey;
                return config;
            }
        }
        private const string BaseUrl = "https://api-endpoint.igdb.com/";

        public GameInfoModel GetDataFromApi(string title) {
            // https://igdb.github.io/api/references/filters/#text-search
            var search = "?search=" + title.Replace(" ", "+");

            //https://igdb.github.io/api/references/fields/
            var fields = "&fields=id,name,cover,url,platforms";

            // https://igdb.github.io/api/references/filters/
            var filter = "&filter[platforms][in]=8"; // id 8 == ps2

            // https://igdb.github.io/api/references/pagination/
            var limit = "&limit=1";

            var parsed = ScrapeArt.WebClient<IGDB.IGDBGame[]>(BaseUrl, p=> {
                // https://igdb.github.io/api/examples/
                p.Headers["user-key"] = ApiKey;
                p.Headers["Accept"] = "application/json";
                return p.DownloadString("games/" + search + fields + filter + limit);
            });

            var first = parsed.FirstOrDefault();
            if (first == null)
                return null;

            //https://igdb.github.io/api/references/images/
            const string sizeString = "cover_big";
            var thumbnail = "https://" + "images.igdb.com/igdb/image/upload/t_" + sizeString + "/" + first.Cover.CloudinaryId + ".jpg";

            return new GameInfoModel {
                Id = first.Id,
                Title = first.Name,
                OriginalUrl = first.Url,
                ScrapeSource = ScrapeSource.IGDB,
                ThumbnailUrl = thumbnail
            };
        }

        private static class IGDB {
            // https://igdb.github.io/api/endpoints/game/
            public class IGDBGame {
                public int Id { get; set; }
                public string Name { get; set; }
                public string Url { get; set; }
                public IGDBImage Cover { get; set; }
                public UInt64[] Platforms { get; set; }
            }

            // https://igdb.github.io/api/misc-objects/image/
            public class IGDBImage {
                public string Url { get; set; }
                [JsonProperty("cloudinary_id")]
                public string CloudinaryId { get; set; }
                public uint Width { get; set; }
                public uint Height { get; set; }
            }
        }
    }
}
