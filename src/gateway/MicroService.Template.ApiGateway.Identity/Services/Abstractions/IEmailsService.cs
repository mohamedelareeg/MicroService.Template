using BuildingBlocks.Results;

namespace MicroService.Template.ApiGateway.Identity.Services.Abstractions;

public interface IEmailsService
{
    Task<Result<bool>> SendEmail(string email, string message, string? reason, string companyName);
}
