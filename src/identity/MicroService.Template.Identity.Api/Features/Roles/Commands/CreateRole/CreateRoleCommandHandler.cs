using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Roles.Commands.CreateRole;
internal class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, bool>
{
    private readonly IRoleService _roleService;

    public CreateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public Task<Result<bool>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return _roleService.CreateRoleAsync(request.RoleName, cancellationToken);
    }
}
