using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Roles.Commands.AssignRoleToUser;
internal class AssignRoleToUserCommandHandler : ICommandHandler<AssignRoleToUserCommand, bool>
{
    private readonly IRoleService _roleService;

    public AssignRoleToUserCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<Result<bool>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        return await _roleService.AddRoleToUserAsync(request.UserId, request.RoleName, cancellationToken);
    }
}
