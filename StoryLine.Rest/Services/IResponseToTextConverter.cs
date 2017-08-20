using StoryLine.Rest.Services.Http;

namespace StoryLine.Rest.Services
{
    public interface IResponseToTextConverter
    {
        string GetText(IResponse response, bool failOnEmptyBody = true);
    }
}