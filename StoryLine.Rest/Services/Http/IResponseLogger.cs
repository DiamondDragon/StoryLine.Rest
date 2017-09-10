namespace StoryLine.Rest.Services.Http
{
    public interface IResponseLogger
    {
        void Add(IResponse response);
    }
}