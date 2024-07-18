using BuildingBlocks.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroService.Template.Identity.Api.Controllers.Base;
using MicroService.Template.Identity.Api.Features.Emails.Commands.SendEmail;

namespace MicroService.Template.Identity.Api.Controllers;
[Authorize(Roles = "Administrator")]
[Route("email")]
public class EmailsController : AppControllerBase
{
    public EmailsController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost("sendEmail")]
    public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }
}
