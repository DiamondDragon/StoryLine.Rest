namespace StoryLine.Rest.Expectations.Services.Json
{
    public interface IJsonFormatter
    {
        string Format(string content, params string[] propertiesToIgnore);
    }
}