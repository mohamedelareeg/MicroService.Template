using BuildingBlocks.Messaging;

namespace MicroService.Template.Identity.Api.Features.Roles.Commands.AssignRoleToUser;
public class AssignRoleToUserCommand : ICommand<bool>
{
    public string UserId { get; set; }
    public string RoleName { get; set; }
}
