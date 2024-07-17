using BuildingBlocks.Messaging;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using BuildingBlocks.Results;

namespace MicroService.Template.ApiGateway.Features.Roles.Queries.GetAllClaims;
internal class GetAllClaimsQueryHandler : IListQueryHandler<GetAllClaimsQuery, string>
{
    private readonly IRoleService _roleService;

    public GetAllClaimsQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<Result<CustomList<string>>> Handle(GetAllClaimsQuery request, CancellationToken cancellationToken)
    {
        return await _roleService.GetAllClaimsAsync(cancellationToken);
    }
}
