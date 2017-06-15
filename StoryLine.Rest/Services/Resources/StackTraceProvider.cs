using System;
using System.Linq;

namespace StoryLine.Rest.Services.Resources
{
    /// <summary>
    /// This class is shim for missing .NET Core functionality
    /// similar to .NET Framework StackFrame functionality
    /// </summary>
    internal sealed class StackTraceProvider : IStackTraceProvider
    {
        public StackFrame[] GetStack()
        {
            var stackTrace = Environment.StackTrace;
            var methods =
                (from line in stackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                    let methodIndex = line.IndexOf("(", StringComparison.OrdinalIgnoreCase)
                    where methodIndex != -1
                    select line.Substring(0, methodIndex))
                .ToArray();

            methods =
                (from method in methods
                    where method.StartsWith("   at ", StringComparison.OrdinalIgnoreCase)
                    select method.Substring("   at ".Length))
                .ToArray();

            return
                (from item in methods
                    let dotIndex = item.LastIndexOf(".", StringComparison.OrdinalIgnoreCase)
                    select new StackFrame(item.Substring(0, dotIndex), item.Substring(dotIndex + 1)))
                .ToArray();
        }
    }
}