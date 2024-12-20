﻿using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Roles.Commands.DeleteRole;
internal class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, bool>
{
    private readonly IRoleService _roleService;

    public DeleteRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public Task<Result<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        return _roleService.DeleteRoleAsync(request.RoleName, cancellationToken);
    }
}
