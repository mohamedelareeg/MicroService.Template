using BuildingBlocks.Results;
using MicroService.Template.ApiGateway.Controllers.Base;
using MicroService.Template.ApiGateway.Features.Authentication.Commands.Login;
using MicroService.Template.ApiGateway.Features.Authentication.Commands.Register;
using MicroService.Template.ApiGateway.Features.Authentication.Commands.ResetPassword;
using MicroService.Template.ApiGateway.Features.Authentication.Commands.SendResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MicroService.Template.Shared.Dtos.AuthonticationDtos;

namespace MicroService.Template.ApiGateway.Controllers;
[Route("api/v1/auth")]
public class AuthController : AppControllerBase
{
    public AuthController(ISender sender)
        : base(sender)
    {
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request, CancellationToken cancellationToken)
    {
        Result<LoginDto> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand request, CancellationToken cancellationToken)
    {
        Result<RegisterDto> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }

    [HttpPost("SendResetPasswordCode")]
    public async Task<IActionResult> SendResetPassword([FromQuery] SendResetPasswordCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }
}
