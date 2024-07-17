namespace MicroService.Template.ApiGateway.Identity.Services.Abstractions;

public interface IUser
{
    string? Id { get; }
    string? Email { get; }
}
