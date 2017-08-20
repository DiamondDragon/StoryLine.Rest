using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using StoryLine.Exceptions;
using StoryLine.Rest.Extensions;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Extensions
{
    public class ResponseExtensionsTests
    {
        private const string BodyText = "Dragon12345";

        private readonly Response _response;

        public ResponseExtensionsTests()
        {
            _response = new Response
            {
                Status = 200,
                Body = Encoding.UTF8.GetBytes(BodyText),
                Request = new Request
                {
                    Service = "Crm",
                    Method = "GET",
                    Url = "/v1/test"
                }
            };
        }

        [Fact]
        public void GetText_When_Request_Validation_Enabled_And_Empty_Text_Should_Throw_Expectation_Exception()
        {
            _response.Body = new byte[0];

            Assert.Throws<ExpectationException>(() => _response.GetText());
        }

        [Fact]
        public void GetText_When_Request_Validation_Disabled_And_Empty_Text_Should_Return_Empty_Body()
        {
            _response.Body = new byte[0];

            _response.GetText(false).Should().BeEmpty();
        }
    }
}
