using System;
using Microservice.Membership.Subsys.v1.Models;
using StoryLine.Contracts;

namespace Microservice.Membership.Subsys.v1.Actions
{
    public class AddUser : IActionBuilder
    {
        private readonly User _user;

        public AddUser()
        {
            _user = GenFu.GenFu.New<User>();
        }

        public AddUser FirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(firstName));

            _user.FirstName = firstName;

            return this;
        }

        public AddUser LastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(lastName));

            _user.LastName = lastName;

            return this;
        }

        public AddUser Age(int age)
        {
            if (age <= 0)
                throw new ArgumentOutOfRangeException(nameof(age));

            _user.Age = age;

            return this;
        }

        public IAction Build()
        {
            return new AddUserAction(_user);
        }
    }
}
