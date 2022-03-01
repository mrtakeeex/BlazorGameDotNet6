namespace BlazorGameDotNet6.Client.Services;

public interface IBattleService
{
    BattleResult LastBattle { get; set; }
    IList<BattleHistoryEntry> History { get; set; }
    Task<BattleResult> StartBattle(int opponentId);
    Task GetHistory();
}
