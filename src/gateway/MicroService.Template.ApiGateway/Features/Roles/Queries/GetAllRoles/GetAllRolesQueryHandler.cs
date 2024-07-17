using BuildingBlocks.Messaging;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using BuildingBlocks.Results;

namespace MicroService.Template.ApiGateway.Features.Roles.Queries.GetAllRoles;
internal class GetAllRolesQueryHandler : IListQueryHandler<GetAllRolesQuery, string>
{
    private readonly IRoleService _roleService;

    public GetAllRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<Result<CustomList<string>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        return await _roleService.GetAllRolesAsync(cancellationToken);
    }
}
