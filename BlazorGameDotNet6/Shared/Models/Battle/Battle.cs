namespace BlazorGameDotNet6.Shared;

public class Battle
{
    public int Id { get; set; }
    public User Attacker { get; set; } = null!;
    public int AttackerId { get; set; }
    public User Opponent { get; set; } = null!;
    public int OpponentId { get; set; }
    public User Winner { get; set; } = null!;
    public int WinnerId { get; set; }
    public int WinnerDamage { get; set; }
    public int RoundsFought { get; set; }
    public DateTime BattleDate { get; set; } = DateTime.Now;
}
