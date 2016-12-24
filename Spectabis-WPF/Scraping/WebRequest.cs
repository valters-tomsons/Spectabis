using System;
using System.Threading.Tasks;

namespace Spectabis_WPF.Scraping {
	public static class WebRequest{
		private const string GameArtDir = "\\GameArt\\";

		public static async Task<string> GetImage(string gameId, string url) {
			var req = System.Net.WebRequest.Create(url);
			var dir = Environment.CurrentDirectory + GameArtDir;

			if (System.IO.Directory.Exists(dir) == false)
				System.IO.Directory.CreateDirectory(dir);

			var imagePath = Environment.CurrentDirectory + GameArtDir + gameId + "_medium.jpg";
			try {
				var resp = await req.GetResponseAsync();
				if (resp == null) return null;

				var respStream = resp.GetResponseStream();
				if (respStream == null) return null;

				var image = new System.Drawing.Bitmap(respStream);
				image.Save(imagePath);

				return imagePath;
			}
			catch (Exception) {
				return null;
			}
		}
	}
}
