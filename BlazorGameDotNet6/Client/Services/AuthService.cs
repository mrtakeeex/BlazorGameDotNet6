namespace BlazorGameDotNet6.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _client;

    public AuthService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ServiceResponse<string>> Login(UserLogin request)
    {
        var result = await _client.PostAsJsonAsync("api/auth/login", request);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
    }

    public async Task<ServiceResponse<int>> Register(UserRegister request)
    {
        var result = await _client.PostAsJsonAsync("api/auth/register", request);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
    }
}
