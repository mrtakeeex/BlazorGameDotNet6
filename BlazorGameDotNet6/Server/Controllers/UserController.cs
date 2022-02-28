namespace BlazorGameDotNet6.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // only authorized users can access 
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IUtilityService _utilityService;

    public UserController(DataContext context, IUtilityService utilityService)
    {
        _context = context;
        _utilityService = utilityService;
    }

    [HttpGet("getbananas")]
    public async Task<IActionResult> GetBananas()
    {
        // find current authorized user details
        var user = await _utilityService.GetCurrentUser();

        return Ok(user!.Bananas);
    }

    [HttpPut("addbananas")]
    public async Task<IActionResult> AddBananas([FromBody] int bananas) // because its integer and not a complex type
    {
        var user = await _utilityService.GetCurrentUser();
        user.Bananas += bananas;

        await _context.SaveChangesAsync();
        return Ok(user.Bananas);
    }

    //private int GetCurrentUserid() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    //private async Task<User> GetCurrentUser() => await _context.Users.FindAsync(GetCurrentUserid());

    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard()
    {
        var users = await _context.Users.Where(user => !user.isDeleted && 
                                                        user.isConfirmed)
                                        .OrderByDescending(u => u.Victories)
                                        .ThenBy(u => u.Defeats)
                                        .ThenBy(u => u.DateCreated)
                                        .ToListAsync();

        var rank = 1;
        var response = users.Select(user => new UserStatistic
        {
            Rank = rank++,
            UserId = user.Id,
            Username = user.Username,
            Battles = user.Battles,
            Victories = user.Victories,
            Defeats = user.Defeats
        });

        return Ok(response);    
    }
}
