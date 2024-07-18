namespace MicroService.Template.Identity.Api.Services.Abstractions;

public interface IUser
{
    string? Id { get; }
    string? Email { get; }
}
