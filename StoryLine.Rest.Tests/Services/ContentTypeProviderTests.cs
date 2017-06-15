using System;
using System.Collections.Generic;
using FluentAssertions;
using StoryLine.Rest.Services;
using Xunit;

namespace StoryLine.Rest.Tests.Services
{
    public class ContentTypeProviderTests
    {
        private readonly ContentTypeProvider _underTest;
        private readonly Dictionary<string, string[]> _lowerCaseHeaders;
        private readonly Dictionary<string, string[]> _upperCaseHeaders;

        public ContentTypeProviderTests()
        {
            _lowerCaseHeaders = new Dictionary<string, string[]>
            {
                ["content-type"] = new[] { "application/json; charset=utf-8"}
            };
            _upperCaseHeaders = new Dictionary<string, string[]>
            {
                ["CONTENT-TYPE"] = new[] { "application/json; charset=utf-8" }
            };

            _underTest = new ContentTypeProvider();
        }

        [Fact]
        public void GetContentType_When_Null_Headers_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.GetContentType(null));
        }
        
        [Fact]
        public void GetContentType_When_Content_Type_Not_Found_Should_Return_Empty_String()
        {
            _underTest.GetContentType(new Dictionary<string, string[]>()).Should().BeEmpty();
        }

        [Fact]
        public void GetContentType_When_Content_Type_Can_Not_Be_Parsed_Should_Return_Header_Value()
        {
            var headers = new Dictionary<string, string[]>
            {
                ["Content-Type"] = new[] { "text/plain" }
            };

            _underTest.GetContentType(headers).Should().Be("text/plain");
        }

        [Fact]
        public void GetContentType_When_Content_Type_Contains_CharSet_Should_Return_Only_Mime_Type()
        {
            _underTest.GetContentType(_lowerCaseHeaders).Should().Be("application/json");
        }

        [Fact]
        public void GetContentType_When_Content_Type_Header_Is_In_Upper_Case_Should_Return_Expected_Mime_Type()
        {
            _underTest.GetContentType(_upperCaseHeaders).Should().Be("application/json");
        }

        [Fact]
        public void GetCharSet_When_Null_Headers_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.GetCharSet(null));
        }

        [Theory]
        [InlineData("aaaa")]
        [InlineData("aaa;bbbb;cccc")]
        [InlineData("aaa;bbbb")]
        public void GetCharSet_When_Parsed_String_Contains_Not_Two_Elements_Should_Return_Empty_String(string headerValue)
        {
            var headers = new Dictionary<string, string[]>
            {
                ["Content-Type"] = new[] { headerValue }
            };

            _underTest.GetCharSet(headers).Should().BeEmpty();
        }

        [Fact]
        public void GetCharSet_When_Content_Type_Contains_CharSet_Should_Return_CharSet_Name()
        {
            _underTest.GetCharSet(_lowerCaseHeaders).Should().Be("utf-8");
        }


        [Fact]
        public void GetCharSet_When_Content_Type_Header_Name_Is_Upper_Case_Should_Return_CharSet_Name()
        {
            _underTest.GetCharSet(_upperCaseHeaders).Should().Be("utf-8");
        }
    }
}
