using BuildingBlocks.Messaging;

namespace MicroService.Template.Identity.Api.Features.Roles.Queries.GetRoleClaims;
public class GetRoleClaimsQuery : IListQuery<string>
{
    public string RoleName { get; set; }
}
