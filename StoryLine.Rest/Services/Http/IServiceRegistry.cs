using System.Collections.Generic;

namespace StoryLine.Rest.Services.Http
{
    public interface IServiceRegistry
    {
        IServiceConfig Get(string serviceName);
        IReadOnlyCollection<IServiceConfig> GetAll();
    }
}