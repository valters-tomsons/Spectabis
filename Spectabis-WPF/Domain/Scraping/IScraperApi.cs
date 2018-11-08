using System.Threading.Tasks;

namespace Spectabis_WPF.Domain.Scraping {
    public interface IScraperApi {
        GameInfoModel GetDataFromApi(string title);
    }
}
