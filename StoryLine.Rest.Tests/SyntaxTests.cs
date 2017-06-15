using StoryLine.Rest.Actions;
using StoryLine.Rest.Actions.Extensions;
using StoryLine.Rest.Expectations;
using StoryLine.Rest.Expectations.Extensions;

namespace StoryLine.Rest.Tests
{
    public class SyntaxTests
    {
        public void Test()
        {
            Scenario.New()
                .When()
                    .Performs<HttpRequest>(x => x
                        .Method("POST")
                        .Url("/aaaaa")
                        .JsonObjectBody(new { a = "a" }))
                .Then()
                    .Expects<HttpResponse>(x => x
                        .Url(p => p.StartsWith("ss"))
                        .Service("CRM")
                        .JsonBody()
                            .Matches("xxx")
                        .Header("ssss")
                            .Contains("sss"))
                .Run();
        }
    }
}
