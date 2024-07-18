using BuildingBlocks.Messaging;

namespace MicroService.Template.Identity.Api.Features.Roles.Queries.GetUserRoles;
public class GetUserRolesQuery : IListQuery<string>
{
    public string UserId { get; set; }
}
