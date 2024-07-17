using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using MicroService.Template.Shared.Dtos.AuthonticationDtos;

namespace MicroService.Template.ApiGateway.Features.Authentication.Commands.Register;
internal class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterDto>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<RegisterDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await _authService.Register(request.FirstName, request.LastName, request.Email, request.UserName, request.Password);
    }
}
