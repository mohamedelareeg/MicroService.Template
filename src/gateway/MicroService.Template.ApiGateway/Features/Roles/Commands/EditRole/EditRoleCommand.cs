using BuildingBlocks.Messaging;

namespace MicroService.Template.ApiGateway.Features.Roles.Commands.EditRole;
public class EditRoleCommand : ICommand<bool>
{
    public string RoleName { get; set; }
    public string NewRoleName { get; set; }
}
