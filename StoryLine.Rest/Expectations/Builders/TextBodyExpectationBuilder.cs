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
                x => $"Text body must be equal to \"{value}\", but actual value was \"{x}\"."
            ));

            return _builder;
        }

        public HttpResponse MatchesRegex(string value)
        {
            return MatchesRegex(value, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public HttpResponse MatchesRegex(string value, RegexOptions options)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _builder.RequestExpectation(new ResponseTextBodyExpectation(
                x => Regex.IsMatch(x, value, options),
                x => $"Text body must match regual expression \"{value}\", but actual value was \"{x}\"."
            ));

            return _builder;
        }

        public HttpResponse Matches(Action<string> validator)
        {
            return Matches(validator, "Text body doesn't pass validation check.");
        }

        public HttpResponse Matches(Action<string> validator, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));

            return Matches(validator, x => errorMessage);
        }

        public HttpResponse Matches(Action<string> validator, Func<string, string> errorMessageBuilder)
        {
            if (validator == null)
                throw new ArgumentNullException(nameof(validator));
            if (errorMessageBuilder == null)
                throw new ArgumentNullException(nameof(errorMessageBuilder));

            _builder.RequestExpectation(new ResponseTextBodyExpectation(
                x => { validator(x); return true; },
                errorMessageBuilder
            ));

            return _builder;
        }

        public HttpResponse Matches(Func<string, bool> predicate)
        {
            return Matches(predicate, "Text body doesn't match predicate.");
        }

        public HttpResponse Matches(Func<string, bool> predicate, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));

            return Matches(predicate, x => errorMessage);
        }

        public HttpResponse Matches(Func<string, bool> predicate, Func<string, string> errorMessageBuilder)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (errorMessageBuilder == null)
                throw new ArgumentNullException(nameof(errorMessageBuilder));

            _builder.RequestExpectation(new ResponseTextBodyExpectation(
               predicate,
                errorMessageBuilder
            ));

            return _builder;
        }

    }
}