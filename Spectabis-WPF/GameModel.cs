using System;
using System.Linq;

namespace Spectabis_WPF {

	// From PCSX2Bonus
	public class GameModel {
		public string Title { get; set; }
		public string Serial { get; set; }

		public string Description { get; set; }
		public string ImagePath { get; set; }
		public string Location { get; set; }
		public string MetacriticScore { get; set; }
		public string Notes { get; set; }
		public string Publisher { get; set; }
		public string Region { get; set; }
		public DateTime ReleaseDate { get; set; }
		public int Compatibility { get; set; }

		public TimeSpan TimePlayed { get; set; }

		public string GetFileName(){
			return System.IO.Path.GetInvalidFileNameChars().Aggregate(Title, (current, c) => current.Replace(c.ToString(), ""));
		}
	}
}
