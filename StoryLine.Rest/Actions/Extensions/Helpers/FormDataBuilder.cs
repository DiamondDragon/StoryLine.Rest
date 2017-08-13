using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace StoryLine.Rest.Actions.Extensions.Helpers
{
    public class FormDataBuilder : IFormDataBuilder
    {
        private readonly List<KeyValuePair<string, string>> _keyValuePairs = new List<KeyValuePair<string, string>>();

        public FormDataBuilder Param(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Value cannot be null or empty.", nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _keyValuePairs.Add(new KeyValuePair<string, string>(key, value));

            return this;
        }

        public string BuildBody()
        {
            var builder = new StringBuilder();

            foreach (var pair in _keyValuePairs)
            {
                if (builder.Length > 0)
                    builder.Append("&");

                builder.Append(WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value));
            }

            return builder.ToString();
        }
    }
}