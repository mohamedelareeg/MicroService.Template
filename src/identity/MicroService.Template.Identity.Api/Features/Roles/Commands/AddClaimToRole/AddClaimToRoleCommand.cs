using BuildingBlocks.Messaging;

namespace MicroService.Template.Identity.Api.Features.Roles.Commands.AddClaimToRole;
public class AddClaimToRoleCommand : ICommand<bool>
{
    public string RoleName { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}
