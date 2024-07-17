using BuildingBlocks.Results;
using MicroService.Template.ApiGateway.Controllers.Base;
using MicroService.Template.ApiGateway.Features.Emails.Commands.SendEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Template.ApiGateway.Controllers;
[Authorize(Roles = "Administrator")]
[Route("api/v1/email")]
public class EmailsController : AppControllerBase
{
    public EmailsController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost("SendEmail")]
    public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }
}
