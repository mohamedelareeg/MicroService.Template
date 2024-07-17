using BuildingBlocks.Messaging;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using BuildingBlocks.Results;

namespace MicroService.Template.ApiGateway.Features.Roles.Queries.GetUserRoles;
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
