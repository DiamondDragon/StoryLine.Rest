namespace StoryLine.Rest.Services.Http
{
    public interface IHttpClientFactory
    {
        IHttpClient Create(string service);
    }
}