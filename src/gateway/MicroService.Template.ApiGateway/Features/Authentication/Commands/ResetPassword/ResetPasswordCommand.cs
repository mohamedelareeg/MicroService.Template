using BuildingBlocks.Messaging;

namespace MicroService.Template.ApiGateway.Features.Authentication.Commands.ResetPassword;
public class ResetPasswordCommand : ICommand<bool>
{
    public string Code { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
}
