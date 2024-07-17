using BuildingBlocks.Messaging;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using BuildingBlocks.Results;

namespace MicroService.Template.ApiGateway.Features.Authentication.Commands.ResetPassword;
internal class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, bool>
{
    private readonly IAuthService _authService;

    public ResetPasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ConfirmAndResetPassword(request.Code, request.Email, request.NewPassword);
    }
}
