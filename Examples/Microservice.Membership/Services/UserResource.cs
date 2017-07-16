using System;
using System.Collections.Immutable;
using System.Linq;
using Microservice.Membership.Contracts;

namespace Microservice.Membership.Services
{
    public class UserResource : IUserResource
    {
        private static ImmutableList<User> _users = ImmutableList<User>.Empty;

        public User Get(Guid id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public User Create(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Id = Guid.NewGuid();
            user.CreatedOn = DateTime.UtcNow;
            user.UpdatedOn = DateTime.UtcNow;

            _users = _users.Add(user);

            return user;
        }

        public User Update(Guid id, User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingUser = _users.FirstOrDefault(x => x.Id == id);
            if (existingUser == null)
                throw new ArgumentException("Requested user was not found. UserId = " + id);

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Age = user.Age;
            existingUser.UpdatedOn = DateTime.UtcNow;

            return existingUser;
        }

        public bool Exists(Guid id)
        {
            return _users.Any(x => x.Id == id);
        }

        public void Delete(Guid id)
        {
            var existingUser = _users.FirstOrDefault(x => x.Id == id);
            if (existingUser == null)
                throw new ArgumentException("Requested user was not found. UserId = " + id);

            _users = _users.Remove(existingUser);
        }

        public UserCollection GetAll(int skip, int take)
        {
            if (skip < 0)
                throw new ArgumentOutOfRangeException(nameof(skip));
            if (take <= 0)
                throw new ArgumentOutOfRangeException(nameof(take));

            var users =
                (from user in _users
                 orderby user.UpdatedOn descending 
                 select user)
                .Skip(skip)
                .Take(take)
                .ToArray();

            return new UserCollection
            {
                Items = users,
                Count = _users.Count
            };
        }
    }
}
