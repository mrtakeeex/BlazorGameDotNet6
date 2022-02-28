namespace BlazorGameDotNet6.Server.Services;

public class UtilityService : IUtilityService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UtilityService(DataContext context, IHttpContextAccessor httpContextAccesor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccesor;
    }
    public async Task<User> GetCurrentUser()
    {
        var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _context.Users.FindAsync(userId);

        return user;
    }
}
