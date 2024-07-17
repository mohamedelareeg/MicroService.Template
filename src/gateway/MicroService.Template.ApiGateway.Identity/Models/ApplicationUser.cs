using Microsoft.AspNetCore.Identity;

namespace MicroService.Template.ApiGateway.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Code { get; set; }

}
