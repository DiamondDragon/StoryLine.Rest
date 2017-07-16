using Microservice.Membership.Subsys.v1.Actions;
using StoryLine;

namespace Microservice.Membership.Subsys.v1.Resources.User
{
    public class GetAllTestDataFixture
    {
        public GetAllTestDataFixture()
        {
            Scenario.New()
                .When()
                .Performs<AddUser>(x => x
                    .FirstName("Last")
                    .LastName("User")
                    .Age(23))
                .Performs<AddUser>(x => x
                    .FirstName("Middle")
                    .LastName("User")
                    .Age(33))
                .Performs<AddUser>(x => x
                    .FirstName("First")
                    .LastName("User")
                    .Age(18))
                .Run();
        }
    }
}