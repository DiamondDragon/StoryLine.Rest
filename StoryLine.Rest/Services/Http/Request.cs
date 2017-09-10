using System;
using System.Collections.Generic;

namespace StoryLine.Rest.Services.Http
{
    public class Request : IRequest
    {
        private IDictionary<string, object> _properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private IReadOnlyDictionary<string, string[]> _headers = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        public string Service { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }

        public IReadOnlyDictionary<string, string[]> Headers
        {
            get => _headers;
            set => _headers = value ?? throw new ArgumentNullException(nameof(value));
        }

        public byte[] Body { get; set; }

        public IDictionary<string, object> Properties
        {
            get => _properties;
            set => _properties = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}