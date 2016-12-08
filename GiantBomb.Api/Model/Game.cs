using System;
using System.Collections.Generic;
using System.Linq;

namespace GiantBomb.Api.Model {
    public class Game {
        public int Id { get; set; }

        /// <summary>
        /// Newline-delimited list of aliases
        /// </summary>
        public string Aliases { get; set; }
        public string Name { get; set; }
        public string ApiDetailUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string Deck { get; set; }
        public string Description { get; set; }
        public List<Developer> Developers { get; set; }
        public int? ExpectedReleaseDay { get; set; }
        public int? ExpectedReleaseMonth { get; set; }
        public int? ExpectedReleaseQuarter { get; set; }
        public int? ExpectedReleaseYear { get; set; }
        public List<Franchise> Franchises { get; set; }
        public List<Genre> Genres { get; set; }
        public Image Image { get; set; }
        public List<Image> Images { get; set; }
        public int NumberOfUserReviews { get; set; }
        public string OriginalGameRating { get; set; }
        public DateTime? OriginalReleaseDate { get; set; }
        public List<Platform> Platforms { get; set; }
        public List<Publisher> Publishers { get; set; }
        public List<Release> Releases { get; set; }
        public string SiteDetailUrl { get; set; }
        public List<Game> SimilarGames { get; set; }
    }

    public class GameDistinctComparer : IEqualityComparer<Game>
    {
        public bool Equals(Game x, Game y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Game obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
