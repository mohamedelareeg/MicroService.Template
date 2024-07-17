using Microsoft.AspNetCore.Authorization;

namespace MicroService.Template.ApiGateway.Identity.Filters;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; private set; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}
