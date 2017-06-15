using System;
using System.Text.RegularExpressions;
using StoryLine.Rest.Expectations.Services.Expectations;

namespace StoryLine.Rest.Expectations.Builders
{
    public class HeaderExpectationBuilder
    {
        private readonly string _header;
        private readonly HttpResponse _builder;

        public HeaderExpectationBuilder(string header, HttpResponse builder)
        {
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(header));

            _header = header;
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public HttpResponse EqualsTo(string value)
        {
            return EqualsTo(value, StringComparison.OrdinalIgnoreCase);
        }

        public HttpResponse EqualsTo(string value, StringComparison comparison)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _builder.RequestExpectation(new ResponseHeaderExpectation(
                _header,
                x => x.Equals(value, comparison),
                x => $"Expected value must be equal to \"{value}\", actual values were \"{x}\"."
            ));

            return _builder;
        }

        public HttpResponse Matches(string value, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _builder.RequestExpectation(new ResponseHeaderExpectation(
                _header,
                x => Regex.IsMatch(x, value, options),
                x => $"Expected value must match regual expression \"{value}\", actual values were \"{x}\"."
            ));

            return _builder;
        }

        public HttpResponse StartsWith(string value)
        {
            return StartsWith(value, StringComparison.OrdinalIgnoreCase);
        }

        public HttpResponse StartsWith(string value, StringComparison comparison)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var lowerCasePattern = value.ToLower();

            _builder.RequestExpectation(new ResponseHeaderExpectation(
                _header,
                x => x.StartsWith(lowerCasePattern, comparison),
                x => $"Expected value must start \"{value}\", actual values were \"{x}\"."
            ));

            return _builder;
        }

        public HttpResponse Contains(string value, bool ignoreCase = true)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var lowerCasePattern = ignoreCase ? value.ToLower() : value;

            _builder.RequestExpectation(new ResponseHeaderExpectation(
                _header,
                x => (ignoreCase ? x.ToLower() : x).Contains(lowerCasePattern),
                x => $"Expected value must contain \"{value}\", actual values were \"{x}\"."
            ));

            return _builder;
        }
    }
}