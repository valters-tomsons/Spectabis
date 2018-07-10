namespace Spectabis_WPF.Domain.Scraping {
    public class GameInfoModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OriginalUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public ScrapeSource ScrapeSource { get; set; }
    }
}
