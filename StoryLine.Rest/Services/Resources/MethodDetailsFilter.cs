using System;
using System.Linq;
using System.Reflection;

namespace StoryLine.Rest.Services.Resources
{
    internal class MethodDetailsFilter : IMethodDetailsFilter
    {
        private static readonly string[] AttributeNames =
        {
            "TestAttribute",
            "TestCaseAttribute",
            "FactAttribute",
            "TheoryAttribute"
        };

        private readonly IAssemblyProvider _assemblyProvider;

        public MethodDetailsFilter(IAssemblyProvider assemblyProvider)
        {
            _assemblyProvider = assemblyProvider ?? throw new ArgumentNullException(nameof(assemblyProvider));
        }

        public bool IsTestMethod(StackFrame details)
        {
            foreach (var assembly in _assemblyProvider.GetAssemblies())
            {
                var type = assembly.GetType(details.TypeName);

                var method = type?.GetTypeInfo().GetDeclaredMethod(details.MethodName);
                if (method == null)
                    continue;

                var attributes = method.GetCustomAttributes();

                return attributes.Any(x => AttributeNames.Any(p => p == x.GetType().Name));
            }

            return false;
        }
    }
}