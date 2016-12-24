using System.Collections.Generic;

namespace GiantBombApi.Model {
    public class GiantBombBase {
        public string Error { get; set; }
        public int Limit { get; set; }
        public int NumberOfPageResults { get; set; }
        public int NumberOfTotalResults { get; set; }
        public int StatusCode { get; set; }
        public string Version { get; set; }

        public const int StatusOk = 1;
        public const int StatusApiKeyInvalid = 100;
        public const int StatusObjectNotFound = 101;
        public const int StatusErrorUrlFormat = 102;
        public const int StatusFilterError = 104;
        public const int StatusRateLimitExceeded = 107;
        public const int DefaultLimit = 100;
    }

    public class GiantBombResult<TResult> : GiantBombBase
    {
        public TResult Results { get; set; }
    }

    public class GiantBombResults<TResult> : GiantBombBase {
        public List<TResult> Results { get; set; }
    }
}
