namespace StoryLine.Rest.Services
{
    public interface IStringContentComparer
    {
        void Verify(string expected, string actual);
    }
}