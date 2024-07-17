using BuildingBlocks.Messaging;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using BuildingBlocks.Results;

namespace MicroService.Template.ApiGateway.Features.Roles.Commands.AddClaimToRole;
internal class AddClaimToRoleCommandHandler : ICommandHandler<AddClaimToRoleCommand, bool>
{
    private readonly IRoleService _roleService;

    public AddClaimToRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<Result<bool>> Handle(AddClaimToRoleCommand request, CancellationToken cancellationToken)
    {
        return await _roleService.AddClaimToRoleAsync(request.RoleName, request.ClaimType, request.ClaimValue, cancellationToken);
    }
}
