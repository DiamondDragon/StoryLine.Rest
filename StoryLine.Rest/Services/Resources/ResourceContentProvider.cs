using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StoryLine.Rest.Services.Resources
{
    internal class ResourceContentProvider : IResourceContentProvider
    {
        private readonly IMethodDetailsFilter _methodDetailsFilter;
        private readonly IStackTraceProvider _stackTraceProvider;

        private static readonly byte[] EmptyArray = new byte[0];

        private static readonly string[] Suffixes =
        {
            "",
            ".approved."
        };

        private readonly IAssemblyProvider _resourceAssemblyProvider;

        public ResourceContentProvider(
            IAssemblyProvider resourceAssemblyProvider,
            IStackTraceProvider stackTraceProvider,
            IMethodDetailsFilter methodDetailsFilter)
        {
            _methodDetailsFilter = methodDetailsFilter ?? throw new ArgumentNullException(nameof(methodDetailsFilter));
            _resourceAssemblyProvider = resourceAssemblyProvider ?? throw new ArgumentNullException(nameof(resourceAssemblyProvider));
            _stackTraceProvider = stackTraceProvider ?? throw new ArgumentNullException(nameof(stackTraceProvider));
        }

        public byte[] GetAsBytes(string resourceName = null, bool exactMatch = false)
        {
            var stream = GetAsStream(resourceName, exactMatch);

            if (stream == null)
                return EmptyArray;

            if (stream == Stream.Null)
                return EmptyArray;

            var buffer = new byte[16 * 1024];

            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public string GetAsString(string resourceName = null, bool exactMatch = false)
        {
            var stream = GetAsStream(resourceName, exactMatch);

            if (stream == null)
                return string.Empty;

            if (stream == Stream.Null)
                return string.Empty;

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public Stream GetAsStream(string resourceName = null, bool exactMatch = false)
        {
            var frame =  _stackTraceProvider.GetStack().FirstOrDefault(_methodDetailsFilter.IsTestMethod);
            if (frame == null)
                return Stream.Null;

            resourceName = resourceName ?? frame.MethodName;

            foreach (var assembly in _resourceAssemblyProvider.GetAssemblies())
            {
                var resources = assembly.GetManifestResourceNames();

                return exactMatch ? 
                    GetExactMatchResource(assembly, resourceName, resources) : 
                    GetSimilarResource(frame, assembly, resourceName, resources);
            }

            return Stream.Null;
        }

        private static Stream GetSimilarResource(StackFrame frame, Assembly assembly, string resourceName, string[] resources)
        {
            foreach (var suffix in Suffixes)
            {
                var possibleResourceName = frame.TypeName + "." + resourceName + suffix;

                var matchingResource = resources.FirstOrDefault(x => x.StartsWith(possibleResourceName, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(matchingResource))
                    return assembly.GetManifestResourceStream(matchingResource);
            }

            return Stream.Null;
        }

        private static Stream GetExactMatchResource(Assembly assembly, string resourceName, IEnumerable<string> resources)
        {
            return resources.Contains(resourceName) ? 
                assembly.GetManifestResourceStream(resourceName) : 
                Stream.Null;
        }
    }
}