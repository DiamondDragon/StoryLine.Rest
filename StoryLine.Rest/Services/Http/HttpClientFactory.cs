using System;
using System.Net.Http;
using StoryLine.Exceptions;

namespace StoryLine.Rest.Services.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IServiceRegistry _serviceRegistry;

        public HttpClientFactory(IServiceRegistry serviceRegistry)
        {
            _serviceRegistry = serviceRegistry ?? throw new ArgumentNullException(nameof(serviceRegistry));
        }

        public IHttpClient Create(string service)
        {
            var config = GetConfig(service);

            if (config == null)
                return new HttpClientWrapper();

            var httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };

            return new HttpClientWrapper(httpClientHandler)
            {
                Timeout = config.Timeout,
                BaseAddress = new Uri(config.BaseAddress.TrimEnd('/'), UriKind.Absolute)
            };
        }

        private IServiceConfig GetConfig(string service)
        {
            if (string.IsNullOrEmpty(service))
                return null;

            var config = _serviceRegistry.Get(service);
            if (config == null)
                throw new ExpectationException($"Configuration for service \"{service}\" was not found.");

            return config;
        }
    }
}