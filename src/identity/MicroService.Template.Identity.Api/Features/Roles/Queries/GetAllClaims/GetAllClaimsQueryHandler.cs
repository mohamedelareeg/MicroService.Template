using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Roles.Queries.GetAllClaims;
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
