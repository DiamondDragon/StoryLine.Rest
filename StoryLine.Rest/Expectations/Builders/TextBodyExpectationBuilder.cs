using System;
using System.Text.RegularExpressions;
using StoryLine.Rest.Expectations.Services.Expectations;

namespace StoryLine.Rest.Expectations.Builders
{
    public class TextBodyExpectationBuilder
    {
        private readonly HttpResponse _builder;

        public TextBodyExpectationBuilder(HttpResponse builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public TextResourceBodyExpectationBuilder EqualsTo()
        {
            return new TextResourceBodyExpectationBuilder(_builder);
        }

        public HttpResponse EqualsTo(string value)
        {
            return EqualsTo(value, StringComparison.OrdinalIgnoreCase);
        }

        public HttpResponse EqualsTo(string value, StringComparison comparison)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _builder.RequestExpectation(new ResponseTextBodyExpectation(
                x => x.Equals(value, comparison),
                x => $"Expected value must be equal to \"{value}\", but actual value was \"{x}\"."
            ));

            return _builder;
        }

        public HttpResponse Matches(string value, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _builder.RequestExpectation(new ResponseTextBodyExpectation(
                x => Regex.IsMatch(x, value, options),
                x => $"Expected value must match regual expression \"{value}\", but actual value was \"{x}\"."
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

            _builder.RequestExpectation(new ResponseTextBodyExpectation(
                x => x.StartsWith(lowerCasePattern, comparison),
                x => $"Expected value must start \"{value}\", but actual value was \"{x}\"."
            ));

            return _builder;
        }

        public HttpResponse Contains(string value, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            var lowerCasePattern = ignoreCase ? value.ToLower() : value;

            _builder.RequestExpectation(new ResponseTextBodyExpectation(
                x => (ignoreCase ? x.ToLower() : x).Contains(lowerCasePattern),
                x => $"Expected value must contain \"{value}\", but actual value was \"{x}\"."
            ));

            return _builder;
        }

        public HttpResponse Meets(Action<string> validator)
        {
            if (validator == null)
                throw new ArgumentNullException(nameof(validator));

            _builder.RequestExpectation(new ResponseTextBodyExpectation(
                x => { validator(x); return true; },
                x => $"Value \"{x}\", doesn't meet expectation."
            ));

            return _builder;
        }

        public HttpResponse Meets(Func<string, bool> validator)
        {
            if (validator == null)
                throw new ArgumentNullException(nameof(validator));

            _builder.RequestExpectation(new ResponseTextBodyExpectation(
                validator,
                x => $"Value \"{x}\", doesn't meet expectation."
            ));

            return _builder;
        }
    }
}