using BuildingBlocks.Messaging;

namespace MicroService.Template.Identity.Api.Features.Roles.Queries.GetUserClaims;
public class GetUserClaimsQuery : IListQuery<string>
{
    public string UserId { get; set; }
}
