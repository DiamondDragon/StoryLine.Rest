using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using StoryLine.Rest.Loggers;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Loggers
{
    public class TestJsonResponseLogger
    {
        private readonly JsonResponseLogger _underTest;
        private readonly MemoryStream _output;

        public TestJsonResponseLogger()
        {
            _output = new MemoryStream();

            _underTest = new JsonResponseLogger(_output);
        }

        [Fact]
        public void Add_Should_Save_Response_As_Expected_Json()
        {
            _underTest.Add(CreateResponse());
            _underTest.Add(CreateResponse());

            GetOutputText().Should().Be("[{\"request\":{\"service\":\"Srv1\",\"method\":\"POST\",\"url\":\"/v1/test/srv\",\"headers\":{\"header1\":[\"value1\"],\"header2\":[\"value2\"]},\"body\":\"UmVxdWVzdEJvZHk=\",\"properties\":{}},\"exception\":null,\"headers\":{\"header3\":[\"value3\"],\"header4\":[\"value4\"]},\"body\":\"UmVzcG9uc2VCb2R5\",\"status\":200,\"reasonPhrase\":\"Phrase1\",\"properties\":{\"key1\":\"prop1\"}},{\"request\":{\"service\":\"Srv1\",\"method\":\"POST\",\"url\":\"/v1/test/srv\",\"headers\":{\"header1\":[\"value1\"],\"header2\":[\"value2\"]},\"body\":\"UmVxdWVzdEJvZHk=\",\"properties\":{}},\"exception\":null,\"headers\":{\"header3\":[\"value3\"],\"header4\":[\"value4\"]},\"body\":\"UmVzcG9uc2VCb2R5\",\"status\":200,\"reasonPhrase\":\"Phrase1\",\"properties\":{\"key1\":\"prop1\"}}]");
        }

        private static Response CreateResponse()
        {
            return new Response
            {
                Request = new Request
                {
                    Body = Encoding.UTF8.GetBytes("RequestBody"),
                    Headers = new Dictionary<string, string[]>
                    {
                        ["header1"] = new[] { "value1" },
                        ["header2"] = new[] { "value2" }
                    },
                    Method = "POST",
                    Service = "Srv1",
                    Url = "/v1/test/srv"
                },
                Headers = new Dictionary<string, string[]>
                {
                    ["header3"] = new[] { "value3" },
                    ["header4"] = new[] { "value4" }
                },
                Body = Encoding.UTF8.GetBytes("ResponseBody"),
                ReasonPhrase = "Phrase1",
                Status = 200,
                Properties = new Dictionary<string, object>
                {
                    ["key1"] = "prop1"
                }
            };
        }

        private string GetOutputText()
        {
            _underTest.Dispose();

            return Encoding.UTF8.GetString(_output.ToArray());
        }
    }
}
