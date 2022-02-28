namespace BlazorGameDotNet6.Client.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly HttpClient _client;

    public IList<UserStatistic> Leaderboard { get; set; }

    public LeaderboardService(HttpClient client)
    {
        _client = client;
    }
    public async Task GetLeaderboard()
    {
        Leaderboard = await _client.GetFromJsonAsync<IList<UserStatistic>>("api/user/leaderboard");
    }
}
