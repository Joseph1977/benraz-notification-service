using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notifications.Domain.NotificationServices.Phones;
using Notifications.WebApi.Authorization;
using Notifications.WebApi.Models.Phones;
using System.Threading.Tasks;

namespace Notifications.WebApi.Controllers
{
    /// <summary>
    /// Phone controller.
    /// </summary>
    [ApiController]
    [Route("/v{version:ApiVersion}/[controller]")]
    public class PhoneController : ControllerBase
    {
        private readonly IPhoneService _phoneService;
        private readonly ILogger<PhoneController> _logger;

        /// <summary>
        /// Creates controller.
        /// </summary>
        /// <param name="phoneService">Phone service.</param>
        /// <param name="logger">Logger.</param>
        public PhoneController(
            IPhoneService phoneService,
            ILogger<PhoneController> logger)
        {
            _phoneService = phoneService;
            _logger = logger;
        }

        /// <summary>
        /// Send sms.
        /// </summary>
        /// <param name="viewModel">Sms view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
#if EnableAuthorization
        [Authorize(NotificationsPolicies.SMS_SEND)]
#endif
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendSmsAsync([FromBody] SmsViewModel viewModel)
        {
            if (viewModel == null)
                return BadRequest();
            await _phoneService.SendSmsAsync(viewModel.Body, viewModel.Recipients, viewModel.Sender);
            return NoContent();
        }
    }
}