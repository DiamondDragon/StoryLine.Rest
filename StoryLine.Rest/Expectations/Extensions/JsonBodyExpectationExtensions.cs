using System;
using StoryLine.Rest.Expectations.Builders;

namespace StoryLine.Rest.Expectations.Extensions
{
    public static class JsonBodyExpectationExtensions
    {
        public static JsonBodyExpectationBuilder JsonBody(this HttpResponse builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            return new JsonBodyExpectationBuilder(builder);
        }
    }
}