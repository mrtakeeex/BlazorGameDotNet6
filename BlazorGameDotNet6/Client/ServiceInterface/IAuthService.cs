namespace BlazorGameDotNet6.Client.Services;

public interface IAuthService
{
    Task<ServiceResponse<string>> Login(UserLogin request);
    Task<ServiceResponse<int>> Register(UserRegister request);
}
