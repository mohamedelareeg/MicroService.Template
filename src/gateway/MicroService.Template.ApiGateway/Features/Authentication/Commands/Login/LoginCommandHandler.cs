using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using MicroService.Template.ApiGateway.Identity.Services.Abstractions;
using MicroService.Template.Shared.Dtos.AuthonticationDtos;

namespace MicroService.Template.ApiGateway.Features.Authentication.Commands.Login;
internal class LoginCommandHandler : ICommandHandler<LoginCommand, LoginDto>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authService.Login(request.Email, request.Password);
    }
}
