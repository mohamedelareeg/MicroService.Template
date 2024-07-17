using BuildingBlocks.Messaging;

namespace MicroService.Template.ApiGateway.Features.Roles.Commands.DeleteRole;
public class DeleteRoleCommand : ICommand<bool>
{
    public string RoleName { get; set; }
}
