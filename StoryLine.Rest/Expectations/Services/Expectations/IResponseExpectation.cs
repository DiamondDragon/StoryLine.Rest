using StoryLine.Rest.Services.Http;

namespace StoryLine.Rest.Expectations.Services.Expectations
{
    public interface IResponseExpectation
    {
        void Validate(IResponse response);
    }
}