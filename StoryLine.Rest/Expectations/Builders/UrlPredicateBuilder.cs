using System;
using System.Text.RegularExpressions;

namespace StoryLine.Rest.Expectations.Builders
{
    public class UrlPredicateBuilder
    {
        private readonly HttpResponse _response;

        public UrlPredicateBuilder(HttpResponse response)
        {
            _response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public HttpResponse Matches(string pattern, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            _response.Url(x => Regex.IsMatch(x, pattern, options));

            return _response;
        }

        public HttpResponse EqualsTo(string pattern)
        {
            pattern = pattern?.ToLower() ?? throw new ArgumentNullException(nameof(pattern));

            _response.Url(x => x.Equals(pattern, StringComparison.OrdinalIgnoreCase));

            return _response;
        }

        public HttpResponse StartingWith(string pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            _response.Url(x => x.StartsWith(pattern, StringComparison.OrdinalIgnoreCase));

            return _response;
        }

        public HttpResponse Contains(string pattern)
        {
            pattern = pattern?.ToLower() ?? throw new ArgumentNullException(nameof(pattern));

            _response.Url(x => x.ToLower().Contains(pattern));

            return _response;
        }
    }
}