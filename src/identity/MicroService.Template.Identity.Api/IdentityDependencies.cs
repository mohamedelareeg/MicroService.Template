using BuildingBlocks.Behaviors;
using FluentValidation;
using MediatR;
using MicroService.Template.Identity.Api.Data;
using MicroService.Template.Identity.Filters;
using MicroService.Template.Identity.Models;
using MicroService.Template.Identity.Api.Services;
using MicroService.Template.Identity.Api.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using MicroService.Template.Identity.Api.Models;

namespace MicroService.Template.Identity.Api;
public static class IdentityDependencies
{
    public static IServiceCollection AddIdentityDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
              b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)),
              ServiceLifetime.Scoped);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //MediatR PopleLines
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPiplineBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
        #region Services
        services.AddScoped<IEmailsService, EmailsService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUser, CurrentUser>();
        services.AddScoped<IRoleService, RoleService>();
        #endregion
        #region Email
        var emailSettings = new EmailSettings();
        configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);

        services.AddSingleton(emailSettings);
        #endregion
        #region DefaultIdentityOptions
        IConfigurationSection iConfigurationSection = configuration.GetSection("IdentityDefaultOptions");
        services.Configure<DefaultIdentityOptions>(iConfigurationSection);
        DefaultIdentityOptions? defaultIdentityOptions = iConfigurationSection.Get<DefaultIdentityOptions>();
        AddIdentityOptions.SetOptions(services, defaultIdentityOptions: defaultIdentityOptions);
        #endregion

        return services;
    }
}
