namespace StoryLine.Rest.Actions.Extensions.Helpers
{
    public interface IFormDataBuilder
    {
        FormDataBuilder Param(string key, string value);
    }
}