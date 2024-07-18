using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Roles.Queries.GetUserRoles;
internal class GetUserRolesQueryHandler : IListQueryHandler<GetUserRolesQuery, string>
{
    private readonly IRoleService _roleService;

    public GetUserRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public Task<Result<CustomList<string>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        return _roleService.GetUserRolesAsync(request.UserId, cancellationToken);
    }
}
