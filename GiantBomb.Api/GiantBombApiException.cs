using System;

namespace GiantBomb.Api
{
    public class GiantBombApiException : Exception
    {
        public int StatusCode { get; private set; }

        public GiantBombApiException(int statusCode, string error)
            : base("GiantBomb API Error: " + statusCode + ": " + error)
        {
            StatusCode = statusCode;
        }
    }
}