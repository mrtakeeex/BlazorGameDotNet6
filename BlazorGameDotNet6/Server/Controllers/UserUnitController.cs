namespace BlazorGameDotNet6.Server.Controllers;

[Route("api/[controller]")]
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

    [HttpPost("revive")]
    public async Task<IActionResult> ReviveArmy()
    {
        var user = await _utilityService.GetCurrentUser();
        var userUnits = await _context.UserUnits
            .Where(unit => unit.UserId == user.Id)
            .Include(unit => unit.Unit)
            .ToListAsync();

        var bananaCost = 1000;

        if (user.Bananas < bananaCost)
        {
            return BadRequest($"Not enough bananas! You need {bananaCost} bananas to revive your army.");
        }

        var armyAlreadyAlive = true;
        foreach (var userUnit in userUnits)
        {
            if (userUnit.HitPoints <= 0)
            {
                armyAlreadyAlive = false;
                
                // revive unit with random hitpoints
                userUnit.HitPoints = new Random().Next(0, userUnit.HitPoints); 
            }
        }

        if (armyAlreadyAlive)
        {
            return Ok("Your army is already alive.");
        }

        user.Bananas -= bananaCost;

        await _context.SaveChangesAsync();

        return Ok("Army revived!");
    }

    [HttpPost]
    public async Task<IActionResult> BuildUserUnit([FromBody] int unitId)
    {
        var unit = await _context.Units.FindAsync(unitId);
        var user = await _utilityService.GetCurrentUser();
        
        if (user.Bananas < unit!.BananaCost)
        {
            return BadRequest("Not enough bananas!");
        }
        
        user.Bananas -= unit.BananaCost;

        var newUserUnit = new UserUnit
        {
            UnitId = unit.Id,
            UserId = user.Id,
            HitPoints = unit.HitPoints
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
                UnitId = userUnit.UnitId,
                HitPoints = userUnit.HitPoints
            });

        return Ok(response);
    }
}
