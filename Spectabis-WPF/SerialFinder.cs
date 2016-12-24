using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectabis_WPF {

	// From PCSX2Bonus
	public static class SerialFinder {

		public static readonly string[] AcceptableSerials = { 
			"SCUS", "SLUS", "PCPX", "SCAJ", "SCKA", "SCPS", "SLAJ", "SLKA", "SLPM", "SLPS", "TCPS", "PBPS", "SCED", "SCES", "SLED", "SLES", "TCES"
		};

		public static async Task<string> ExtractSerial(string file){
			return await Task.Run(() => { 
				string result;
				using (var streamReader = new System.IO.StreamReader(file)) {
					streamReader.BaseStream.Position = 30000L;
					string s2;
					while ((s2 = streamReader.ReadLine()) != null) {
						if (streamReader.BaseStream.Position > 600000L)
							break;
						var bytes = Encoding.UTF8.GetBytes(s2);
						var rawString = Encoding.UTF8.GetString(bytes);
						var text = AcceptableSerials.FirstOrDefault(rawString.Contains);
						if (string.IsNullOrEmpty(text)) continue;
						var array = rawString.Substring(rawString.IndexOf(text)).Split(' ');
						if (array[0].Length == text.Length) continue;
						var text2 = array[0].Replace(".", "").Replace("_", "-");
						if (text2.Contains(";"))
							text2 = text2.Remove(text2.IndexOf(';'));
						if (text2 == "SLES-5314")
							text2 = "SLES-53142";
						try {
							var text3 = text2.Substring(text2.IndexOf("-")).Replace("-", "");
							if (char.IsLetter(text3[0]))
								continue;
						}
						catch {
							Console.WriteLine(@"failed, string is empty");
						}
						result = text2.Trim();
						return result;
					}
				}
				result = string.Empty;
				return result;
			});
		}

	}
}
