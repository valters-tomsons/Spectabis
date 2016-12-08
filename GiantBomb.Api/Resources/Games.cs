using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiantBomb.Api.Model;
using RestSharp;

namespace GiantBomb.Api {
    public partial class GiantBombRestClient {

        public Game GetGame(int id, string[] limitFields = null)
        {
            return GetGameAsync(id, limitFields).Result;
        }

        public async Task<Game> GetGameAsync(int id, string[] limitFields = null) {
            return await GetSingleResourceAsync<Game>("game", ResourceTypes.Games, id, limitFields).ConfigureAwait(false);
        }

        public IEnumerable<Game> GetGames(int page = 1, int pageSize = GiantBombBase.DefaultLimit, string[] limitFields = null)
        {
            return GetGamesAsync(page, pageSize, limitFields).Result;
        }

        public async Task<IEnumerable<Game>> GetGamesAsync(int page = 1, int pageSize = GiantBombBase.DefaultLimit, string[] limitFields = null) {
            return await GetListResourceAsync<Game>("games", page, pageSize, limitFields).ConfigureAwait(false);
        }
    }
}
