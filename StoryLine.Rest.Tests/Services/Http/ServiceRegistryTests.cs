using System;
using FluentAssertions;
using StoryLine.Rest.Services.Http;
using Xunit;

namespace StoryLine.Rest.Tests.Services.Http
{
    public class ServiceRegistryTests
    {
        private readonly ServiceRegistry _underTest;

        public ServiceRegistryTests()
        {
            _underTest = new ServiceRegistry();
        }

        [Fact]
        public void Add_When_Null_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.Add(null));
        }

        [Theory]
        [InlineData("Service1", "Service1")]
        [InlineData("Service1", "SERVICE1")]
        public void Add_When_The_Save_Service_Added_Twice_Should_Throw_InvalidOperationExceptions(string service1, string service2)
        {
            var config1 = new ServiceConfig(service1, "http://google.com", TimeSpan.FromHours(5), false);
            var config2 = new ServiceConfig(service2, "http://yandex.ru", TimeSpan.FromHours(5), false);

            _underTest.Add(config1);
            Assert.Throws<InvalidOperationException>(() => _underTest.Add(config2));
        }

        [Fact]
        public void GetAll_Should_Return_All_Configs_Which_Were_Registered()
        {
            var config1 = new ServiceConfig("Service1", "http://google.com", TimeSpan.FromHours(5), false);
            var config2 = new ServiceConfig("Service2", "http://yandex.ru", TimeSpan.FromHours(5), false);

            _underTest.Add(config1);
            _underTest.Add(config2);

            _underTest.GetAll().Should().BeEquivalentTo(new[] { config1, config2 });
        }

        [Theory]
        [InlineData("Service1", "Service1")]
        [InlineData("Service1", "SERVICE1")]
        public void Get_When_The_Save_Service_Added_Twice_Should_Throw_InvalidOperationExceptions(string service1, string service2)
        {
            var config1 = new ServiceConfig(service1, "http://google.com", TimeSpan.FromHours(5), false);
            var config2 = new ServiceConfig("test123", "http://yandex.ru", TimeSpan.FromHours(5), false);

            _underTest.Add(config2);
            _underTest.Add(config1);

            _underTest.Get(service2).Should().BeEquivalentTo(config1);
        }
    }
}
