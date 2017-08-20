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

        [Theory]
        [InlineData(100)]
        [InlineData(199)]
        [InlineData(300)]
        [InlineData(400)]
        public void GetText_When_Request_Validation_Enabled_And_Status_Invalid_And_Empty_Text_Should_Throw_Expectation_Exception(int status)
        {
            _response.Status = status;

            Assert.Throws<ExpectationException>(() => _response.GetText());
        }

        [Fact]
        public void GetText_When_Request_Validation_Enabled_And_Empty_Text_Should_Throw_Expectation_Exception()
        {
            _response.Body = new byte[0];

            Assert.Throws<ExpectationException>(() => _response.GetText());
        }

        [Theory]
        [InlineData(100)]
        [InlineData(199)]
        [InlineData(300)]
        [InlineData(400)]
        public void GetText_When_Request_Validation_Disabled_And_Status_Invalid_And_Empty_Text_Should_Return_Expected_Body(int status)
        {
            _response.Status = status;

            _response.GetText(false, false).Should().Be(BodyText);
        }

        [Fact]
        public void GetText_When_Request_Validation_Disabled_And_Empty_Text_Should_Return_Empty_Body()
        {
            _response.Body = new byte[0];

            _response.GetText(false, false).Should().BeEmpty();
        }
    }
}
