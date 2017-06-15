using System;
using StoryLine.Rest.Expectations.Services.Expectations;
using StoryLine.Rest.Expectations.Services.Text;
using StoryLine.Rest.Services.Resources;

namespace StoryLine.Rest.Expectations.Builders
{
    public class TextResourceBodyExpectationBuilder
    {
        private readonly HttpResponse _buider;
        private static IResourceContentProvider ContentProvider => Config.ResourceContentProvider;

        public TextResourceBodyExpectationBuilder(HttpResponse buider)
        {
            _buider = buider ?? throw new ArgumentNullException(nameof(buider));
        }

        public HttpResponse ResourceFile(string resourceName = null, bool exactMatch = false)
        {
            var content = ContentProvider.GetAsString(resourceName, exactMatch);

            if (string.IsNullOrEmpty(content))
                throw new InvalidOperationException(
                    string.IsNullOrEmpty(resourceName) ?
                        "Default resource was not found" :
                        $"Resource \"{resourceName}\" was not found");

            _buider.RequestExpectation(
                new BodyContentExpectation(
                    content,
                    Config.PlainTextVerifierFactory(new PlainTextVerifierSettings())));

            return _buider;
        }
    }
}