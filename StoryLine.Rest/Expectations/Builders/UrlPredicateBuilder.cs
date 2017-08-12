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

        public HttpResponse EqualsTo(string pattern)
        {
            return EqualsTo(pattern, StringComparison.OrdinalIgnoreCase);
        }

        public HttpResponse EqualsTo(string pattern, StringComparison comparison)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            _response.Url(x => x.Equals(pattern, comparison));

            return _response;
        }

        public HttpResponse MatchesRegex(string pattern)
        {
            return MatchesRegex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public HttpResponse MatchesRegex(string pattern, RegexOptions options)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            _response.Url(x => Regex.IsMatch(x, pattern, options));

            return _response;
        }

        public HttpResponse Matches(Func<string, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            _response.Url(predicate);

            return _response;
        }
    }
}