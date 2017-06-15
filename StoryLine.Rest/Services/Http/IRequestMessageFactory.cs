using System.Net.Http;

namespace StoryLine.Rest.Services.Http
{
    public interface IRequestMessageFactory
    {
        HttpRequestMessage Create(IRequest request);
    }
}