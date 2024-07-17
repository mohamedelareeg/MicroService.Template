using BuildingBlocks.Messaging;

namespace MicroService.Template.ApiGateway.Features.Roles.Commands.CreateRole;
public class CreateRoleCommand : ICommand<bool>
{
    public string RoleName { get; set; }
}
