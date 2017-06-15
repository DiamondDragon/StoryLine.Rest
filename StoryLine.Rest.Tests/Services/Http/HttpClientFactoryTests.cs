using System;
using System.Collections.Generic;
using System.Net.Http;
using FakeItEasy;
using FluentAssertions;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Services.Http
{
    public class HttpClientFactoryTests
    {
        private readonly IServiceRegistry _serviceRegistry;
        private readonly HttpClientFactory _underTest;
        private readonly IServiceConfig _config1;
        private const string ServiceName = "service1";

        public HttpClientFactoryTests()
        {
            _serviceRegistry = A.Fake<IServiceRegistry>();

            _config1 = A.Fake<IServiceConfig>();

            A.CallTo(() => _serviceRegistry.Get(ServiceName)).Returns(_config1);
            A.CallTo(() => _serviceRegistry.GetAll()).Returns(new List<IServiceConfig> { _config1, A.Fake<IServiceConfig>() });

            A.CallTo(() => _config1.AllowRedirect).Returns(true);
            A.CallTo(() => _config1.Timeout).Returns(TimeSpan.FromSeconds(222));
            A.CallTo(() => _config1.BaseAddress).Returns("http://google.com");

            A.CallTo(() => A.Fake<IServiceConfig>().AllowRedirect).Returns(false);
            A.CallTo(() => A.Fake<IServiceConfig>().Timeout).Returns(TimeSpan.FromSeconds(333));
            A.CallTo(() => A.Fake<IServiceConfig>().BaseAddress).Returns("http://yandex.ru");

            _underTest = new HttpClientFactory(
                _serviceRegistry
            );
        }

        [Fact]
        public void Create_When_Service_Name_Not_Specified_And_Multiple_Endpoints_Exists_Should_Create_Default_HttpClient()
        {
            var result = (HttpClient)_underTest.Create(null);

            result.BaseAddress.Should().BeNull();
            result.Timeout.Should().Be(TimeSpan.FromSeconds(100));
        }

        [Fact]
        public void Create_When_Service_Name_Not_Specified_And_Signle_Endpoints_Exists_Should_Create_Default_HttpClient()
        {
            A.CallTo(() => _serviceRegistry.GetAll()).Returns(new List<IServiceConfig> { _config1 });

            var result = (HttpClient)_underTest.Create(null);

            result.BaseAddress.ToString().Should().Be("http://google.com/");
            result.Timeout.Should().Be(TimeSpan.FromSeconds(222));
        }

        [Fact]
        public void Create_When_ServiceName_Specified_But_Not_Found_Should_Throw_ExpectationException()
        {
            A.CallTo(() => _serviceRegistry.Get(ServiceName)).Returns(null);

            Assert.Throws<Exceptions.ExpectationException>(() => _underTest.Create(ServiceName));
        }

        [Fact]
        public void Create_When_ServiceName_Specified_Found_Should_Construct_HttpClient()
        {
            var result = (HttpClient)_underTest.Create(ServiceName);

            result.BaseAddress.ToString().Should().Be("http://google.com/");
            result.Timeout.Should().Be(TimeSpan.FromSeconds(222));
        }
    }
}
