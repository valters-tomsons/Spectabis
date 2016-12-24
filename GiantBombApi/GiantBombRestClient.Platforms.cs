using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiantBombApi {
    public partial class GiantBombRestClient {
        public Model.Platform GetPlatform(int id, string[] limitFields = null)
        {
            return GetPlatformAsync(id, limitFields).Result;
        }

        public async Task<Model.Platform> GetPlatformAsync(int id, string[] limitFields = null) {
            return await GetSingleResourceAsync<Model.Platform>("platform", ResourceTypes.Platforms, id, limitFields).ConfigureAwait(false);
        }

        public IEnumerable<Model.Platform> GetPlatforms(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit,
            string[] limitFields = null)
        {
            return GetPlatformsAsync(page, pageSize, limitFields).Result;
        }

        public async Task<IEnumerable<Model.Platform>> GetPlatformsAsync(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null) {
            return await GetListResourceAsync<Model.Platform>("platforms", page, pageSize, limitFields).ConfigureAwait(false);
        }
    }
}
