using System;
using System.Collections.Generic;

namespace StoryLine.Rest.Services.Http
{
    public interface IResponse
    {
        IRequest Request { get; }
        Exception Exception { get; }
        IReadOnlyDictionary<string, string[]> Headers { get; }
        byte[] Body { get; }
        int Status { get; }
        string ReasonPhrase { get; }

        IDictionary<string, object> Properties { get; }
    }
}