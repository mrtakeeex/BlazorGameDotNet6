namespace BlazorGameDotNet6.Shared;

public class BattleHistoryEntry
{
    public int BattleId { get; set; }
    public DateTime BattleDate { get; set; }
    public int AttackerId { get; set; }
    public int OpponentId { get; set; }
    public bool HasWon { get; set; }
    public string AttackerName { get; set; } = null!;
    public string OpponentName { get; set; } = null!;
    public int RoundsFought { get; set; }
    public int WinnerDamageDealt { get; set; }
}
