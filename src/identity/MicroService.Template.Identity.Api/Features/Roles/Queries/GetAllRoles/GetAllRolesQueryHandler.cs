﻿using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.Identity.Api.Services.Abstractions;

namespace MicroService.Template.Identity.Api.Features.Roles.Queries.GetAllRoles;
internal class GetAllRolesQueryHandler : IListQueryHandler<GetAllRolesQuery, string>
{
    private readonly IRoleService _roleService;

    public GetAllRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<Result<CustomList<string>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        return await _roleService.GetAllRolesAsync(cancellationToken);
    }
}