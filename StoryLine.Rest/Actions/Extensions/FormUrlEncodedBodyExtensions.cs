using System;
using StoryLine.Rest.Actions.Extensions.Helpers;

namespace StoryLine.Rest.Actions.Extensions
{
    public static class FormUrlEncodedBodyExtensions
    {
        public static IHttpRequest FormUrlEncodedBody(this IHttpRequest builder, Action<IFormDataBuilder> configurator) 
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));

            var formDataBuilder = new FormDataBuilder();

            configurator(formDataBuilder);

            return builder.TextBody(formDataBuilder.BuildBody());
        }
    }
}
