﻿namespace BlazorGameDotNet6.Server.Controllers;

[Route(Constants.API + "/[controller]")]
[ApiController]
[Authorize]
public class BattleController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IUtilityService _utilityService;

    public BattleController(DataContext context, IUtilityService utilityService)
    {
        _context = context;
        _utilityService = utilityService;
    }

    [HttpGet(Constants.ApiRoute.History)]
    public async Task<IActionResult> GetBattleHistory()
    {
        var user = await _utilityService.GetCurrentUser();
        var battles = await _context.Battles
            .Where(battle => battle.AttackerId == user.Id || battle.OpponentId == user.Id)
            // include user records for attacker, opponent and winner
            .Include(battle => battle.Attacker)
            .Include(battle => battle.Opponent)
            .Include(battle => battle.Winner)
            .ToListAsync();

        var history = battles.Select(battle => new BattleHistoryEntry
        {
            BattleId = battle.Id,
            AttackerId = battle.AttackerId,
            OpponentId = battle.OpponentId,
            HasWon = battle.WinnerId == user.Id, // if the winner is the current user
            AttackerName = battle.Attacker.Username,
            OpponentName = battle.Opponent.Username,
            RoundsFought = battle.RoundsFought,
            WinnerDamageDealt = battle.WinnerDamage,
            BattleDate = battle.BattleDate,
        });

        return Ok(history.OrderByDescending(h => h.BattleDate));
    }

    [HttpPost]
    public async Task<IActionResult> StartBattle([FromBody] int opponentId)
    {
        //attack ID comes from the current authenticated user
        var attacker = await _utilityService.GetCurrentUser();
        var opponent = await _context.Users.FindAsync(opponentId);
        if (opponent == null || opponent.IsDeleted)
        {
            return NotFound("Opponent not available.");
        }

        var result = new BattleResult();

        await Fight(attacker, opponent, result);

        return Ok(result);
    }

    private async Task Fight(User attacker, User opponent, BattleResult result)
    {
        var attackerArmy = await _context.UserUnits
            .Where(u => u.UserId == attacker.Id && u.HitPoints > 0)
            .Include(u => u.Unit) // get the actual Unit records
            .ToListAsync();
        var opponentArmy = await _context.UserUnits
            .Where(u => u.UserId == opponent.Id && u.HitPoints > 0)
            .Include(u => u.Unit) // get the actual Unit records
            .ToListAsync();

        var attackerDamageSum = 0;
        var opponentDamageSum = 0;
        var currentRound = 0;

        while (attackerArmy.Count > 0 && opponentArmy.Count > 0) 
        { 
            currentRound++;

            // they attack in each round respectively
            attackerDamageSum += currentRound % 2 != 0 ? FightRound(attacker, opponent, attackerArmy, opponentArmy, result) : FightRound(opponent, attacker, opponentArmy, attackerArmy, result);
        }

        result.IsVictory = opponentArmy.Count == 0;
        result.RoundsFought = currentRound;

        //user statistics update
        if (result.RoundsFought > 0)
        {
            await FinishFight(attacker, opponent, result, attackerDamageSum, opponentDamageSum);
        }
    }

    private int FightRound(User opponent, User attacker, List<UserUnit> opponentArmy, List<UserUnit> attackerArmy, BattleResult result)
    {
        int randomAttackerIndex = new Random().Next(attackerArmy.Count);
        int randomOpponentIndex = new Random().Next(opponentArmy.Count);

        var randomAttacker = attackerArmy[randomAttackerIndex];
        var randomOpponent = opponentArmy[randomOpponentIndex];

        // random value from attacker and defender unit 
        var damage = Math.Abs(new Random().Next(1, randomAttacker.Unit.Attack) - new Random().Next(1, randomOpponent.Unit.Defense));

        if (damage <= randomOpponent.HitPoints)
        {
            randomOpponent.HitPoints -= damage;
            result.Log.Add(
                $"{attacker.Username}'s {randomAttacker.Unit.Title} attacks " +
                $"{opponent.Username}'s {randomOpponent.Unit.Title} with {damage} damage.");

            return damage;
        }
        // if the damage is greater, than the Hitpoints -> unit has been killed
        else
        {
            damage = randomOpponent.HitPoints;
            randomOpponent.HitPoints = 0;
            opponentArmy.Remove(randomOpponent);

            result.Log.Add(
                $"{attacker.Username}'s {randomAttacker.Unit.Title} kills " +
                $"{opponent.Username}'s {randomOpponent.Unit.Title}!");

            return damage;
        }
    }

    private async Task FinishFight(User attacker, User opponent, BattleResult result, int attackerDamageSum, int opponentDamageSum)
    {
        result.AttackerDamageSum = attackerDamageSum;
        result.OpponentDamageSum = opponentDamageSum;

        attacker.Battles++;
        opponent.Battles++;

        if (result.IsVictory)
        {
            attacker.Victories++;
            opponent.Defeats++;
            
            attacker.Coins += opponentDamageSum;
            opponent.Coins += attackerDamageSum * 10; // for rebuilding the army and strike back
        }
        else
        {
            attacker.Defeats++;
            opponent.Victories++;

            attacker.Coins += opponentDamageSum * 10;
            opponent.Coins += attackerDamageSum;
        }

        StoreBattleHistory(attacker, opponent, result);

        await UpdateUserUnitCurrentValues(attacker);
        await UpdateUserUnitCurrentValues(opponent);

        await _context.SaveChangesAsync();
    }

    private void StoreBattleHistory(User attacker, User opponent, BattleResult result)
    {
        _context.Battles.Add(new Battle
        {
            Attacker = attacker,
            Opponent = opponent,
            RoundsFought = result.RoundsFought,
            WinnerDamage = result.IsVictory ? result.AttackerDamageSum : result.OpponentDamageSum,
            Winner = result.IsVictory ? attacker : opponent
        });
    }

    private async Task UpdateUserUnitCurrentValues(User user)
    {
        await _context.UserUnits.Where(u => u.UserId == user.Id).ForEachAsync(async u => u.CurrentValue = await GetCurrentValue(u));

        // local method
        async Task<int> GetCurrentValue(UserUnit userUnitId)
        {
            var unit = await _context.Units.FindAsync(userUnitId.UnitId);
            return Convert.ToInt16(Convert.ToDouble(userUnitId.HitPoints) / Convert.ToDouble(unit!.HitPoints) * unit.CoinCost);
        }
    }
}
