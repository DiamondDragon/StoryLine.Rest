using System;

namespace StoryLine.Rest.Services.Http
{
    public interface IServiceConfig
    {
        string Service { get; }
        bool AllowRedirect { get; }
        string BaseAddress { get; }
        TimeSpan Timeout { get; }
    }
}