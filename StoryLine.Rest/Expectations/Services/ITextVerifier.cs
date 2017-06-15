namespace StoryLine.Rest.Expectations.Services
{
    public interface ITextVerifier
    {
        void Verify(string expectedValue, string actualValue);
    }
}