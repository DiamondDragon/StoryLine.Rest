using System;
using Microservice.Membership.Subsys.v1.Models;
using StoryLine;
using StoryLine.Contracts;
using StoryLine.Rest.Actions;
using StoryLine.Rest.Actions.Extensions;
using StoryLine.Rest.Expectations;
using StoryLine.Rest.Extensions;
using StoryLine.Rest.Services.Http;

namespace Microservice.Membership.Subsys.v1.Actions
{
    public class AddUserAction : IAction
    {
        private readonly User _user;

        public AddUserAction(User user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public void Execute(IActor actor)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            Scenario.New()
                .When(actor)
                    .Performs<HttpRequest>(x => x
                        .Method("POST")
                        .Header("Content-Type", "application/json")
                        .Url("v1/users")
                        .JsonObjectBody(_user))
                .Then()
                    .Expects<HttpResponse>(x => x
                        .Status(201))
               .Run();
        }
    }
}