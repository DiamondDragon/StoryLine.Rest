using System.Collections.Generic;
using System.Reflection;

namespace StoryLine.Rest.Services.Resources
{
    public interface IAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}