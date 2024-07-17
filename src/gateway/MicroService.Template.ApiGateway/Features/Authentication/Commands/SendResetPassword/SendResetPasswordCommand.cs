using BuildingBlocks.Messaging;

namespace MicroService.Template.ApiGateway.Features.Authentication.Commands.SendResetPassword;
public class SendResetPasswordCommand : ICommand<bool>
{
    public string Email { get; set; }
}
