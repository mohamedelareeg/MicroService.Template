using BuildingBlocks.Messaging;

namespace MicroService.Template.ApiGateway.Features.Roles.Queries.GetRoleClaims;
public class GetRoleClaimsQuery : IListQuery<string>
{
    public string RoleName { get; set; }
}
