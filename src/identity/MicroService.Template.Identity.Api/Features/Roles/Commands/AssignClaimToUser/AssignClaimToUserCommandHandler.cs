using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Roles.Commands.AssignClaimToUser;
internal class AssignClaimToUserCommandHandler : ICommandHandler<AssignClaimToUserCommand, bool>
{
    private readonly IRoleService _roleService;

    public AssignClaimToUserCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<Result<bool>> Handle(AssignClaimToUserCommand request, CancellationToken cancellationToken)
    {
        return await _roleService.AddClaimToUserAsync(request.UserId, request.ClaimType, request.ClaimValue, cancellationToken);
    }
}
