using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Roles.Commands.EditRole;
internal class EditRoleCommandHandler : ICommandHandler<EditRoleCommand, bool>
{
    private readonly IRoleService _roleService;

    public EditRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<Result<bool>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        return await _roleService.EditRoleAsync(request.RoleName, request.NewRoleName, cancellationToken);
    }
}
