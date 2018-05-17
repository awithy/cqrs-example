using System;

namespace Api.Common
{
    public interface IApiConfig
    {
        Uri BaseUri { get; }
    }

    public class Configuration : IApiConfig
    {
        public Uri BaseUri => new Uri("https://localhost:5000");
    }
}