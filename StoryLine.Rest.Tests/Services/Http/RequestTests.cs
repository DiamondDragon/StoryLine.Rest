using System;
using FluentAssertions;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Services.Http
{
    public class RequestTests
    {
        private readonly Request _underTest;

        public RequestTests()
        {
            _underTest = new Request();
        }

        [Fact]
        public void Headers_Should_Not_Be_Null_By_Default()
        {
            _underTest.Headers.Should().NotBeNull();
        }

        [Fact]
        public void Header_When_Null_Assigned_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.Headers = null);
        }
    }
}
