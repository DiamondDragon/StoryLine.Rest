using System;

namespace StoryLine.Rest.Services.Http
{
    public class ServiceConfig : IServiceConfig
    {
        public string Service { get; }
        public bool AllowRedirect { get; }
        public string BaseAddress { get; }
        public TimeSpan Timeout { get; }

        public ServiceConfig(string service, string baseAddress, TimeSpan timeout, bool allowRedirect)
        {
            if (string.IsNullOrWhiteSpace(baseAddress))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(baseAddress));
            if (string.IsNullOrWhiteSpace(service))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(service));

            Service = service;
            BaseAddress = baseAddress;
            Timeout = timeout;
            AllowRedirect = allowRedirect;
        }
    }
}