using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using StoryLine.Contracts;
using StoryLine.Rest.Expectations.Builders;
using StoryLine.Rest.Expectations.Services;
using StoryLine.Rest.Expectations.Services.Expectations;

namespace StoryLine.Rest.Expectations
{
    public class HttpResponse : IExpectationBuilder
    {
        private readonly List<IResponseExpectation> _expectations = new List<IResponseExpectation>();

        private Func<string, bool> _urlPredicate;
        private string _service;
        private string _method;

        IExpectation IExpectationBuilder.Build()
        {
            return new HttpResponseExpectation
            {
                Expectations = _expectations.ToArray(),
                Selector = CreateResonseSelector()
            };
        }

        private IResponseSelector CreateResonseSelector()
        {
            var selector = new ResponseSelector();

            if (_urlPredicate != null)
                selector.Url = _urlPredicate;

            if (!string.IsNullOrEmpty(_service))
                selector.Service = x => x.Equals(_service, StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrEmpty(_method))
                selector.Method = x => x.Equals(_method, StringComparison.OrdinalIgnoreCase);

            return selector;
        }

        public HttpResponse Url(Func<string, bool> value)
        {
            _urlPredicate = value ?? throw new ArgumentNullException(nameof(value));

            return this;
        }

        public HttpResponse Service(string value)
        {
            _service = value ?? throw new ArgumentException(nameof(value) + " can't be null or whitespace", nameof(value));

            return this;
        }

        public HttpResponse Method(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

            _method = value;

            return this;
        }

        public HttpResponse Status(HttpStatusCode status)
        {
            return Status((int) status);
        }

        public HttpResponse Status(int status)
        {
            if (status <= 0)
                throw new ArgumentOutOfRangeException(nameof(status));

            return RequestExpectation(new ResponseLamdaExpectation(
                x => x.Status == status,
                x => $"Expected status was {status}, but actual status is {x.Status}"
            ));
        }

        public HttpResponse ReasonPhrase(string phrase)
        {
            if (phrase == null)
                throw new ArgumentNullException(nameof(phrase));

            return RequestExpectation(new ResponseLamdaExpectation(
                x => x.ReasonPhrase.Equals(phrase, StringComparison.OrdinalIgnoreCase),
                x => $"Expected reason phrase was \"{phrase}\", but actual status is \"{x.ReasonPhrase}\""
            ));
        }

        public HttpResponse Header(string header, string value)
        {
            return Header(header, value, StringComparison.OrdinalIgnoreCase);
        }

        public HttpResponse Header(string header, string value, StringComparison comparison)
        {
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(header));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return RequestExpectation(new ResponseHeaderExpectation(
                header,
                x => x.Equals(value, comparison),
                x => $"Expected value for header \"{header}\" must be equal to \"{value}\", actual value was \"{x}\"."
            ));
        }

        public HttpResponse Header(string header, Func<string, bool> predicate)
        {
            return Header(header, predicate, $"Header \"{header}\" doesn't match predicate.");
        }

        public HttpResponse Header(string header, Func<string, bool> predicate, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));

            return Header(header, predicate, x => errorMessage);
        }

        public HttpResponse Header(string header, Func<string, bool> predicate, Func<string, string> errorMessageBuilder)
        {
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(header));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (errorMessageBuilder == null)
                throw new ArgumentNullException(nameof(errorMessageBuilder));

            return RequestExpectation(new ResponseHeaderExpectation(
                header,
                predicate,
                errorMessageBuilder
            ));
        }

        public HttpResponse Header(string header, Action<string> validator)
        {
            return Header(header, validator, $"Header \"{header}\" doesn't pass validation check.");
        }

        public HttpResponse Header(string header, Action<string> validator, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));

            return Header(header, validator, x => errorMessage);
        }

        public HttpResponse Header(string header, Action<string> validator, Func<string, string> errorMessageBuilder)
        {
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(header));
            if (validator == null)
                throw new ArgumentNullException(nameof(validator));
            if (errorMessageBuilder == null)
                throw new ArgumentNullException(nameof(errorMessageBuilder));

            return RequestExpectation(new ResponseHeaderExpectation(
                header,
                x => Validate(x, validator),
                errorMessageBuilder
            ));
        }

        private static bool Validate(string value, Action<string> validator)
        {
            try
            {
                validator(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public HttpResponse HeaderMatchesRegex(string header, string value)
        {
            return HeaderMatchesRegex(header, value, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public HttpResponse HeaderMatchesRegex(string header, string value, RegexOptions options)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return RequestExpectation(new ResponseHeaderExpectation(
                header,
                x => Regex.IsMatch(x, value, options),
                x => $"Expected value for header \"{header}\" must match regual expression \"{value}\", actual value was \"{x}\"."
            ));
        }

        public HttpResponse RequestExpectation(IResponseExpectation expectation)
        {
            if (expectation == null)
                throw new ArgumentNullException(nameof(expectation));

            _expectations.Add(expectation);

            return this;
        }
    }
}