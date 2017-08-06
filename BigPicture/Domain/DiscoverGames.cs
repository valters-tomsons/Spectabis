using BigPicture.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BigPicture.Domain
{
    class DiscoverGames
    {
        private string ProfileFolder = null;

        public DiscoverGames()
        {
            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            ProfileFolder = $"{basedir}resources\\configs\\";
        }

        public List<GameProfile> AllGames()
        {
            List<GameProfile> allGames = new List<GameProfile>();
            if(Directory.Exists(ProfileFolder))
            {
                foreach (var folder in Directory.GetDirectories(ProfileFolder))
                {
                    if(!folder.Contains("#"))
                    {
                        GameProfile profile = new GameProfile();

                        profile.Title = folder.ToString().Replace(ProfileFolder, String.Empty);
                        profile.ImageLink = $"{folder}\\art.jpg";
                        profile.Image = NewImage(profile.ImageLink);
                        profile.SpectabisIniPath = $"{folder}\\spectabis.ini";
                        profile.ProfilePath = folder;
                        profile.Platform = "Playstation 2";

                        allGames.Add(profile);
                    }
                }
                return allGames;
            }
            else
            {
                Console.WriteLine("Profile Folder not found...");
                return allGames;
            }
            
        }

        private BitmapImage NewImage(string imageLink)
        {
            BitmapImage artSource = new BitmapImage();
            artSource.BeginInit();

            //Forces cache not to save
            artSource.CacheOption = BitmapCacheOption.None;
            artSource.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            artSource.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            artSource.CacheOption = BitmapCacheOption.OnLoad;
            artSource.UriSource = new Uri(imageLink, UriKind.RelativeOrAbsolute);
            artSource.EndInit();

            return artSource;
        }
    }
}
