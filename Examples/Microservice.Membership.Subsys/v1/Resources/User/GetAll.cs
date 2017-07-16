using FluentAssertions;
using Microservice.Membership.Subsys.v1.Models;
using StoryLine;
using StoryLine.Rest.Actions;
using StoryLine.Rest.Expectations;
using StoryLine.Rest.Extensions;
using StoryLine.Rest.Services.Http;
using StoryLine.Utils.Actions;
using StoryLine.Utils.Services;
using Xunit;

namespace Microservice.Membership.Subsys.v1.Resources.User
{
    public class GetAll : ApiTestBase, IClassFixture<GetAllTestDataFixture>
    {
        [Fact]
        public void When_Neither_Skip_Nor_Take_Specified_Should_Return_Not_Less_Then_3_Items()
        {
            var actor = new Actor();

            Scenario.New()
                .When(actor)
                    .Performs<HttpRequest>(x => x
                        .Method("GET")
                        .Url("v1/users"))
                    .Performs<Transform, IResponse>((x, response) => x
                        .From(response.GetText())
                        .To<UserCollection>()
                        .Using<JsonConverter>())
                .Then()
                    .Expects<HttpResponse>(x => x
                        .Status(200))
                .Run();

            var userColletion = actor.Artifacts.Get<UserCollection>();

            userColletion.Count.Should().BeGreaterOrEqualTo(3);
            AssertIsExpectedUser(userColletion.Items[0], new Models.User { FirstName = "First", LastName = "User", Age = 18 });
            AssertIsExpectedUser(userColletion.Items[1], new Models.User { FirstName = "Middle", LastName = "User", Age = 33 });
            AssertIsExpectedUser(userColletion.Items[2], new Models.User { FirstName = "Last", LastName = "User", Age = 23 });
        }

        [Fact]
        public void When_Skip_And_Take_Are_Specified_Should_Return_1_Expected_Result()
        {
            var actor = new Actor();

            Scenario.New()
                .When(actor)
                .Performs<HttpRequest>(x => x
                    .Method("GET")
                    .Url("v1/users?skip=1&take=1"))
                .Performs<Transform, IResponse>((x, response) => x
                    .From(response.GetText())
                    .To<UserCollection>()
                    .Using<JsonConverter>())
                .Then()
                .Expects<HttpResponse>(x => x
                    .Status(200))
                .Run();

            var userColletion = actor.Artifacts.Get<UserCollection>();

            userColletion.Count.Should().BeGreaterOrEqualTo(3);
            AssertIsExpectedUser(userColletion.Items[0], new Models.User { FirstName = "Middle", LastName = "User", Age = 33 });
        }

        private static void AssertIsExpectedUser(Models.User actual, Models.User expected)
        {
            actual.ShouldBeEquivalentTo(
                expected,
                p => p
                    .Excluding(x => x.Id)
                    .Excluding(x => x.CreatedOn)
                    .Excluding(x => x.UpdatedOn));
        }
    }
}
