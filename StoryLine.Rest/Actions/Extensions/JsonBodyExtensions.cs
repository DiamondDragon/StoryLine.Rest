using System;
using System.Text;
using Newtonsoft.Json;

namespace StoryLine.Rest.Actions.Extensions
{
    public static class JsonBodyExtensions
    {
        public static IHttpRequest JsonObjectBody(this IHttpRequest builder, object body)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            return JsonObjectBody(builder, body, Config.DefaultJsonSerializerSettings);
        }

        private static IHttpRequest JsonObjectBody(IHttpRequest builder, object body, JsonSerializerSettings settings)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (body == null)
                throw new ArgumentNullException(nameof(body));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            return JsonObjectBody(builder, body, settings, Config.DefaultEncoding);
        }

        private static IHttpRequest JsonObjectBody(IHttpRequest builder, object body, JsonSerializerSettings settings, Encoding encoding)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (body == null)
                throw new ArgumentNullException(nameof(body));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            return JsonBody(builder, JsonConvert.SerializeObject(body, settings), encoding);
        }

        public static IHttpRequest JsonBody(this IHttpRequest builder, string json)
        {
            return JsonBody(builder, json, Config.DefaultEncoding);
        }

        public static IHttpRequest JsonBody(this IHttpRequest builder, string json, Encoding encoding)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            builder.Body(encoding.GetBytes(json));

            return builder;
        }
    }
}