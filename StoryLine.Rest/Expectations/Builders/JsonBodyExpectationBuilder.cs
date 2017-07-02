using System;
using Newtonsoft.Json;
using StoryLine.Rest.Expectations.Services.Expectations;
using StoryLine.Rest.Expectations.Services.Json;

namespace StoryLine.Rest.Expectations.Builders
{
    public class JsonBodyExpectationBuilder
    {
        private readonly HttpResponse _builder;

        public JsonBodyExpectationBuilder(HttpResponse builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public HttpResponse MatchesObject(object content, params string[] propertiesToIgnore)
        {
            return MatchesObject(content, Config.DefaultJsonSerializerSettings, propertiesToIgnore);
        }

        public HttpResponse MatchesObject(object content, JsonSerializerSettings settings, params string[] propertiesToIgnore)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            return Matches(JsonConvert.SerializeObject(content, settings), propertiesToIgnore);
        }

        public HttpResponse Matches(string expectedContent, params string[] propertiesToIgnore)
        {
            if (propertiesToIgnore == null)
                throw new ArgumentNullException(nameof(propertiesToIgnore));
            if (string.IsNullOrEmpty(expectedContent))
                throw new ArgumentException("Value cannot be null or empty.", nameof(expectedContent));

            var config = new JsonVerifierSettings
            {
                IgnoredProperties = propertiesToIgnore
            };

            _builder.RequestExpectation(
                new BodyContentExpectation(
                    expectedContent,
                    Config.JsonVerifierFactory(config)));

            return _builder;
        }

        public JsonResourceBodyExpectationBuilder Matches()
        {
            return new JsonResourceBodyExpectationBuilder(_builder);
        }
    }
}