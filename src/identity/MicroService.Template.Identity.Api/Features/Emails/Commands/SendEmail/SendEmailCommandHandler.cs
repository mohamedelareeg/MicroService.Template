using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Emails.Commands.SendEmail;
internal class SendEmailCommandHandler : ICommandHandler<SendEmailCommand, bool>
{
    private readonly IEmailsService _emailsService;

    public SendEmailCommandHandler(IEmailsService emailsService)
    {
        _emailsService = emailsService;
    }

    public async Task<Result<bool>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        return await _emailsService.SendEmail(request.Email, request.Message, null, "Demo Company");
    }
}
