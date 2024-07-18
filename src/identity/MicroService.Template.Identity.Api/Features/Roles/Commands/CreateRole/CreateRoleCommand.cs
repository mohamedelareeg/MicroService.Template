using BuildingBlocks.Messaging;

namespace MicroService.Template.Identity.Api.Features.Roles.Commands.CreateRole;
public class CreateRoleCommand : ICommand<bool>
{
    public string RoleName { get; set; }
}
