using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using StoryLine.Contracts;
using StoryLine.Exceptions;

namespace StoryLine.Rest.Actions
{
    public class HttpRequest : IHttpRequest
    {
        private readonly Dictionary<string, List<string>> _headers = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, List<string>> _queryParameters = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        private string _method = "GET";
        private string _url;
        private string _service;
        private byte[] _body;

        public IHttpRequest Service(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

            _service = value;

            return this;
        }

        public IHttpRequest Method(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

            _method = value;

            return this;
        }

        public IHttpRequest Url(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

            _url = value;

            return this;
        }

        public IHttpRequest QueryParam(string parameter, string value)
        {
            if (string.IsNullOrWhiteSpace(parameter))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(parameter));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

            AddDictionaryValue(_queryParameters, parameter, value);

            return this;
        }

        public IHttpRequest Header(string header, string value)
        {
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(header));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            AddDictionaryValue(_headers, header, value);

            return this;
        }

        public IHttpRequest Body(byte[] value)
        {
            _body = value ?? throw new ArgumentNullException(nameof(value));

            return this;
        }

        IAction IActionBuilder.Build()
        {
            return new HttpRequestAction
            {
                Service = GetService(),
                Body = _body,
                Headers = _headers.ToDictionary(x => x.Key, x => x.Value.ToArray(), StringComparer.OrdinalIgnoreCase),
                Method = _method,
                Url = BuildUrl()
            };
        }

        private string GetService()
        {
            if (!string.IsNullOrEmpty(_service))
            {
                var config = Config.ServiceRegistry.Get(_service);
                if (config == null)
                    throw new ExpectationException($"Configuration for service \"{_service}\" was not found.");

                return _service;
            }

            var allConfigs = Config.ServiceRegistry.GetAll();

            // If there only one service is registered it's clear that all services cases
            // are expected to use the endpoint by defalt. These lines eliminate a need
            // to specify the same service name for each HttpRequest.
            return allConfigs.Count == 1 ? allConfigs.First().Service : null;
        }

        private string BuildUrl()
        {
            if (_queryParameters.Count == 0)
                return _url;

            var queryParametersBuilder = new StringBuilder();

            foreach (var header in _queryParameters)
            {
                foreach (var value in header.Value)
                {
                    if (queryParametersBuilder.Length > 0)
                        queryParametersBuilder.Append('&');

                    queryParametersBuilder.Append(WebUtility.UrlEncode(header.Key));
                    queryParametersBuilder.Append('=');
                    queryParametersBuilder.Append(WebUtility.UrlEncode(value));
                }
            }

            if ((_url ?? string.Empty).Contains("?"))
                return _url + "&" + queryParametersBuilder;

            return _url + "?" + queryParametersBuilder;
        }

        private static void AddDictionaryValue(IDictionary<string, List<string>> headers, string header, string value)
        {
            if (!headers.ContainsKey(header))
                headers.Add(header, new List<string>());

            headers[header].Add(value);
        }
    }
}