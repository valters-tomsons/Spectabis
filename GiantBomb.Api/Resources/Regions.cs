using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiantBomb.Api.Model;

namespace GiantBomb.Api
{
    public partial class GiantBombRestClient
    {
        public Region GetRegion(int id, string[] limitFields = null)
        {
            return GetRegionAsync(id, limitFields).Result;
        }

        public async Task<Region> GetRegionAsync(int id, string[] limitFields = null)
        {
            return await GetSingleResourceAsync<Region>("region", ResourceTypes.Regions, id, limitFields).ConfigureAwait(false);
        }

        public IEnumerable<Region> GetRegions(int page = 1, int pageSize = GiantBombBase.DefaultLimit, string[] limitFields = null)
        {
            return GetRegionsAsync(page, pageSize, limitFields).Result;
        }

        public async Task<IEnumerable<Region>> GetRegionsAsync(int page = 1, int pageSize = GiantBombBase.DefaultLimit, string[] limitFields = null)
        {
            return await GetListResourceAsync<Region>("regions", page, pageSize, limitFields).ConfigureAwait(false);
        }
    }
}
