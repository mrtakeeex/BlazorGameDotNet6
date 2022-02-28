namespace BlazorGameDotNet6.Client.Services;

public class BananaService : IBananaService
{
    private readonly HttpClient _client;

    public int Bananas { get; set; } = 0;

    public event Action OnChange;

    public BananaService(HttpClient client)
    {
        _client = client;
    }

    public async Task AddBananas(int amount)
    {
        var result = await _client.PutAsJsonAsync<int>("api/user/addbananas", amount);
        Bananas = await result.Content.ReadFromJsonAsync<int>();
        BananasChanged();
    }

    public void EatBananas(int amount)
    {
        Bananas -= amount;
        BananasChanged();
    }

    // notify component about banana change
    public void BananasChanged() => OnChange.Invoke();

    public async Task GetBananas()
    {
        Bananas = await _client.GetFromJsonAsync<int>("api/user/getbananas");
        BananasChanged();
    }
}
