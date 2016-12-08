using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiantBomb.Api.Model;

namespace GiantBomb.Api {
    public partial class GiantBombRestClient {

        public Release GetRelease(int id, string[] limitFields = null)
        {
            return GetReleaseAsync(id, limitFields).Result;
        }

        public async Task<Release> GetReleaseAsync(int id, string[] limitFields = null) {
            return await GetSingleResourceAsync<Release>("release", ResourceTypes.Releases, id, limitFields).ConfigureAwait(false);
        }

        public IEnumerable<Release> GetReleasesForGame(int gameId, string[] limitFields = null)
        {
            return GetReleasesForGameAsync(gameId, limitFields).Result;
        }

        public async Task<IEnumerable<Release>> GetReleasesForGameAsync(int gameId, string[] limitFields = null)
        {
            var filter = new Dictionary<string, object>()
                             {
                                 {"game", gameId}
                             };

            return await GetListResourceAsync<Release>("releases", fieldList: limitFields, filterOptions: filter).ConfigureAwait(false);
        }

        public IEnumerable<Release> GetReleasesForGame(Game game, string[] limitFields = null)
        {
            return GetReleasesForGame(game.Id, limitFields);
        }

        public Task<IEnumerable<Release>> GetReleasesForGameAsync(Game game, string[] limitFields = null)
        {
            return GetReleasesForGameAsync(game.Id, limitFields);
        }
    }
}
