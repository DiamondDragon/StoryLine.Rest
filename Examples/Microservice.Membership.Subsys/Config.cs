using System.Reflection;
using Microservice.Membership.Subsys.v1.Models;
using Microsoft.Extensions.Configuration;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Microservice.Membership.Subsys
{
    public class Config
    {
        private static string ServiceAddress { get; set; }

        public Config()
        {
            GenFu.GenFu.Configure<User>()
                .Fill(p => p.Age).WithinRange(19, 25)
                .Fill(p => p.Id, () => null)
                .Fill(p => p.CreatedOn, () => null)
                .Fill(p => p.UpdatedOn, () => null);

            var config = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .Build();

            ServiceAddress = config["ServiceAddress"];

            StoryLine.Rest.Config.AddServiceEndpont("Membership", ServiceAddress);
            StoryLine.Rest.Config.SetAssemblies(typeof(Config).GetTypeInfo().Assembly);
        }
    }
}
