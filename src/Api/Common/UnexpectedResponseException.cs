using System;
using System.Net;

namespace Api.Common
{
    public class UnexpectedResponseException : Exception
    {
        public UnexpectedResponseException(HttpStatusCode statusCode)
            : base($"Unexpected response {statusCode}.")
        {
        }
    }
}