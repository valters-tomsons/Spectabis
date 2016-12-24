using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiantBombApi
{
    public partial class GiantBombRestClient
    {
        public Model.Region GetRegion(int id, string[] limitFields = null)
        {
            return GetRegionAsync(id, limitFields).Result;
        }

        public async Task<Model.Region> GetRegionAsync(int id, string[] limitFields = null)
        {
            return await GetSingleResourceAsync<Model.Region>("region", ResourceTypes.Regions, id, limitFields).ConfigureAwait(false);
        }

        public IEnumerable<Model.Region> GetRegions(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null)
        {
            return GetRegionsAsync(page, pageSize, limitFields).Result;
        }

        public async Task<IEnumerable<Model.Region>> GetRegionsAsync(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null)
        {
            return await GetListResourceAsync<Model.Region>("regions", page, pageSize, limitFields).ConfigureAwait(false);
        }
    }
}
