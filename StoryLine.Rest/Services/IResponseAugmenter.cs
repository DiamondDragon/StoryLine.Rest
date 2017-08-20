using StoryLine.Rest.Services.Http;

namespace StoryLine.Rest.Services
{
    public interface IResponseAugmenter
    {
        void BeforeRequestExecuted(IRequest request);
        void AfterResponseReceived(IResponse response);
    }
}
