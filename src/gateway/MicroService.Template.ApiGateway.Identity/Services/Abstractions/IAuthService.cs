using BuildingBlocks.Results;
using MicroService.Template.Shared.Dtos.AuthonticationDtos;

namespace MicroService.Template.ApiGateway.Identity.Services.Abstractions;

public interface IAuthService
{
    Task<Result<LoginDto>> Login(string email, string password);
    Task<Result<RegisterDto>> Register(string firstName, string lastName, string email, string userName, string password);
    Task<Result<bool>> SendResetPasswordCode(string email);
    Task<Result<bool>> ConfirmAndResetPassword(string code, string email, string newPassword);
}
