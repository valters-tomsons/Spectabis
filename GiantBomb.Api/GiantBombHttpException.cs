using System;

namespace GiantBomb.Api
{
    public class GiantBombHttpException : Exception
    {
        public GiantBombHttpException(string message, string response = null)
            : base("GiantBomb HTTP Exception, bad request from API: " + message + ", Response: " + response)
        {

        }

        public GiantBombHttpException(string message, Exception innerEx, string response = null)
            : base("GiantBomb HTTP Exception, bad request from API: " + message + ", Response: " + response, innerEx)
        {

        }
    }
}