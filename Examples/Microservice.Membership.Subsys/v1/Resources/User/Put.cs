using System;
using FluentAssertions;
using Microservice.Membership.Subsys.v1.Actions;
using StoryLine;
using StoryLine.Rest.Actions;
using StoryLine.Rest.Actions.Extensions;
using StoryLine.Rest.Expectations;
using StoryLine.Rest.Extensions;
using StoryLine.Rest.Services.Http;
using StoryLine.Utils.Actions;
using StoryLine.Utils.Expectations;
using StoryLine.Utils.Services;
using Xunit;

namespace Microservice.Membership.Subsys.v1.Resources.User
{
    public class Put : ApiTestBase
    {
        [Fact]
        public void When_User_Not_Found_Should_Return_404()
        {
            Scenario.New()
                .When()
                .Performs<HttpRequest>(x => x
                    .Method("PUT")
                    .Header("Content-Type", "application/json")
                    .Url("/v1/users/" + Guid.NewGuid())
                    .JsonObjectBody(new
                    {
                        firstName = "dragon1",
                        lastName = "dragon2",
                        Age = 22
                    }))
                .Then()
                .Expects<HttpResponse>(x => x
                    .Status(404))
                .Run();
        }

        [Fact]
        public void When_Existing_User_Requested_Should_Return_200_And_Update_Fields()
        {
            Scenario.New()
                .Given()
                    .HasPerformed<AddUser>()
                    .HasPerformed<Transform, IResponse>((x, response) => x
                        .From(response.GetText())
                        .To<Models.User>()
                        .Using<JsonConverter>())
                .When()
                    .Performs<HttpRequest, Models.User>((x, user) => x
                        .Method("PUT")
                        .Header("Content-Type", "application/json")
                        .JsonObjectBody(new
                        {
                            firstName = "dragon1",
                            lastName = "dragon2",
                            age = 22
                        })
                        .Url($"/v1/users/{user.Id}"))
                    .Performs<Transform, IResponse>((x, response) => x
                        .From(response.GetText())
                        .To<Models.User>()
                        .Using<JsonConverter>())
                .Then()
                    .Expects<HttpResponse>(x => x
                        .Status(200))
                    .Expects<Artifact<Models.User>>(x => x
                        .Meets(a => a.ShouldBeEquivalentTo(new Models.User
                            {
                                FirstName = "dragon1",
                                LastName = "dragon2",
                                Age = 22
                            },
                            p => p.Excluding(m => m.Id).Excluding(m => m.CreatedOn).Excluding(m => m.UpdatedOn))))
                .Run();
        }
    }
}