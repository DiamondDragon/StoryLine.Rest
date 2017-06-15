namespace StoryLine.Rest.Services.Http
{
    public interface IRestClient
    {
        IResponse Send(IRequest request);
    }
}