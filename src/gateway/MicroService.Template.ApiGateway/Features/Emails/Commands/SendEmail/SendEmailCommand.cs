using BuildingBlocks.Messaging;

namespace MicroService.Template.ApiGateway.Features.Emails.Commands.SendEmail;
public class SendEmailCommand : ICommand<bool>
{
    public string Email { get; set; }
    public string Message { get; set; }
}
