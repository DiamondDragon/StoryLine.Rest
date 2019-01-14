using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace StoryLine.Rest.Tests
{
    public class ConfigTests
    {
        //[Fact]
        //public void DefaultEncoding_Should_Be_Utf8()
        //{
        //    Config.DefaultEncoding.WebName.Should().Be("utf-8");
        //}

        [Fact]
        public void DefaultJsonSerializerSettings_Should_Use_CamelCasePropertyNameContactResource_And_NullValueHandling_Equal_To_Ignore()
        {
            Config.DefaultJsonSerializerSettings.Should().BeEquivalentTo(new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            Config.DefaultJsonSerializerSettings.ContractResolver.Should()
                .BeOfType<CamelCasePropertyNamesContractResolver>();
        }

        [Fact]
        public void DefaultTestCaseAttributes_Should_Include_NUnit_And_Xunit_Test_Case_Attributes()
        {
            Config.DefaultTestCaseAttributes.Should().BeEquivalentTo(
                "TestAttribute", 
                "TestCaseAttribute", 
                "FactAttribute", 
                "TheoryAttribute");
        }
    }
}
