using System.IO;

namespace StoryLine.Rest.Services.Resources
{
    public interface IResourceContentProvider
    {
        byte[] GetAsBytes(string resourceName = null, bool exactMatch = false);
        string GetAsString(string resourceName = null, bool exactMatch = false);
        Stream GetAsStream(string resourceName = null, bool exactMatch = false);
    }
}