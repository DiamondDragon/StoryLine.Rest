using StoryLine;
using StoryLine.Rest.Actions;
using StoryLine.Rest.Actions.Extensions;
using StoryLine.Rest.Expectations;
using StoryLine.Rest.Expectations.Extensions;
using StoryLine.Rest.Extensions;
using StoryLine.Rest.Services.Http;
using StoryLine.Utils.Actions;
using StoryLine.Utils.Services;
using Xunit;

namespace Microservice.Membership.Subsys.v1.Resources.User
{
    public class Post : ApiTestBase
    {
        [Fact]
        public void When_Existing_User_Requested_Should_Return_201_Location_Header_And_Expected_Body()
        {
            Scenario.New()
                .When()
                    .Performs<HttpRequest>(x => x
                        .Method("POST")
                        .Header("Content-Type", "application/json")
                        .JsonObjectBody(new
                        {
                            firstName = "dragon1",
                            lastName = "dragon2",
                            age = 22
                        })
                        .Url("v1/users"))
                    .Performs<Transform, IResponse>((x, response) => x
                        .From(response.GetText())
                        .To<Models.User>()
                        .Using<JsonConverter>())
                .Then()
                    .Expects<HttpResponse, Models.User>((x, user) => x
                        .Status(201)
                        .Header("Location")
                            .Contains("v1/users/" + user.Id)
                        .JsonBody()
                            .Matches()
                                .ResourceFile(new[] { "$.id", "$.createdOn", "$.updatedOn" }))
                .Run();
        }
    }
}