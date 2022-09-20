using Application.Command.Notification;
using Application.Models.Notification;
using Application.Models.User;
using Application.Queries;
using Application.Queries.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllergyTrackAPI.Controllers
{
    [Produces("application/json")]
    [Route("notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public NotificationController(ILogger<UserController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        ///     Returns notification types and categories
        /// </summary>
        /// <returns>All notification types and categories</returns>
        /// <response code="200">All notification types and categories</response>
        [HttpGet]
        [Route("types"), Authorize]
        [ProducesResponseType(typeof(GetAllNotificationsTypesResponse), StatusCodes.Status200OK)]
        public async Task<GetAllNotificationsTypesResponse> GetAllNotificationTypes(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetAllNotificationTypesQuery(), cancellationToken);
        }

        /// <summary>
        ///     Adds a list of notifications
        /// </summary>
        /// <remarks>
        /// * StartFrom - Date in UTC
        ///     - Start date for sending notifications
        ///     - Cannot be less than the current date.
        ///     - Cannot be greater than the current date plus three months
        /// * RepetitionIntervalInDays - number of days
        ///     - Cannot be less than 1
        ///     - Cannot be greater than 31
        /// </remarks>
        /// <param name="addNotificationsRequest"></param>
        /// <returns></returns>
        /// <response code="201">All notification types and categories</response>
        [HttpPost]
        [Route(""), Authorize]
        [ProducesResponseType(typeof(GetAllNotificationsTypesResponse), StatusCodes.Status201Created)]
        public async Task AddNotifications(NotificationSendingModel addNotificationsRequest, CancellationToken cancellationToken)
        {
            await _mediator.Send(new AddNotificationCommand(addNotificationsRequest), cancellationToken);
        }

        /// <summary>
        ///     Returns all user notifications 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>All user notifications/returns>
        /// <response code="200">All user notifications</response>
        [HttpGet]
        [Route("all"), Authorize]
        [ProducesResponseType(typeof(GetAllUserNotificationsResponse), StatusCodes.Status200OK)]
        public async Task<GetAllUserNotificationsResponse> GetAllUserNotifications(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetAllUserNotificationQuery(), cancellationToken);
        }
    }
}