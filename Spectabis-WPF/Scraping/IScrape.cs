using System.Threading.Tasks;

namespace Spectabis_WPF.Scraping {
	public interface IScrape {
		Task<GameModel> Scrape(string fileName, string gameId);
	}
}
