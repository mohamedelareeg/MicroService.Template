using MicroService.Template.ApiGateway.Identity.Data;
using MicroService.Template.ApiGateway.Identity.Models;
using MicroService.Template.ApiGateway.Identity.Services;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MicroService.Template.ApiGateway.Identity;
public static class IdentityDependencies
{
    public static IServiceCollection AddIdentityDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
              b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)),
              ServiceLifetime.Scoped);

        //MediatR
        //services.AddMediatR(cfg =>
        //{
        //    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        //    //MediatR PopleLines
        //    //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPiplineBehavior<,>));
        //    //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        //});

        //Fluent Validation
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
        #region Services
        services.AddScoped<IEmailsService, EmailsService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUser, CurrentUser>();
        services.AddScoped<IRoleService, RoleService>();
        #endregion
        #region Authontication & JWT
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
         {
             o.RequireHttpsMetadata = false;
             o.SaveToken = true;
             o.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidIssuer = configuration["JwtSettings:Issuer"],
                 ValidAudience = configuration["JwtSettings:Audience"],
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
             };
         });
        //services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        //services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        //services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.Zero);
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
