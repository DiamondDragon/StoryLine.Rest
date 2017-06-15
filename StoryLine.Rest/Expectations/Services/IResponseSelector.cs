using StoryLine.Rest.Services.Http;

namespace StoryLine.Rest.Expectations.Services
{
    public interface IResponseSelector
    {
        bool Maches(IResponse response);
    }
}