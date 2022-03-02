namespace BlazorGameDotNet6.Server.Controllers;

[Route(Constants.API + "/[controller]")]
[ApiController]
[Authorize]
public class UserUnitController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IUtilityService _utilityService;

    public UserUnitController(DataContext context, IUtilityService utilityService)
    {
        _context = context;
        _utilityService = utilityService;
    }

    [HttpPost(Constants.ApiRoute.Revive)]
    public async Task<IActionResult> ReviveArmy()
    {
        var user = await _utilityService.GetCurrentUser();
        var userUnits = await _context.UserUnits
            .Where(unit => unit.UserId == user.Id)
            .Include(unit => unit.Unit)
            .ToListAsync();

        // Calculate revival cost
        // Dead units revival will cost the half of their original price (because HP revives randomly)
        var coinCost = (int) Math.Round(userUnits.Where(u => u.HitPoints <= 0).Sum(u => u.Unit.CoinCost * 0.5), MidpointRounding.AwayFromZero);

        if (user.Coins < coinCost)
        {
            return BadRequest($"Not enough coins! You need {coinCost} coins to revive your army.");
        }

        var armyAlreadyAlive = true;
        foreach (var userUnit in userUnits)
        {
            if (userUnit.HitPoints <= 0)
            {
                armyAlreadyAlive = false;
                
                // revive unit with random hitpoints
                userUnit.HitPoints = new Random().Next(1, userUnit.Unit.HitPoints);
                userUnit.CurrentValue = Convert.ToInt16(Convert.ToDouble(userUnit.HitPoints) / Convert.ToDouble(userUnit.Unit.HitPoints) * userUnit.Unit.CoinCost);
            }
        }

        if (armyAlreadyAlive)
        {
            return Ok("Your army is already alive.");
        }

        user.Coins -= coinCost;

        await _context.SaveChangesAsync();

        return Ok("Army revived!");
    }

    [HttpPost]
    public async Task<IActionResult> BuildUserUnit([FromBody] int unitId)
    {
        var unit = await _context.Units.FindAsync(unitId);
        var user = await _utilityService.GetCurrentUser();
        
        if (user.Coins < unit!.CoinCost)
        {
            return BadRequest("Not enough coins!");
        }
        
        user.Coins -= unit.CoinCost;

        var newUserUnit = new UserUnit
        {
            UnitId = unit.Id,
            UserId = user.Id,
            HitPoints = unit.HitPoints,
            CurrentValue = unit.CoinCost
        };

        _context.UserUnits.Add(newUserUnit);
        await _context.SaveChangesAsync();

        return Ok(newUserUnit);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserUnits()
    {
        var user = await _utilityService.GetCurrentUser();
        var userUnits = await _context.UserUnits.Where(userUnit => userUnit.UserId == user.Id).ToListAsync();

        var response = userUnits.Select(
            userUnit => new UserUnitResponse
            {
                Id = userUnit.Id,
                UnitId = userUnit.UnitId,
                HitPoints = userUnit.HitPoints,
                CurrentValue = userUnit.CurrentValue
            });

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserUnit(int id)
    {
        var userUnit = await _context.UserUnits.FindAsync(id);

        if (userUnit == null)
        {
            return NotFound($"UserUnit with id {id} was not found");
        }

        var responseMsg = "Unit was deleted successfully!";

        if (userUnit.HitPoints > 0)
        {
            // Sell
            var user = await _context.Users.FindAsync(userUnit.UserId);
            user!.Coins += userUnit.CurrentValue;
            responseMsg = $"Unit was sold for {userUnit.CurrentValue} coins!";
        }
        
        _context.UserUnits.Remove(userUnit);
        await _context.SaveChangesAsync();

        return Ok(responseMsg);
    }
}
