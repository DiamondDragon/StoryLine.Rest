using System;
using Microservice.Membership.Subsys.v1.Actions;
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
    public class Delete : ApiTestBase
    {
        [Fact]
        public void When_User_Not_Found_Should_Return_404()
        {
            Scenario.New()
                .When()
                .Performs<HttpRequest>(x => x
                    .Url("/v1/users/" + Guid.NewGuid()))
                .Then()
                    .Expects<HttpResponse>(x => x
                        .Status(404))
                .Run();
        }

        [Fact]
        public void When_Existing_User_Requested_Should_Return_204()
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
                        .Method("DELETE")
                        .Url($"/v1/users/{user.Id}"))
                .Then()
                    .Expects<HttpResponse>(x => x
                        .Status(204))
                .Run();
        }
    }
}