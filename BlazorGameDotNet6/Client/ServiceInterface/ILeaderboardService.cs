namespace BlazorGameDotNet6.Client.Services;

public interface ILeaderboardService
{
    IList<UserStatistic> Leaderboard { get; set; }
    Task GetLeaderboard();
}
