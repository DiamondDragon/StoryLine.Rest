using System;
using Microservice.Membership.Contracts;

namespace Microservice.Membership.Services
{
    public interface IUserResource
    {
        User Get(Guid id);
        User Create(User user);
        User Update(User user);
        bool Exists(Guid id);
        void Delete(Guid id);
        UserCollection GetAll(int skip, int take);
    }
}