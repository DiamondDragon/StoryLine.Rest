using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryLine.Rest.Services.Http
{
    public class ServiceRegistry : IServiceRegistry
    {
        private readonly List<IServiceConfig> _configs = new List<IServiceConfig>();

        public void Add(IServiceConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (_configs.Any(x => x.Service.Equals(config.Service, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Service \"{config.Service}\" is already registered");

            _configs.Add(config);
        }

        public IReadOnlyCollection<IServiceConfig> GetAll()
        {
            return _configs;
        }

        public IServiceConfig Get(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(serviceName));

            return _configs.FirstOrDefault(x => x.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
        }
    }
}