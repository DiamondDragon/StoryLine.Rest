using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microservice.Membership.Subsys;
using Microservice.Membership.Subsys.v1.Models;
using Microsoft.Extensions.Configuration;

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
