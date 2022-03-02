namespace BlazorGameDotNet6.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepo;

    public AuthController(IAuthRepository authRepo)
    {
        _authRepo = authRepo;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLogin request)
    {
        var response = await _authRepo.Login(request.Email, request.Password);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegister request)
    {
        var response = await _authRepo.Register(
                    new User
                    {
                        Username = request.Username,
                        Email = request.Email,
                        // starting amount
                        Coins = Constants.StartingCoin, 
                        DateOfBirth = request.DateOfBirth,
                        isConfirmed = request.IsConfirmed,
                        Bio = request.Bio
                    },
                    request.Password,
                    request.StartUnitId);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}
