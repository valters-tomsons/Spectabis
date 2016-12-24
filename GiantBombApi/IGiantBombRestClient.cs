using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace GiantBombApi
{
    public interface IGiantBombRestClient
    {
        /// <summary>
        /// Base URL of API (defaults to http://api.giantbomb.com)
        /// </summary>
        string BaseUrl { get; set; }

        /// <summary>
        /// Gets a single platform
        /// </summary>
        /// <param name="id">The platform's ID</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Model.Platform GetPlatform(int id, string[] limitFields = null);

        /// <summary>
        /// Gets a single platform
        /// </summary>
        /// <param name="id">The platform's ID</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Task<Model.Platform> GetPlatformAsync(int id, string[] limitFields = null);

        /// <summary>
        /// Gets list of platforms
        /// </summary>
        /// <returns></returns>
        IEnumerable<Model.Platform> GetPlatforms(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null);

        /// <summary>
        /// Gets list of platforms
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Model.Platform>> GetPlatformsAsync(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null);

        /// <summary>
        /// Get a region
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Model.Region GetRegion(int id, string[] limitFields = null);

        /// <summary>
        /// Get a region
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Task<Model.Region> GetRegionAsync(int id, string[] limitFields = null);

        /// <summary>
        /// Gets list of regions
        /// </summary>
        /// <param name="page">The page to retrieve</param>
        /// <param name="pageSize">The number of results per page (default: 100)</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        IEnumerable<Model.Region> GetRegions(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null);

        /// <summary>
        /// Gets list of regions
        /// </summary>
        /// <param name="page">The page to retrieve</param>
        /// <param name="pageSize">The number of results per page (default: 100)</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Task<IEnumerable<Model.Region>> GetRegionsAsync(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null);

        /// <summary>
        /// Gets a game with the given ID
        /// </summary>
        /// <param name="id">The ID of the game</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Model.Game GetGame(int id, string[] limitFields = null);

        /// <summary>
        /// Gets a game with the given ID
        /// </summary>
        /// <param name="id">The ID of the game</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Task<Model.Game> GetGameAsync(int id, string[] limitFields = null);

        /// <summary>
        /// Gets list of games
        /// </summary>
        /// <param name="page">The page to retrieve</param>
        /// <param name="pageSize">The number of results per page (default: 100)</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        IEnumerable<Model.Game> GetGames(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null);

        /// <summary>
        /// Gets list of games
        /// </summary>
        /// <param name="page">The page to retrieve</param>
        /// <param name="pageSize">The number of results per page (default: 100)</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Task<IEnumerable<Model.Game>> GetGamesAsync(int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null);

        /// <summary>
        /// Gets a release with the given ID
        /// </summary>
        /// <param name="id">The ID of the release</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Model.Release GetRelease(int id, string[] limitFields = null);

        /// <summary>
        /// Gets a release with the given ID
        /// </summary>
        /// <param name="id">The ID of the release</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Task<Model.Release> GetReleaseAsync(int id, string[] limitFields = null);

        /// <summary>
        /// Gets all releases for a game with the given ID (multiple requests)
        /// </summary>
        /// <param name="gameId">The ID of the game to get releases for</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        IEnumerable<Model.Release> GetReleasesForGame(int gameId, string[] limitFields = null);

        /// <summary>
        /// Gets all releases for a game with the given ID (multiple requests)
        /// </summary>
        /// <param name="gameId">The ID of the game to get releases for</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Task<IEnumerable<Model.Release>> GetReleasesForGameAsync(int gameId, string[] limitFields = null);

        /// <summary>
        /// Gets all releases for the given game (multiple requests)
        /// </summary>
        /// <param name="game">The game to get releases for</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        IEnumerable<Model.Release> GetReleasesForGame(Model.Game game, string[] limitFields = null);

        /// <summary>
        /// Gets all releases for the given game (multiple requests)
        /// </summary>
        /// <param name="game">The game to get releases for</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Task<IEnumerable<Model.Release>> GetReleasesForGameAsync(Model.Game game, string[] limitFields = null);

        /// <summary>
        /// Searches for a game by keyword and gets paged results
        /// </summary>
        /// <param name="query">The search string</param>
        /// <param name="page">The page to retrieve</param>
        /// <param name="pageSize">The number of results per page (default: 100)</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        IEnumerable<Model.Game> SearchForGames(string query, int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null);

        /// <summary>
        /// Searches for a game by keyword and gets paged results
        /// </summary>
        /// <param name="query">The search string</param>
        /// <param name="page">The page to retrieve</param>
        /// <param name="pageSize">The number of results per page (default: 100)</param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Task<IEnumerable<Model.Game>> SearchForGamesAsync(string query, int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] limitFields = null);

        /// <summary>
        /// Searches for a game by keyword and recursively gets all results to enable sorting, filtering, etc.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        IEnumerable<Model.Game> SearchForAllGames(string query, string[] limitFields = null);

        /// <summary>
        /// Searches for a game by keyword and recursively gets all results to enable sorting, filtering, etc.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="limitFields">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns></returns>
        Task<IEnumerable<Model.Game>> SearchForAllGamesAsync(string query, string[] limitFields = null);

        /// <summary>
        /// Gets a list resource request
        /// </summary>
        /// <param name="resource">The resource name (e.g. "games")</param>
        /// <param name="page">The page to fetch (default: 1)</param>
        /// <param name="pageSize">The number of results per page (default: 100)</param>
        /// <param name="fieldList">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <param name="sortOptions">Results will be sorted by field names in the order you specify</param>
        /// <param name="filterOptions">Results will be filtered by the field name and value you specify</param>
        /// <returns></returns>
        RestRequest GetListResource(string resource, int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] fieldList = null, IDictionary<string, SortDirection> sortOptions = null, IDictionary<string, object> filterOptions = null);

        /// <summary>
        /// Gets a typed list from a GiantBomb list resource request
        /// </summary>
        /// <typeparam name="TResult">The type of result you expect the request to result in</typeparam>
        /// <param name="resource">The resource name (e.g. "games")</param>
        /// <param name="page">The page to fetch (default: 1)</param>
        /// <param name="pageSize">The number of results per page (default: 100)</param>
        /// <param name="fieldList">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <param name="sortOptions">Results will be sorted by field names in the order you specify</param>
        /// <param name="filterOptions">Results will be filtered by the field name and value you specify</param>
        /// <returns>A typed list of TResult</returns>
        IEnumerable<TResult> GetListResource<TResult>(string resource, int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] fieldList = null, IDictionary<string, SortDirection> sortOptions = null, IDictionary<string, object> filterOptions = null) where TResult : new();

        /// <summary>
        /// Gets a typed list from a GiantBomb list resource request
        /// </summary>
        /// <typeparam name="TResult">The type of result you expect the request to result in</typeparam>
        /// <param name="resource">The resource name (e.g. "games")</param>
        /// <param name="page">The page to fetch (default: 1)</param>
        /// <param name="pageSize">The number of results per page (default: 100)</param>
        /// <param name="fieldList">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <param name="sortOptions">Results will be sorted by field names in the order you specify</param>
        /// <param name="filterOptions">Results will be filtered by the field name and value you specify</param>
        /// <returns>A typed list of TResult</returns>
        Task<IEnumerable<TResult>> GetListResourceAsync<TResult>(string resource, int page = 1, int pageSize = Model.GiantBombBase.DefaultLimit, string[] fieldList = null, IDictionary<string, SortDirection> sortOptions = null, IDictionary<string, object> filterOptions = null) where TResult : new();

        /// <summary>
        /// Gets a single resource request
        /// </summary>
        /// <param name="resource">The resource name (e.g. "game")</param>
        /// <param name="resourceId">The resource type ID (e.g. 3030)</param>
        /// <param name="id">The ID of the resource</param>
        /// <param name="fieldList">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns>A REST request object</returns>
        RestRequest GetSingleResource(string resource, int resourceId, int id, string[] fieldList = null);

        /// <summary>
        /// Returns a single object from a resource
        /// </summary>
        /// <typeparam name="TResult">The model of what you expect</typeparam>
        /// <param name="resource">The resource name (e.g. "game")</param>
        /// <param name="resourceId">The resource type ID (e.g. 3030)</param>
        /// <param name="id">The ID of the resource</param>
        /// <param name="fieldList">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns>A REST request object</returns>
        TResult GetSingleResource<TResult>(string resource, int resourceId, int id, string[] fieldList = null) where TResult : class, new();

        /// <summary>
        /// Returns a single object from a resource
        /// </summary>
        /// <typeparam name="TResult">The model of what you expect</typeparam>
        /// <param name="resource">The resource name (e.g. "game")</param>
        /// <param name="resourceId">The resource type ID (e.g. 3030)</param>
        /// <param name="id">The ID of the resource</param>
        /// <param name="fieldList">List of field names to include in the response. Use this if you want to reduce the size of the response payload.</param>
        /// <returns>A REST request object</returns>
        Task<TResult> GetSingleResourceAsync<TResult>(string resource, int resourceId, int id, string[] fieldList = null) where TResult : class, new();

        /// <summary>
        /// Execute a manual REST request
        /// </summary>
        /// <typeparam name="T">The type of object to create and populate with the returned data.</typeparam>
        /// <param name="request">The RestRequest to execute (will use client credentials)</param>
        T Execute<T>(RestRequest request) where T : new();

        /// <summary>
        /// Execute a manual REST request
        /// </summary>
        /// <typeparam name="T">The type of object to create and populate with the returned data.</typeparam>
        /// <param name="request">The RestRequest to execute (will use client credentials)</param>
        Task<T> ExecuteAsync<T>(RestRequest request) where T : new();

        /// <summary>
        /// Execute a manual REST request
        /// </summary>
        /// <param name="request">The RestRequest to execute (will use client credentials)</param>
        IRestResponse Execute(RestRequest request);

        /// <summary>
        /// Execute a manual REST request
        /// </summary>
        /// <param name="request">The RestRequest to execute (will use client credentials)</param>
        Task<IRestResponse> ExecuteAsync(RestRequest request);
    }
}