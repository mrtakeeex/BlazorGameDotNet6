namespace BlazorGameDotNet6.Client;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    public ILocalStorageService _localStorageService;
    private readonly HttpClient _client;
    private readonly ICoinService _coinService;

    public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient client, ICoinService coinService)
    {
        _localStorageService = localStorageService;
        _client = client;
        _coinService = coinService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string authToken = await _localStorageService.GetItemAsStringAsync("authToken");

        var identity = new ClaimsIdentity();
        _client.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(authToken))
        {
            try
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                // token will be sent to server with every request -> server will know if a valid user asks for the end points
                // "Bearer" - general name for that token
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));

                await _coinService.GetCoins();
            }
            // if the authentication is not valid anymore (e.g. timeout), set up a new one
            catch (Exception ex)
            {
                await _localStorageService.RemoveItemAsync("authToken");
                identity = new ClaimsIdentity();
            }
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

        return claims;
    }
}
