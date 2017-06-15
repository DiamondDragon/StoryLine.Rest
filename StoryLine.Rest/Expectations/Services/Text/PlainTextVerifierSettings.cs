using System;
using StoryLine.Rest.Services;

namespace StoryLine.Rest.Expectations.Services.Text
{
    public class PlainTextVerifierSettings
    {
        private IStringContentComparer _contentComparer = new StringContentComparer();

        public IStringContentComparer ContentComparer
        {
            get => _contentComparer;
            set => _contentComparer = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}