using BuildingBlocks.Results;

namespace MicroService.Template.Identity.Api.Services.Abstractions;

public interface IEmailsService
{
    Task<Result<bool>> SendEmail(string email, string message, string? reason, string companyName);
}
