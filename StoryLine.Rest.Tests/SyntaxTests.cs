using StoryLine.Rest.Actions;
using StoryLine.Rest.Actions.Extensions;
using StoryLine.Rest.Expectations;
using StoryLine.Rest.Expectations.Extensions;
using Xunit;

namespace StoryLine.Rest.Tests
{
    public class SyntaxTests
    {
        //[Fact]
        public void Test()
        {
            Scenario.New()
                .When()
                    .Performs<HttpRequest>(x => x
                        .Method("POST")
                        .Url("/aaaaa?xxx=a")
                        .QueryParam("a", "b")
                        .QueryParam("c", "d")
                        .JsonObjectBody(new { a = "a" }))
                .Then()
                    .Expects<HttpResponse>(x => x
                        .Url(p => p.StartsWith("ss"))
                        .Service("CRM")
                        .TextBody()
                            .Matches(p => p.Contains("xxx"))
                        .JsonBody()
                            .Matches("xxx")
                        .Header("ssss", p => p.Contains("xxx")))
                .Run();
        }
    }
}
