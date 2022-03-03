namespace BlazorGameDotNet6.Server.Controllers;

[Route(Constants.API + "/[controller]")]
[ApiController]
[Authorize] 
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IUtilityService _utilityService;

    public UserController(DataContext context, IUtilityService utilityService)
    {
        _context = context;
        _utilityService = utilityService;
    }

    [HttpGet(Constants.ApiRoute.GetCoins)]
    public async Task<IActionResult> GetCoins()
    {
        var user = await _utilityService.GetCurrentUser();
        return Ok(user.Coins);
    }

    [HttpPut(Constants.ApiRoute.AddCoins)]
    public async Task<IActionResult> AddCoins([FromBody] int coins) 
    {
        var user = await _utilityService.GetCurrentUser();
        user.Coins += coins;

        await _context.SaveChangesAsync();
        return Ok(user.Coins);
    }

    [HttpGet(Constants.ApiRoute.Leaderboard)]
    public async Task<IActionResult> GetLeaderboard()
    {
        var users = await _context.Users.Where(user => !user.IsDeleted && 
                                                        user.IsConfirmed)
                                        .OrderByDescending(u => u.Victories)
                                        .ThenBy(u => u.Defeats)
                                        .ThenBy(u => u.DateCreated)
                                        .ToListAsync();

        var rank = 1;
        return Ok(users.Select(user => new UserStatistic
        {
            Rank = rank++,
            UserId = user.Id,
            Username = user.Username,
            Battles = user.Battles,
            Victories = user.Victories,
            Defeats = user.Defeats
        }));    
    }
}
