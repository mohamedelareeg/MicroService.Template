using BuildingBlocks.Messaging;
using MicroService.Template.Shared.Dtos.AuthonticationDtos;

namespace MicroService.Template.ApiGateway.Features.Authentication.Commands.Login;
public class LoginCommand : ICommand<LoginDto>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
