using BuildingBlocks.Messaging;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using BuildingBlocks.Results;

namespace MicroService.Template.ApiGateway.Features.Authentication.Commands.SendResetPassword;
internal class SendResetPasswordCommandHandler : ICommandHandler<SendResetPasswordCommand, bool>
{
    private readonly IAuthService _authService;

    public SendResetPasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Result<bool>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _authService.SendResetPasswordCode(request.Email);
    }
}
