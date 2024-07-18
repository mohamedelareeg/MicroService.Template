using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Authentication.Commands.ResetPassword;
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
