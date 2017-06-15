using System;
using System.Text;

namespace StoryLine.Rest.Actions.Extensions
{
    public static class TextBodyExtensions
    {
        public static IHttpRequest TextBody(this IHttpRequest builder, string value)
        {
            return TextBody(builder, value, Config.DefaultEncoding);
        }

        public static IHttpRequest TextBody(this IHttpRequest builder, string value, Encoding encoding)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            builder.Body(encoding.GetBytes(value));

            return builder;
        }
    }
}