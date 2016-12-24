using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiantBombApi {
    public partial class GiantBombRestClient {

        public IEnumerable<Model.Game> SearchForGames(string query, int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null)
        {
            return SearchForGamesAsync(query, page, pageSize, limitFields).Result;
        }

        public async Task<IEnumerable<Model.Game>> SearchForGamesAsync(string query, int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null) {
            var result = await InternalSearchForGames(query, page, pageSize, limitFields).ConfigureAwait(false);

            if (result.StatusCode == Model.GiantBombBase.StatusOk)
                return result.Results;

            return null;
        }

        public IEnumerable<Model.Game> SearchForAllGames(string query, string[] limitFields = null)
        {
            return SearchForAllGamesAsync(query, limitFields).Result;
        }

        public async Task<IEnumerable<Model.Game>> SearchForAllGamesAsync(string query, string[] limitFields = null) {
            var results = new List<Model.Game>();
            var result = await InternalSearchForGames(query, limitFields: limitFields).ConfigureAwait(false);

            if (result == null || result.StatusCode != Model.GiantBombBase.StatusOk)
                return null;

            results.AddRange(result.Results);

            if (result.NumberOfTotalResults > result.Limit) {
                double remaining = Math.Ceiling(Convert.ToDouble(result.NumberOfTotalResults) / Convert.ToDouble(result.Limit));

                // Start on page 2
                for (var i = 2; i <= remaining; i++) {
                    result = await InternalSearchForGames(query, i, result.Limit, limitFields).ConfigureAwait(false);

                    if (result.StatusCode != Model.GiantBombBase.StatusOk)
                        break;

                    results.AddRange(result.Results);
                }
            }

            // FIX: Clean duplicates that GiantBomb returns
            // Can only do it if we have IDs in the resultset
            if (limitFields == null || limitFields.Contains("id"))
            {
                results = results.Distinct(new Model.GameDistinctComparer()).ToList();
            }

            return results;
        }

        internal async Task<Model.GiantBombResults<Model.Game>> InternalSearchForGames(string query, int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null)
        {
            var request = GetListResource("search", page, pageSize, limitFields);

            request.AddParameter("query", query);
            request.AddParameter("resources", "game");

            return await ExecuteAsync<Model.GiantBombResults<Model.Game>>(request).ConfigureAwait(false);
        }
    }
}
