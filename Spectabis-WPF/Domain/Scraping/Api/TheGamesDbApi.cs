using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectabis_WPF.Domain.Scraping.Api {
    public class TheGamesDbApi : IScraperApi {
        private static string DefaultApiKey { get { return ScrapeArt.DecryptApiKey(""); } }
        private static string ApiKey {
            get {
                var config = Properties.Settings.Default.APIKey_TheGamesDb;
                if (string.IsNullOrEmpty(config))
                    return DefaultApiKey;
                return config;
            }
        }

        private const string BaseUrl = "";

        public GameInfoModel GetDataFromApi(string title) {
            throw new NotImplementedException();
        }
    }
}
