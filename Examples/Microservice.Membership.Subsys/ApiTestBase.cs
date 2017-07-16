using System;
using Xunit;

namespace Microservice.Membership.Subsys
{
    [Collection(nameof(Config))]
    public abstract class ApiTestBase : IDisposable
    {
        public virtual void Dispose()
        {
        }
    }
}