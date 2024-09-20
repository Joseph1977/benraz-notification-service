using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notifications.Domain.Messages;
using Notifications.Domain.Messages.MessagesFilter;
using Notifications.Domain.NotificationServices.Emails;
using Notifications.WebApi.Authorization;
using Notifications.WebApi.Models.Emails;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Notifications.WebApi.Controllers
{
    /// <summary>
    /// Emails controller.
    /// </summary>
    [ApiController]
    [Route("/v{version:ApiVersion}/[controller]")]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailsService _emailService;
        private readonly IMessageFilterProvider _messageFilterProvider;
        private readonly ILogger<EmailsController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates controller.
        /// </summary>
        /// <param name="emailService">Email service.</param>
        /// <param name="messageFilterProvider">Message filter provider.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        public EmailsController(
            IEmailsService emailService,
            IMessageFilterProvider messageFilterProvider,
            ILogger<EmailsController> logger,
            IMapper mapper)
        {
            _emailService = emailService;
            _messageFilterProvider = messageFilterProvider;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="viewModel">Email model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
#if EnableAuthorization
        [Authorize(NotificationsPolicies.EMAILS_SEND)]
#endif
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequestViewModel viewModel)
        {
            if (viewModel == null)
                return BadRequest();

            try
            {
                var emailInfo = _mapper.Map<EmailBasicInfo>(viewModel.BasicInfo);
                var whiteLabelParams = viewModel.WhileLabelParams;
                var emailParams = viewModel.EmailParams;

                if (!string.IsNullOrEmpty(emailInfo.To))
                {
                    emailInfo.To = _messageFilterProvider.FinalToAddressToSend(emailInfo.To, MessageType.eEmail);
                    if (emailInfo.To == null)
                    {
                        string msg = "Email Not Sent -- Filter return no email, nothing to send!";
                        _logger.LogInformation(msg);
                        return Ok(msg);
                    }
                }

                if (emailInfo.Tos != null)
                {
                    emailInfo.Tos = _messageFilterProvider.FinalTosAddressToSend(emailInfo.Tos, MessageType.eEmail);
                    if (emailInfo.Tos == null)
                    {
                        string msg = "Email Not Sent -- Filter return no email, nothing to send!";
                        _logger.LogInformation(msg);
                        return Ok(msg);
                    }
                }

                try
                {
                    if (emailInfo.Tos != null)
                    {
                        await _emailService.SendMultipleWhiteLabeledEmailTemplateAsync(
                            emailInfo.From,
                            emailInfo.Tos,
                            emailInfo.Subject,
                            emailInfo.DisplayName,
                            emailInfo.TempId,
                            emailParams,
                            whiteLabelParams,
                            emailInfo.Attachments);

                        _logger.LogInformation("{0} -- From: {1} Subject: {2}", MessageStatus.MessageSent,
                            emailInfo.From, emailInfo.Subject);
                        return Ok($"Email Batch sent - {emailInfo.From}: {emailInfo.Subject}");
                    }
                    else
                    {
                        await _emailService.SendWhiteLabeledEmailTemplateAsync(
                            emailInfo.From,
                            emailInfo.To,
                            emailInfo.Subject,
                            emailInfo.DisplayName,
                            emailInfo.TempId,
                            emailParams,
                            whiteLabelParams);

                        _logger.LogInformation("{0} -- To: {1}", MessageStatus.MessageSent, emailInfo.To);
                        return Ok($"Email sent to {emailInfo.To}");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("{0} -- {1}", MessageStatus.MessageNotSent, e.Message);
                    if (!string.IsNullOrEmpty(emailInfo.To))
                        return BadRequest(string.Format("Email not sent to: {0}", emailInfo.To));

                    return BadRequest($"Email batch not sent - {emailInfo.From}: {emailInfo.Subject}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("{0} -- {1}", MessageStatus.MessageNotSent, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    string.Format("Exception message:", e.Message));
            }
        }

        /// <summary>
        /// Send email without using template.
        /// </summary>
        /// <param name="viewModel">Email model.</param>
        /// <returns>Action result.</returns>
        [HttpPost ("HtmlContent")]
#if EnableAuthorization
        [Authorize(NotificationsPolicies.EMAILS_SEND)]
#endif
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendEmailHtmlContentAsync([FromBody] EmailRequestHtmlContentViewModel viewModel)
        {
            if (viewModel == null)
                return BadRequest();

            try
            {
                var emailInfo = _mapper.Map<EmailHtmlContentViewModel>(viewModel);


                if (emailInfo.Tos != null)
                {
                    emailInfo.Tos = _messageFilterProvider.FinalTosAddressToSend(emailInfo.Tos, MessageType.eEmail);
                    if (emailInfo.Tos == null)
                    {
                        string msg = "Email Not Sent -- Filter return no email, nothing to send!";
                        _logger.LogInformation(msg);
                        return Ok(msg);
                    }
                }
                else
                {
                    string msg = "Email Not Sent -- no email list, nothing to send!";
                    _logger.LogInformation(msg);
                    return BadRequest(msg);
                }

                try
                {
                    await _emailService.SendHtmlContentAsync(
                        emailInfo.From,
                        emailInfo.Tos,
                        emailInfo.Subject,
                        emailInfo.DisplayName,
                        emailInfo.HtmlContent);

                    _logger.LogInformation("{0} -- From: {1} Subject: {2}", MessageStatus.MessageSent,
                        emailInfo.From, emailInfo.Subject);
                    return Ok($"Email Batch sent - {emailInfo.From}: {emailInfo.Subject}");

                }
                catch (Exception e)
                {
                    _logger.LogError("{0} -- {1}", MessageStatus.MessageNotSent, e.Message);
                    return BadRequest($"Email batch not sent - {emailInfo.From}: {emailInfo.Subject}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("{0} -- {1}", MessageStatus.MessageNotSent, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    string.Format("Exception message:", e.Message));
            }
        }
    }
}