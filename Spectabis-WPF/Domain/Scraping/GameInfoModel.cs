namespace Spectabis_WPF.Domain.Scraping {
    public class GameInfoModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public ScrapeSource InfoSource { get; set; }
    }
}
