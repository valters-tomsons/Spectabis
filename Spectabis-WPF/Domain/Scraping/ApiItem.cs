using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Spectabis_WPF.Properties;

namespace Spectabis_WPF.Domain.Scraping {
	[DebuggerDisplay("{" + nameof(ApiName) + "}")]
	public class ApiItem {
		public string ApiName { get; set; }
		public string ApiKey {
			get => (string) PropertyInfo?.GetValue(Settings.Default);
			set => PropertyInfo?.SetValue(Settings.Default, value);
		}

		public bool IsFreeApi { get; set; }
		public IScraperApi ScraperApi { get; set; }
		public PropertyInfo PropertyInfo { get; set; }
		public int Id { get; set; }
        public bool Enabled { get; internal set; }
    }
}