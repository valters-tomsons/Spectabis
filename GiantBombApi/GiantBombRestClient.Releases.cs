using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiantBombApi {
    public partial class GiantBombRestClient {

        public Model.Release GetRelease(int id, string[] limitFields = null)
        {
            return GetReleaseAsync(id, limitFields).Result;
        }

        public async Task<Model.Release> GetReleaseAsync(int id, string[] limitFields = null) {
            return await GetSingleResourceAsync<Model.Release>("release", ResourceTypes.Releases, id, limitFields).ConfigureAwait(false);
        }

        public IEnumerable<Model.Release> GetReleasesForGame(int gameId, string[] limitFields = null)
        {
            return GetReleasesForGameAsync(gameId, limitFields).Result;
        }

        public async Task<IEnumerable<Model.Release>> GetReleasesForGameAsync(int gameId, string[] limitFields = null)
        {
            var filter = new Dictionary<string, object>()
                             {
                                 {"game", gameId}
                             };

            return await GetListResourceAsync<Model.Release>("releases", fieldList: limitFields, filterOptions: filter).ConfigureAwait(false);
        }

        public IEnumerable<Model.Release> GetReleasesForGame(Model.Game game, string[] limitFields = null)
        {
            return GetReleasesForGame(game.Id, limitFields);
        }

        public Task<IEnumerable<Model.Release>> GetReleasesForGameAsync(Model.Game game, string[] limitFields = null)
        {
            return GetReleasesForGameAsync(game.Id, limitFields);
        }
    }
}
