using System;
using FluentAssertions;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Services.Http
{
    public class ResponseTests
    {
        private readonly Response _underTest;

        public ResponseTests()
        {
            _underTest = new Response();
        }

        [Fact]
        public void Headers_Should_Not_Be_Null_By_Default()
        {
            _underTest.Headers.Should().NotBeNull();
        }

        [Fact]
        public void Headers_When_Set_Null_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.Headers = null);
        }
    }
}
