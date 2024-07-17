using BuildingBlocks.Messaging;
using MicroService.Template.Shared.Dtos.AuthonticationDtos;

namespace MicroService.Template.ApiGateway.Features.Authentication.Commands.Register;
public class RegisterCommand : ICommand<RegisterDto>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
