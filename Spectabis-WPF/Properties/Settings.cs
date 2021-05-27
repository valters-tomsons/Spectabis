using Newtonsoft.Json;
using System;
using System.IO;

namespace Spectabis_WPF.Properties {
    public sealed class Settings {
         
        private static Settings DefaultSettings() {
            return new Settings {
                showTitle = true,
                doubleClick = true,
                emuDir = null,
                nightMode = false,
                autoBoxart = true,
                swatch = "bluegrey",
                gameDirectory = null,
                searchbar = true,
                tooltips = false,
                aprilfooled = false,
                checkupdates = true,
                titleAsFile = false,
                globalController = false,
                playtime = false,
                APIKey_GiantBomb = "",
                APIKey_MobyGames = "",
                APIKey_IGDB = "",
                APIKey_TheGamesDb = "",
                APIAutoSequence = "0,1,2,3,4",
                APIUserSequence = "",
                BoxWidth = 150,
                BoxHeight = 200
            };
        }

        public bool showTitle { get; set; }
        public bool doubleClick { get; set; }
        public string emuDir { get; set; }
        public bool nightMode { get; set; }
        public bool autoBoxart { get; set; }
        public string swatch { get; set; }
        public string gameDirectory { get; set; }
        public bool searchbar { get; set; }
        public bool tooltips { get; set; }
        public bool aprilfooled { get; set; }
        public bool checkupdates { get; set; }
        public bool titleAsFile { get; set; }
        public bool globalController { get; set; }
        public bool playtime { get; set; }
        public string APIKey_GiantBomb { get; set; }
        public string APIKey_MobyGames { get; set; }
        public string APIKey_IGDB { get; set; }
        public string APIKey_TheGamesDb { get; set; }
        public string APIAutoSequence { get; set; }
        public string APIUserSequence { get; set; }
        public int BoxWidth { get; set; } = 150;
        public int BoxHeight { get; set; } = 200;

        private static string DefaultSaveFile => Path.Combine(App.BaseDirectory, "spectabis.json");
        private static Settings _defaultInstance = null;

        public static Settings Default {
            get {
                if (_defaultInstance != null)
                    return _defaultInstance;

                Settings settings;
                if (File.Exists(DefaultSaveFile) == false)
                    settings = DefaultSettings();
                else {
                    var str = File.ReadAllText(DefaultSaveFile);
                    settings = JsonConvert.DeserializeObject<Settings>(str);
                }
                _defaultInstance = settings;
                return settings;
            }
        }

        // TODO UAC
        public void Save() {
            _defaultInstance = this;
            var str = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(DefaultSaveFile, str);
        }
    }
}
