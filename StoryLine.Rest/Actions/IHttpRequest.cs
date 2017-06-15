using StoryLine.Contracts;

namespace StoryLine.Rest.Actions
{
    public interface IHttpRequest : IActionBuilder
    {
        IHttpRequest Service(string value);
        IHttpRequest Method(string value);
        IHttpRequest Url(string value);
        IHttpRequest Path(string value);
        IHttpRequest QueryParameter(string parameter, string value);
        IHttpRequest Header(string header, string value);
        IHttpRequest Body(byte[] value);
    }
}