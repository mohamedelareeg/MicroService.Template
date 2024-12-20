﻿using BuildingBlocks.Messaging;

namespace MicroService.Template.Identity.Api.Features.Roles.Commands.AssignClaimToUser;
public class AssignClaimToUserCommand : ICommand<bool>
{
    public string UserId { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}
