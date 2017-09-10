using System;
using StoryLine.Rest.Services.Http;

namespace StoryLine.Rest.Expectations.Services
{
    public sealed class ResponseSelector : IResponseSelector
    {
        public Func<string, bool> Service { get; set; } = x => true;
        public Func<string, bool> Url { get; set; } = x => true;
        public Func<string, bool> Method { get; set; } = x => true;

        public bool Maches(IResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (!Service(response.Request.Service))
                return false;

            return Url(response.Request.Url) && Method(response.Request.Method);
        }
    }
}