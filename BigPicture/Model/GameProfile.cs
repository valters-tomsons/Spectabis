using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BigPicture.Model
{
    //Game profile model
    public class GameProfile
    {
        public string Title { get; set; }
        public BitmapImage Image { get; set; }
        public string Platform { get; set; }
        public string ImageLink { get; set; }
        public string ProfilePath { get; set; }
        public string SpectabisIniPath { get; set; }
    }
}
