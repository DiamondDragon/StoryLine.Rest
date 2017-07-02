using System.Reflection;
using FluentAssertions;
using StoryLine.Rest.Services.Resources;
using Xunit;

namespace StoryLine.Rest.Tests.Services.Resources
{
    public class ResourceContentProviderTests
    {
        private readonly ResourceContentProvider _underTest;

        public ResourceContentProviderTests()
        {
            var assemblyProvider = new AssemblyProvider
            {
                Assemblies = new[]
                {
                    typeof(ResourceContentProviderTests).GetTypeInfo().Assembly
                }
            };

            _underTest = new ResourceContentProvider(assemblyProvider, new StackTraceProvider(), new MethodDetailsFilter(assemblyProvider));
        }

        [Fact]
        public void GetAsString_When_No_Name_Specified_Should_Return_Content_Matching_Test_Case_Name()
        {
            _underTest.GetAsString().Should().Be("{ \"test\" : \"test\" }");
        }

        [Fact]
        public void GetAsString_When_Resource_Name_Spacied_And_Partial_Match_Should_Return_Content_Matching_Test_Case_Name()
        {
            _underTest.GetAsString("Test").Should().Be("{ \"xxx\" : \"xxx\" }");
        }

        [Fact]
        public void GetAsString_When_Resource_Name_Spacied_And_Exact_Match_Should_Return_Content_Matching_Test_Case_Name()
        {
            _underTest.GetAsString("StoryLine.Rest.Tests.Services.Resources.ResourceContentProviderTests.Exact.json", true).Should().Be("{ \"aaa\" : \"aaa\" }");
        }

        [Fact]
        public void GetAsString_When_No_Name_Specified_And_Resource_Not_Found_Should_Return_Empty_String()
        {
            _underTest.GetAsString().Should().BeEmpty();
        }
    }
}
