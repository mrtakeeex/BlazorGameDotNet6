namespace BlazorGameDotNet6.Client.Services;

public interface ICoinService
{
    event Action OnChange;
    int Coins { get; set; }
    void SpendCoins(int amount);
    Task AddCoins(int amount);

    Task GetCoins();
}
