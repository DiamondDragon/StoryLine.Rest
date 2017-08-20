using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Services.Http
{
    public class ResponseFactoryTests
    {
        private readonly ResponseFactory _underTest;

        public ResponseFactoryTests()
        {
            _underTest = new ResponseFactory();
        }

        [Fact]
        public void CreateExceptionResponse_When_Null_Request_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.CreateExceptionResponse(null, new Exception()));
        }

        [Fact]
        public void CreateExceptionResponse_When_Null_Exception_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.CreateExceptionResponse(new Request(), null));
        }

        [Fact]
        public void CreateExceptionResponse_Should_Create_Expected_Result()
        {
            var request = new Request { Method = "POST", Body = new byte[1] };
            var exception = new Exception("Test");

            _underTest.CreateExceptionResponse(request, exception).ShouldBeEquivalentTo(new Response
            {
                Exception =  exception,
                Request = request
            });
        }

        [Fact]
        public void Create_When_Null_Request_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.Create(null, new HttpResponseMessage()));
        }

        [Fact]
        public void Create_When_Null_Exception_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.Create(new Request(), null));
        }

        // Exception thrown by ReadAsByArrayAsync() can't be tested because this method
        // is not virtual or abstract and can't be overriden with mock

        [Fact]
        public void Create_Should_Copy_Values_Response()
        {
            var request = new Request { Method = "POST", Body = new byte[1] };

            var message = new HttpResponseMessage
            {
                ReasonPhrase = "Reason1",
                StatusCode = (HttpStatusCode)213,
                Content = new ByteArrayContent(new byte[] { 123 })
            };

            message.Headers.Add("custom-message-header", new[] { "value1"});
            message.Content.Headers.Add("custom-content-header", new[] { "value2"});

            _underTest.Create(request, message).ShouldBeEquivalentTo(new Response
            {
                Request = request,
                ReasonPhrase = "Reason1",
                Status = 213,
                Body = new byte[] { 123 },
                Headers = new Dictionary<string, string[]>
                {
                    ["custom-message-header"] = new[] { "value1" },
                    ["custom-content-header"] = new[] { "value2" }
                }
            });
        }
    }
}
