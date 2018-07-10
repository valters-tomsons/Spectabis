using CsQuery;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spectabis_WPF.Domain.Scraping.Api {
    public class TheGamesDbHtml : IScraperApi {
        private const string BaseUrl = "https://thegamesdb.net/";

        public GameInfoModel GetDataFromApi(string filename) {
            var platform = "?platformID[]=11";
            var name = "&name=" + filename.Replace(" ", "+");

            var response = ScrapeArt.WebClient(BaseUrl,
                p=>p.DownloadString("search.php" + platform + name)
            );

            var html = CQ.CreateDocument(response);
            if (html["div#display"].Children().Any() == false)
                return null;


            var sanitizedSearchFilename = SanitizeSeach(filename);
            var resultRoot = html["div#display div > a[href]"];
            var results = Peb(resultRoot).ToArray();

            // result is in the wrong order, so i attempt to find the most relevant of the results
            var orderedByRelevance = (
                from item in results
                let sanitizedSerachTitle = SanitizeSeach(item.Title)
                let levenshteinDistance = LevenshteinDistance.Compute(sanitizedSearchFilename, sanitizedSerachTitle)
                let lengthDifference = sanitizedSearchFilename.Length - sanitizedSerachTitle.Length
                orderby lengthDifference
                orderby levenshteinDistance
                select item
            ).ToArray();

            return orderedByRelevance.FirstOrDefault();
        }

        public static IEnumerable<GameInfoModel> Peb(CQ elms) {
            foreach (var resultRoot in elms.Select(p=>CQ.Create(p))) {
                // <a href="./game.php?id=18139">
                var link = resultRoot.Attr("href");
                var id = int.Parse(link.Split('=')[1]);

                // https://cdn.thegamesdb.net/images/thumb/boxart/front/18139-1.jpg
                var thumbnailUrl = resultRoot[".card-img-top"].Attr("src");
                // https://cdn.thegamesdb.net/images/original/boxart/front/18139-1.jpg
                var originalIrl = thumbnailUrl.Replace("thumb", "original");

                var title = resultRoot[".card-footer.bg-secondary > p:first-of-type"].Text();

                yield return new GameInfoModel {
                    Id = id,
                    Title = title,
                    OriginalUrl = link,
                    ScrapeSource = ScrapeSource.TheGamesDB,
                    ThumbnailUrl = thumbnailUrl
                };
            }
        }

        private static string SanitizeSeach(string str) {
            return new[] { ":", "-", "_" }.Aggregate(str.ToLower(), (acc, p) => acc.Replace(p, " "));
        }
    }
}
