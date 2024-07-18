using BuildingBlocks.Messaging;

namespace MicroService.Template.Identity.Api.Features.Authentication.Commands.SendResetPassword;
public class SendResetPasswordCommand : ICommand<bool>
{
    public string Email { get; set; }
}
