using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiantBombApi {
    public partial class GiantBombRestClient {

        public Model.Game GetGame(int id, string[] limitFields = null)
        {
            return GetGameAsync(id, limitFields).Result;
        }

        public async Task<Model.Game> GetGameAsync(int id, string[] limitFields = null) {
            return await GetSingleResourceAsync<Model.Game>("game", ResourceTypes.Games, id, limitFields).ConfigureAwait(false);
        }

        public IEnumerable<Model.Game> GetGames(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null)
        {
            return GetGamesAsync(page, pageSize, limitFields).Result;
        }

        public async Task<IEnumerable<Model.Game>> GetGamesAsync(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null) {
            return await GetListResourceAsync<Model.Game>("games", page, pageSize, limitFields).ConfigureAwait(false);
        }
    }
}
