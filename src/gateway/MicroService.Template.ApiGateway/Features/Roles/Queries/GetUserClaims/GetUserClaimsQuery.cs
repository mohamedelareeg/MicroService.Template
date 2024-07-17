using BuildingBlocks.Messaging;

namespace MicroService.Template.ApiGateway.Features.Roles.Queries.GetUserClaims;
public class GetUserClaimsQuery : IListQuery<string>
{
    public string UserId { get; set; }
}
