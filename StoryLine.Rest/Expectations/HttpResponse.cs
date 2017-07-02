using System;
using System.Collections.Generic;
using System.Net;
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
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(header));

            return Header(header).EqualsTo(value);
        }

        public HeaderExpectationBuilder Header(string header)
        {
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(header));

            return new HeaderExpectationBuilder(header, this);
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