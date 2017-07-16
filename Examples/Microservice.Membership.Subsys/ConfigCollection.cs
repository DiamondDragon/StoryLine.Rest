using Xunit;

namespace Microservice.Membership.Subsys
{
    [CollectionDefinition(nameof(Config))]
    public class ConfigCollection : ICollectionFixture<Config>
    {
    }
}