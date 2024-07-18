using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Authentication.Commands.SendResetPassword;
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
