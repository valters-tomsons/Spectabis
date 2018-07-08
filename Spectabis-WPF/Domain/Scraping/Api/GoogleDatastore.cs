using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectabis_WPF.Domain.Scraping.Api {
    public class GoogleDatastore : IScraperApi {
        private static string DefaultApiKey { get { return ScrapeArt.DecryptApiKey("MzIxMDRhOTA5Nzc1NGU5MGQ0NjI2NDFkMWQwMmExMTgzNjg4ZTZiNw=="); } }
        private static string ApiKey {
            get {
                return DefaultApiKey;
            }
        }

        public GameInfoModel GetDataFromApi(string title) {
            throw new NotImplementedException();
        }
    }
}
