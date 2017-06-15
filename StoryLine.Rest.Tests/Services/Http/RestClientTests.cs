using System;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Services.Http
{
    public class RestClientTests
    {
        private readonly RestClient _underTest;

        private readonly IHttpClient _httpClient;
        private readonly Request _request;
        private readonly Response _response;
        private readonly Exception _exception;
        private readonly Response _errorResponse;
        private readonly HttpRequestMessage _requestMessage;

        public RestClientTests()
        {
            var httpClientFactory = A.Fake<IHttpClientFactory>();
            var requestMessageFactory = A.Fake<IRequestMessageFactory>();
            var responseFactory = A.Fake<IResponseFactory>();
            _httpClient = A.Fake<IHttpClient>();
            _exception = new Exception();

            _request = new Request
            {
                Service = "Service1"
            };
            _response = new Response();
            _errorResponse = new Response();
            _requestMessage = new HttpRequestMessage();
            var responseMessage = new HttpResponseMessage();

            A.CallTo(() => httpClientFactory.Create(_request.Service)).Returns(_httpClient);
            A.CallTo(() => requestMessageFactory.Create(_request)).Returns(_requestMessage);
            A.CallTo(() => responseFactory.Create(_request, responseMessage)).Returns(_response);
            A.CallTo(() => _httpClient.SendAsync(_requestMessage)).Returns(Task.FromResult(responseMessage));
            A.CallTo(() => responseFactory.CreateExceptionResponse(_request, _exception)).Returns(_errorResponse);


            _underTest = new RestClient(
                httpClientFactory,
                requestMessageFactory,
                responseFactory
            );
        }

        [Fact]
        public void Send_When_Null_Request_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.Send(null));
        }

        [Fact]
        public void Send_When_No_Exception_Thrown_Should_Convert_Request_Into_Message_And_Convert_Response_Message_Into_Response()
        {
            _underTest.Send(_request).Should().BeSameAs(_response);

            A.CallTo(() => _httpClient.Dispose()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Send_When_Exception_Thrown_Should_Return_Error_Response()
        {
            A.CallTo(() => _httpClient.SendAsync(_requestMessage)).Throws(_exception);

            _underTest.Send(_request).Should().BeSameAs(_errorResponse);

            A.CallTo(() => _httpClient.Dispose()).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
