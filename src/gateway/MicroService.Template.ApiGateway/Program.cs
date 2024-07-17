using MicroService.Template.ApiGateway;
using MicroService.Template.ApiGateway.Identity;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCors();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    // MediatR Pipelines
    // cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPiplineBehavior<,>));
    // cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
});
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddHealthChecks();

builder.Services.AddIdentityDependencies(builder.Configuration);
// Configure authorization policy
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("authenticated", policy =>
//        policy.RequireAuthenticatedUser());
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}
app.UseHttpsRedirection();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});
app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        await next();

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {

            await next();
        }
    });
    proxyPipeline.UseSessionAffinity();
    proxyPipeline.UseLoadBalancing();
    proxyPipeline.UsePassiveHealthChecks();
});


app.MapHealthChecks("health");

app.MapControllers();

app.Run();
