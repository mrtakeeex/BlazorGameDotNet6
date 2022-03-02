namespace BlazorGameDotNet6.Server.Data;

public interface IAuthRepository
{
    Task<ServiceResponse<int>> Register(User user, string password, int startUnitId);
    Task<ServiceResponse<string>> Login(string email, string password);
    Task<bool> EmailExists(string email);
    Task<bool> UsernameExists(string username);
}
