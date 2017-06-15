using System;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Services.Http
{
    public class ServiceConfigTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_When_Null_Or_Empty_String_Or_Whitespace_Is_Passed_As_Service_Name_Should_Throw_ArgumentException(string serviceName)
        {
            Assert.Throws<ArgumentException>(() => new ServiceConfig(serviceName, "test", TimeSpan.FromSeconds(5), true));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_When_Null_Or_Empty_String_Or_Whitespace_Is_Passed_As_Base_Address_Should_Throw_ArgumentException(string baseAddress)
        {
            Assert.Throws<ArgumentException>(() => new ServiceConfig("test", baseAddress, TimeSpan.FromSeconds(5), true));
        }
    }
}
