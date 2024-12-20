﻿using BuildingBlocks.Results;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroService.Template.Identity.Api.Controllers.Base;
using MicroService.Template.Identity.Api.Features.Roles.Commands.AddClaimToRole;
using MicroService.Template.Identity.Api.Features.Roles.Commands.AssignClaimToUser;
using MicroService.Template.Identity.Api.Features.Roles.Commands.AssignRoleToUser;
using MicroService.Template.Identity.Api.Features.Roles.Commands.CreateRole;
using MicroService.Template.Identity.Api.Features.Roles.Commands.DeleteRole;
using MicroService.Template.Identity.Api.Features.Roles.Commands.EditRole;
using MicroService.Template.Identity.Api.Features.Roles.Queries.GetRoleClaims;
using MicroService.Template.Identity.Api.Features.Roles.Queries.GetUserRoles;
using MicroService.Template.Identity.Api.Features.Roles.Queries.GetUserClaims;
using MicroService.Template.Identity.Api.Features.Roles.Queries.GetAllRoles;
using MicroService.Template.Identity.Api.Features.Roles.Queries.GetAllClaims;

namespace MicroService.Template.Identity.Api.Controllers;
[Route("roles")]
public class RolesController : AppControllerBase
{
    public RolesController(ISender sender)
        : base(sender)
    {
    }

    [HttpPost("createRole")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }

    [HttpPost("editRole")]
    public async Task<IActionResult> EditRole([FromBody] EditRoleCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }

    [HttpPost("deleteRole")]
    public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }

    [HttpGet("getRoleClaims")]
    public async Task<IActionResult> GetRoleClaims(string roleName, CancellationToken cancellationToken)
    {
        var query = new GetRoleClaimsQuery { RoleName = roleName };
        Result<CustomList<string>> result = await Sender.Send(query, cancellationToken);
        return CustomResult(result);
    }

    [HttpPost("addClaimToRole")]
    public async Task<IActionResult> AddClaimToRole([FromBody] AddClaimToRoleCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }
    [HttpPost("assignRoleToUser")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }

    [HttpPost("assignClaimToUser")]
    public async Task<IActionResult> AssignClaimToUser([FromBody] AssignClaimToUserCommand request, CancellationToken cancellationToken)
    {
        Result<bool> result = await Sender.Send(request, cancellationToken);
        return CustomResult(result);
    }

    [HttpGet("getUserRoles")]
    public async Task<IActionResult> GetUserRoles(string userId, CancellationToken cancellationToken)
    {
        var query = new GetUserRolesQuery { UserId = userId };
        Result<CustomList<string>> result = await Sender.Send(query, cancellationToken);
        return CustomResult(result);
    }

    [HttpGet("getUserClaims")]
    public async Task<IActionResult> GetUserClaims(string userId, CancellationToken cancellationToken)
    {
        var query = new GetUserClaimsQuery { UserId = userId };
        Result<CustomList<string>> result = await Sender.Send(query, cancellationToken);
        return CustomResult(result);
    }
    [Authorize(Roles = "Administrator")]
    [HttpGet("getAllRoles")]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var query = new GetAllRolesQuery();
        Result<CustomList<string>> result = await Sender.Send(query, cancellationToken);
        return CustomResult(result);
    }
    [HttpGet("getAllClaims")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllClaims(CancellationToken cancellationToken)
    {
        var query = new GetAllClaimsQuery();
        Result<CustomList<string>> result = await Sender.Send(query, cancellationToken);
        return CustomResult(result);
    }
}
