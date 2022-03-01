namespace BlazorGameDotNet6.Client.Services;

public class BattleService : IBattleService
{
    private readonly HttpClient _client;
    public BattleResult LastBattle { get; set; } = new BattleResult();
    public IList<BattleHistoryEntry> History { get; set; } = new List<BattleHistoryEntry>();

    public BattleService(HttpClient client)
    {
        _client = client;
    }

    public async Task<BattleResult> StartBattle(int opponentId)
    {
        var result = await _client.PostAsJsonAsync("api/battle", opponentId);
        LastBattle = await result.Content.ReadFromJsonAsync<BattleResult>();
        return LastBattle;
    }

    public async Task GetHistory()
    {
        History = await _client.GetFromJsonAsync<IList<BattleHistoryEntry>>("api/battle/history");
    }
}
