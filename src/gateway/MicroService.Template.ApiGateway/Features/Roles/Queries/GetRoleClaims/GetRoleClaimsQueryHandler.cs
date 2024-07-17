using BuildingBlocks.Messaging;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using BuildingBlocks.Results;

namespace MicroService.Template.ApiGateway.Features.Roles.Queries.GetRoleClaims;
internal class GetRoleClaimsQueryHandler : IListQueryHandler<GetRoleClaimsQuery, string>
{
    private readonly IRoleService _roleService;

    public GetRoleClaimsQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<Result<CustomList<string>>> Handle(GetRoleClaimsQuery request, CancellationToken cancellationToken)
    {
        return await _roleService.GetRoleClaimsAsync(request.RoleName, cancellationToken);
    }
}
