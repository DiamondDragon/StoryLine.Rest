using System;
using Microservice.Membership.Contracts;
using Microservice.Membership.Services;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Membership.Controllers
{
    [Route("v1/users")]
    public class UserController : Controller
    {
        private const string UseDetailsRoute = "UserDetails";

        private readonly IUserResource _userResource;

        public UserController(IUserResource userResource)
        {
            _userResource = userResource ?? throw new ArgumentNullException(nameof(userResource));
        }

        [HttpGet]
        public IActionResult GetAll(int skip = 0, int take = 10)
        {
            if (skip < 0)
                throw new ArgumentOutOfRangeException(nameof(skip));
            if (take <= 0)
                throw new ArgumentOutOfRangeException(nameof(take));

            var userCollection = _userResource.GetAll(skip, take);

            return Ok(userCollection);
        }

        [HttpGet("{id}", Name = UseDetailsRoute)]
        public IActionResult Get(Guid id)
        {
            var user = _userResource.Get(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpHead("{id}")]
        public IActionResult Exists(Guid id)
        {
            if (_userResource.Exists(id))
                return Ok();

            return NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody]User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var createdUser = _userResource.Create(user);

            return Created(Url.RouteUrl(UseDetailsRoute, new {id = createdUser.Id}), user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!_userResource.Exists(id))
                return NotFound();

            return Ok(_userResource.Update(id, user));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!_userResource.Exists(id))
                return NotFound();

            _userResource.Delete(id);

            return NoContent();
        }
    }
}
