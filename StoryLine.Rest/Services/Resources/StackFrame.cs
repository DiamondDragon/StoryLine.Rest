using System;

namespace StoryLine.Rest.Services.Resources
{
    internal sealed class StackFrame
    {
        public string TypeName { get; }
        public string MethodName { get; }

        public StackFrame(string typeName, string methodName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(typeName));
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(methodName));

            TypeName = typeName;
            MethodName = methodName;
        }
    }
}