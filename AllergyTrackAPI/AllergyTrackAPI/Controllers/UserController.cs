using Application.Command.User;
using Application.Models.User;
using Application.Queries;
using Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace AllergyTrackAPI.Controllers
{
    [Produces("application/json")]
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public UserController(ILogger<UserController> logger, IConfiguration configuration, IMediator mediator)
        {
            _logger = logger;
            _configuration = configuration;
            _mediator = mediator;
        }

        ///// <summary>
        /////   Authorize User Request
        ///// </summary>
        ///// <param name="authorizeUserRequest"></param>
        ///// <returns>JWT Token</returns>
        ///// <response code="200">Token</response>
        [HttpPost]
        [Route("autorize")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<string> AuthorizeUser([FromBody] AuthorizeUserRequest authorizeUserRequest, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new AuthorizeUserQuery(authorizeUserRequest), cancellationToken);
        }

        /// <summary>
        ///     Returns user info
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns>Basic user information</returns>
        /// <response code="200">Basic user information</response>
        [HttpGet]
        [Route("{userGuid}"), Authorize]
        [ProducesResponseType(typeof(GetUserInfoResponse), StatusCodes.Status200OK)]
        public async Task<GetUserInfoResponse> GetUserInfo(Guid userGuid, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetUserInfoQuery(userGuid), cancellationToken);
        }

        /// <summary>
        ///    Create user
        /// </summary>
        /// <param name="createUserRequest"></param>
        /// <returns>User Id</returns>
        /// <response code="200">User Id</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<Guid> CreateUser(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new CreateUserCommand(createUserRequest), cancellationToken);
        }

        ///// <summary>
        /////    Update user
        ///// </summary>
        ///// <param name="createUserRequest"></param>
        ///// <returns></returns>
        ///// <response code="200"></response>
        //[HttpPut]
        //[Route("")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //public async Task UpdateUser(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken)
        //{

        //}

        ///// <summary>
        /////    Get all user information
        ///// </summary>
        ///// <param name="userGuid"></param>
        ///// <returns>All user info</returns>
        ///// <response code="200">All user info</response>
        //[HttpGet]
        //[Route("/details")]
        //[ProducesResponseType(typeof(UserDetailsResponse), StatusCodes.Status200OK)]
        //public async Task<UserDetailsResponse> GetUserDetails(Guid userGuid, CancellationToken cancellationToken)
        //{

        //}
    }
}