namespace BlazorGameDotNet6.Client.Services;

public class CoinService : ICoinService
{
    private readonly HttpClient _client;

    public int Coins { get; set; } = 0;

    public event Action OnChange;

    public CoinService(HttpClient client)
    {
        _client = client;
    }

    public async Task AddCoins(int amount)
    {
        var result = await _client.PutAsJsonAsync<int>("api/user/addcoins", amount);
        Coins = await result.Content.ReadFromJsonAsync<int>();
        CoinsAmountChanged();
    }

    public void SpendCoins(int amount)
    {
        Coins -= amount;
        CoinsAmountChanged();
    }

    // notify component about coin change
    public void CoinsAmountChanged() => OnChange.Invoke();

    public async Task GetCoins()
    {
        Coins = await _client.GetFromJsonAsync<int>("api/user/getcoins");
        CoinsAmountChanged();
    }
}
