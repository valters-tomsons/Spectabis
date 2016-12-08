using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiantBomb.Api.Model;
using RestSharp;

namespace GiantBomb.Api {
    public partial class GiantBombRestClient {
        public Platform GetPlatform(int id, string[] limitFields = null)
        {
            return GetPlatformAsync(id, limitFields).Result;
        }

        public async Task<Platform> GetPlatformAsync(int id, string[] limitFields = null) {
            return await GetSingleResourceAsync<Platform>("platform", ResourceTypes.Platforms, id, limitFields).ConfigureAwait(false);
        }

        public IEnumerable<Platform> GetPlatforms(int page = 1, int pageSize = GiantBombBase.DefaultLimit,
            string[] limitFields = null)
        {
            return GetPlatformsAsync(page, pageSize, limitFields).Result;
        }

        public async Task<IEnumerable<Platform>> GetPlatformsAsync(int page = 1, int pageSize = GiantBombBase.DefaultLimit, string[] limitFields = null) {
            return await GetListResourceAsync<Platform>("platforms", page, pageSize, limitFields).ConfigureAwait(false);
        }
    }
}
