using System;

namespace GiantBomb.Api.Model {
    public class Region {
        public int Id { get; set; }
        public string ApiDetailUrl { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}