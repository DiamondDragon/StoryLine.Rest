using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Services.Http
{
    public class RequestMessageFactoryTests
    {
        private readonly RequestMessageFactory _underTest;
        private readonly Request _request;

        public RequestMessageFactoryTests()
        {
            _request = new Request
            {
                Method = "GET",
                Url = "/users/123",
                Body = new byte[] { 123 }
            };

            _underTest = new RequestMessageFactory();
        }

        [Theory]
        [InlineData("head")]
        [InlineData("HEAD")]
        [InlineData("get")]
        [InlineData("GET")]
        public void Create_When_Head_Or_Get_Should_Not_Add_Content(string method)
        {
            _request.Method = method;

            _underTest.Create(_request).Content.Should().BeNull();
        }

        [Theory]
        [InlineData("post")]
        [InlineData("POST")]
        [InlineData("put")]
        [InlineData("PUT")]
        [InlineData("delete")]
        [InlineData("DELETE")]
        [InlineData("options")]
        [InlineData("OPTIONS")]
        public void Create_When_Not_Head_And_Not_Get_Should_Add_Content(string method)
        {
            _request.Method = method;

            _underTest.Create(_request).Content.ReadAsByteArrayAsync().Result.Should().BeEquivalentTo(new byte[] { 123 });
        }

        [Fact]
        public void Create_Should_Copy_Request_Values_To_Proper_Fields()
        {
            var result = _underTest.Create(_request);

            result.Method.Method.Should().Be(_request.Method.ToUpper());
            result.RequestUri.Should().Be(_request.Url);
        }

        [Theory]
        [InlineData("Allow", "value1")]
        [InlineData("Content-Disposition", "value1")]
        [InlineData("Content-Encoding", "value1")]
        [InlineData("Content-Language", "value1")]
        [InlineData("Content-Location", "value1")]
        [InlineData("Content-MD5", "Q2hlY2sgSW50ZWdyaXR5IQ==")]
        [InlineData("Content-Range", "bytes 21010-47021/47022")]
        [InlineData("Content-Type", "text/plain")]
        [InlineData("Expires", "Tue, 31 Jan 2012 15:02:53 GMT")]
        [InlineData("Last-Modified", "Tue, 31 Jan 2012 15:02:53 GMT")]
        public void Create_Should_Not_Add_Content_Headers_Into_Request_Message(string header, string value)
        {
            _request.Method = "POST";
            _request.Headers = new Dictionary<string, string[]>
            {
                [header] = new[] { value}
            };

            var result = _underTest.Create(_request);

            result.Headers.Any(x => x.Key.Equals(header, StringComparison.OrdinalIgnoreCase)).Should().BeFalse();
            result.Content.Headers.First(x => x.Key.Equals(header, StringComparison.OrdinalIgnoreCase)).Value.First().Should().Be(value);
        }

        [Theory]
        [InlineData("x-header1", "value1")]
        [InlineData("Content-xxx", "value1")]
        public void Create_Should_Not_Add_Non_Content_Headers_Into_Content_Headers(string header, string value)
        {
            _request.Method = "POST";
            _request.Headers = new Dictionary<string, string[]>
            {
                [header] = new[] { value }
            };

            var result = _underTest.Create(_request);

            result.Headers.First(x => x.Key.Equals(header, StringComparison.OrdinalIgnoreCase)).Value.First().Should().Be(value);
            result.Content.Headers.Any(x => x.Key.Equals(header, StringComparison.OrdinalIgnoreCase)).Should().BeFalse();
        }

        [Fact]
        public void Create_When_ContentLength_Specified_Value_Should_Not_Be_Copied()
        {
            _request.Method = "POST";
            _request.Headers = new Dictionary<string, string[]>
            {
                ["Content-Length"] = new[] { "1234" }
            };

            var result = _underTest.Create(_request);

            result.Headers.Any(x => x.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase)).Should().BeFalse();
            result.Content.Headers.ContentLength.Should().Be(1);
        }
    }
}
