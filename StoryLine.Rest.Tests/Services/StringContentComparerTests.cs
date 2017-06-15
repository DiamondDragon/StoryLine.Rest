using StoryLine.Exceptions;
using StoryLine.Rest.Services;
using Xunit;

namespace StoryLine.Rest.Tests.Services
{
    public class StringContentComparerTests
    {
        private readonly StringContentComparer _underTest;

        public StringContentComparerTests()
        {
            _underTest = new StringContentComparer();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("AAA", "AAA")]
        [InlineData("BBB", "BBB")]
        public void Verify_When_Expected_Equal_To_Actual_Should_Not_Throw_Exception(string expected, string actual)
        {
            _underTest.Verify(expected, actual);
        }

        [Theory]
        [InlineData(null, "aaa")]
        [InlineData("aaa", null)]
        [InlineData("aaa", "AAAA")]
        [InlineData("aaa\nBBB", "aaa\nCCC")]
        public void Verify_When_Expected_Not_Equal_To_Actual_Should_Throw_Expectation_Exception(string expected, string actual)
        {
            Assert.Throws<ExpectationException>(() => _underTest.Verify(expected, actual));
        }
    }
}
