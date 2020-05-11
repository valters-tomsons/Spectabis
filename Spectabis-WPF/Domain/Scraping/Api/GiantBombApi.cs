using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Spectabis_WPF.Domain.Scraping.Api {
    public class GiantBombApi : IScraperApi {

        private static string DefaultApiKey { get { return ScrapeArt.DecryptApiKey("ZGM1MjAwYjk3N2JjNzdiNGE3YzM3MmYxNDJlYjRhYTI4MDlmNGE4MA=="); } }
        private static string ApiKey { get {
                var config = Properties.Settings.Default.APIKey_GiantBomb;
                if (string.IsNullOrEmpty(config))
                    return DefaultApiKey;
                return config;
        } }

        private const string _baseUrl = "https://www.giantbomb.com/";

        public GameInfoModel GetDataFromApi(string title) {
            var qStrApiKey = "?api_key="+ApiKey;
            var qStrFilter = "&filter=name:"+title;
            var qStrFormat = "&format=json";
            var qStrFields = "&field_list=id,name,image,site_detail_url";
            using (new SecureTLSDumbfuckery())
            using (var webClient = new WebClient()) {
                webClient.BaseAddress = _baseUrl;
                var response = webClient.DownloadString("api/games" + qStrApiKey + qStrFilter + qStrFormat + qStrFields);
                var json = JsonConvert.DeserializeObject<GiantBombRequestStatusJson<GiantBombGameJson[]>>(response);
                if (json == null)
                    return null;
                if (json.Error != "OK")
                    throw new Exception("Giantbomb error: \"" + json.Error + "\"");

                var firstGame = json.Results.FirstOrDefault();
                if (firstGame == null)
                    return null;
                return new GameInfoModel {
                    Id = firstGame.Id,
                    OriginalUrl = firstGame.SiteDetailUrl,
                    ScrapeSource = ScrapeSource.GiantBomb,
                    ThumbnailUrl = firstGame.Image.SmallUrl,
                    Title = firstGame.Name
                };
            }
        }

        private class GiantBombRequestStatusJson<T> where T:class {
            [JsonProperty("error")] public string Error { get; set; }
            [JsonProperty("status_code")] public int StatusCode { get; set; }
            [JsonProperty("results")] public T Results { get; set; }
        }

        private class GiantBombGameJson {
            [JsonProperty("id")] public int Id { get; set; }
            [JsonProperty("name")] public string Name { get; set; }
            [JsonProperty("image")] public GiantBombImagesJson Image { get; set; }
            [JsonProperty("site_detail_url")] public string SiteDetailUrl { get; set; }
        }

        private class GiantBombImagesJson {
            [JsonProperty("small_url")] public string SmallUrl { get; set; }
        }
    }
}
