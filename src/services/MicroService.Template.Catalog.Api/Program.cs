using ARPSolutions.Accounting.Core.Extentions;
using BuildingBlocks.Extentions;
using Carter;
using MicroService.Template.Catalog.Api.Data;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<GlobalExceptionHandlerMiddleWare>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddCarter();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog", Version = "v1" });
    c.TagActionsBy(api => new List<string> { api.RelativePath.Split('/')[0] });
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog V1");
    });
}
app.MapCarter();
app.UseMiddleware<GlobalExceptionHandlerMiddleWare>();

app.Run();
