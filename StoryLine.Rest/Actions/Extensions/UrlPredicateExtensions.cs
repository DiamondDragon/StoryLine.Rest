using System;
using StoryLine.Rest.Expectations;
using StoryLine.Rest.Expectations.Builders;

namespace StoryLine.Rest.Actions.Extensions
{
    public static class UrlPredicateExtensions
    {
        public static UrlPredicateBuilder Url(this HttpResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            return new UrlPredicateBuilder(response);
        }
    }
}