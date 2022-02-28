namespace BlazorGameDotNet6.Server.Services;

public interface IUtilityService
{
    Task<User> GetCurrentUser();
}
