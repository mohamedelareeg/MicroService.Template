using BuildingBlocks.Messaging;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using BuildingBlocks.Results;

namespace MicroService.Template.ApiGateway.Features.Roles.Commands.CreateRole;
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
