using BuildingBlocks.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MicroService.Template.Shared.Dtos.AuthonticationDtos;
using MicroService.Template.Identity.Api.Controllers.Base;
using MicroService.Template.Identity.Api.Features.Authentication.Commands.Login;
using MicroService.Template.Identity.Api.Features.Authentication.Commands.Register;
using MicroService.Template.Identity.Api.Features.Authentication.Commands.ResetPassword;
using MicroService.Template.Identity.Api.Features.Authentication.Commands.SendResetPassword;
using Microsoft.AspNetCore.Authorization;

namespace MicroService.Template.Identity.Api.Controllers;
[AllowAnonymous]
[Route("auth")]
public class AuthController : AppControllerBase
{
    public AuthController(ISender sender)
        : base(sender)
    {
    }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request, CancellationToken cancellationToken)
    {
        Result<LoginDto> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand request, CancellationToken cancellationToken)
    {
        Result<RegisterDto> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }

    [HttpPost("sendResetPasswordCode")]
    public async Task<IActionResult> SendResetPassword([FromQuery] SendResetPasswordCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }
}
