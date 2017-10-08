using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StoryLine.Rest.Services.Http;

namespace StoryLine.Rest.Loggers
{
    public sealed class JsonResponseLogger : IResponseLogger, IDisposable
    {
        public static readonly JsonSerializerSettings DefaultSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Include
        };

        private readonly JsonTextWriter _jsonWriter;
        private readonly JsonSerializer _jsonSerializer;

        public JsonResponseLogger(string filePath)
            : this(filePath, DefaultSerializerSettings)
        {
        }

        public JsonResponseLogger(string filePath, JsonSerializerSettings settings)
            : this(File.OpenWrite(filePath), settings)
        {
        }

        public JsonResponseLogger(Stream outputStream)
            : this(outputStream, DefaultSerializerSettings)
        {
        }

        public JsonResponseLogger(Stream outputStream, JsonSerializerSettings settings)
        {
            if (outputStream == null)
                throw new ArgumentNullException(nameof(outputStream));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _jsonWriter = new JsonTextWriter(new StreamWriter(outputStream));
            _jsonWriter.WriteStartArray();

            _jsonSerializer = JsonSerializer.Create(settings);
        }

        public void Add(IResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            _jsonSerializer.Serialize(_jsonWriter, response);
        }

        public void Dispose()
        {
            _jsonWriter.WriteEndArray();
            _jsonWriter.Flush();
            _jsonWriter.Close();
        }
    }
}
