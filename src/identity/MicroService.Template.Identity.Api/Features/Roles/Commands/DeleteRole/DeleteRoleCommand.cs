using BuildingBlocks.Messaging;

namespace MicroService.Template.Identity.Api.Features.Roles.Commands.DeleteRole;
public class DeleteRoleCommand : ICommand<bool>
{
    public string RoleName { get; set; }
}
