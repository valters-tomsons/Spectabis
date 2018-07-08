using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectabis_WPF.Domain.Scraping.Api {
    public class MobyGamesApi : IScraperApi {
        private static string DefaultApiKey { get { return ScrapeArt.DecryptApiKey(""); } }
        private static string ApiKey {
            get {
                var config = Properties.Settings.Default.APIKey_MobyGames;
                if (string.IsNullOrEmpty(config))
                    return DefaultApiKey;
                return config;
            }
        }

        public GameInfoModel GetDataFromApi(string title) {
            throw new NotImplementedException();
        }
    }
}
