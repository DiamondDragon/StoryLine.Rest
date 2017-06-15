using System;
using System.Reflection;
using FluentAssertions;
using StoryLine.Rest.Services.Resources;
using Xunit;

namespace StoryLine.Rest.Tests.Services.Resources
{
    public class AssemblyProviderTests
    {
        private readonly AssemblyProvider _underTest;

        public AssemblyProviderTests()
        {
            _underTest = new AssemblyProvider();
        }
        
        [Fact]
        public void Assemblies_Should_Return_Empty_Array_By_Default()
        {
            _underTest.Assemblies.Should().BeEmpty();
        }

        [Fact]
        public void Assemblies_When_Assign_Null_Should_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _underTest.Assemblies = null);
        }

        [Fact]
        public void GetAssemblies_Should_Return_Value_Assigned_To_Assembies_Property()
        {
            var assemblies = new Assembly[1];

            _underTest.Assemblies = assemblies;

            _underTest.GetAssemblies().Should().BeSameAs(assemblies);
        }
    }
}
