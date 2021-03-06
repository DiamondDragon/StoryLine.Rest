using System;
using StoryLine.Rest.Expectations.Services.Expectations;
using StoryLine.Rest.Expectations.Services.Json;
using StoryLine.Rest.Services.Resources;

namespace StoryLine.Rest.Expectations.Builders
{
    public class JsonResourceBodyExpectationBuilder
    {
        private readonly HttpResponse _buider;
        private static IResourceContentProvider ContentProvider => Config.ResourceContentProvider;

        public JsonResourceBodyExpectationBuilder(HttpResponse buider)
        {
            _buider = buider ?? throw new ArgumentNullException(nameof(buider));
        }

        public HttpResponse ResourceFile()
        {
            var content = ContentProvider.GetAsString();
            if (string.IsNullOrEmpty(content))
                throw new InvalidOperationException("Default resource was not found");

            _buider.RequestExpectation(
                new BodyContentExpectation(
                    content,
                    Config.JsonVerifierFactory(new JsonVerifierSettings())));

            return _buider;
        }

        public HttpResponse ResourceFile(string[] propertiesToIgnore)
        {
            if (propertiesToIgnore == null)
                throw new ArgumentNullException(nameof(propertiesToIgnore));

            var content = ContentProvider.GetAsString();
            if (string.IsNullOrEmpty(content))
                throw new InvalidOperationException("Default resource was not found");

            var config = new JsonVerifierSettings
            {
                IgnoredProperties = propertiesToIgnore
            };

            _buider.RequestExpectation(
                new BodyContentExpectation(
                    content, 
                    Config.JsonVerifierFactory(config)));

            return _buider;
        }

        public HttpResponse ResourceFile(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentNullException(nameof(resourceName));

            var content = ContentProvider.GetAsString(resourceName);
            if (string.IsNullOrEmpty(content))
                throw new InvalidOperationException($"Resource \"{resourceName}\" was not found");

            _buider.RequestExpectation(
                new BodyContentExpectation(
                    content,
                    Config.JsonVerifierFactory(new JsonVerifierSettings())));

            return _buider;
        }

        public HttpResponse ResourceFile(string resourceName, string[] propertiesToIgnore)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentNullException(nameof(resourceName));
            if (propertiesToIgnore == null)
                throw new ArgumentNullException(nameof(propertiesToIgnore));

            var content = ContentProvider.GetAsString(resourceName);
            if (string.IsNullOrEmpty(content))
                throw new InvalidOperationException($"Resource \"{resourceName}\" was not found");

            var config = new JsonVerifierSettings
            {
                IgnoredProperties = propertiesToIgnore
            };

            _buider.RequestExpectation(
                new BodyContentExpectation(
                    content,
                    Config.JsonVerifierFactory(config)));

            return _buider;
        }
    }
}