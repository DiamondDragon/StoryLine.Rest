using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using FluentAssertions;
using StoryLine.Rest.Services;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Services
{
    public class ResponseToTextConverterTests : IDisposable
    {
        private readonly ResponseToTextConverter _underTest;
        private readonly IContentTypeProvider _contentTypeProvider;
        private readonly IResponse _response;

        private readonly Encoding _prevDefaultEncoding;
        private readonly Dictionary<string, string[]> _headers;
        private const string ResponseBodyContent = "Diamond Dragon!!!";

        public ResponseToTextConverterTests()
        {
            _contentTypeProvider = A.Fake<IContentTypeProvider>();
            _response = A.Fake<IResponse>();
            _headers = new Dictionary<string, string[]>();
            _prevDefaultEncoding = Config.DefaultEncoding;

            A.CallTo(() => _response.Headers).Returns(_headers);
            A.CallTo(() => _response.Body).Returns(Config.DefaultEncoding.GetBytes(ResponseBodyContent));

            _underTest = new ResponseToTextConverter(
                _contentTypeProvider
                
                );
        }

        public void Dispose()
        {
            Config.DefaultEncoding = _prevDefaultEncoding;
        }

        [Fact]
        public void GetText_When_Null_Response_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.GetText(null));
        }

        [Fact]
        public void GetText_When_Null_Body_Should_Return_Empty_String()
        {
            A.CallTo(() => _response.Body).Returns(null);

            _underTest.GetText(_response).Should().BeEmpty();
        }

        [Fact]
        public void GetText_When_Empty_Byte_Array_Should_Return_Empty_String()
        {
            A.CallTo(() => _response.Body).Returns(new byte[0]);

            _underTest.GetText(_response).Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetText_When_CharSet_Not_Found_Should_Use_Default_Encoding(string charSet)
        {
            A.CallTo(() => _contentTypeProvider.GetCharSet(_headers)).Returns(charSet);

            _underTest.GetText(_response).Should().Be(ResponseBodyContent);
        }

        [Fact]
        public void GetText_When_CharSet_Found_Should_Use_Discovred_Encoding_To_Get_Text()
        {
            A.CallTo(() => _contentTypeProvider.GetCharSet(_headers)).Returns("utf-32");
            A.CallTo(() => _response.Body).Returns(Encoding.UTF32.GetBytes("Dragon123"));

            _underTest.GetText(_response).Should().Be("Dragon123");
        }

        [Fact]
        public void GetText_When_Encoding_Unknown_Should_Throw_ExpectationException()
        {
            A.CallTo(() => _contentTypeProvider.GetCharSet(_headers)).Returns("xxx");

            Assert.Throws<Exceptions.ExpectationException>(() => _underTest.GetText(_response));
        }

        [Fact]
        public void GetText_When_Failed_To_Construct_String_From_Bytes_Should_Return_Not_Empty_String()
        {
            Config.DefaultEncoding = Encoding.Unicode;
            A.CallTo(() => _response.Body).Returns(Encoding.ASCII.GetBytes("XXXXX"));

            _underTest.GetText(_response).Should().NotBeEmpty();
        }
    }
}
