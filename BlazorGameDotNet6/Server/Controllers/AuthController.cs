namespace BlazorGameDotNet6.Server.Controllers;

[Route(Constants.API + "/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepo;

    public AuthController(IAuthRepository authRepo) => _authRepo = authRepo;

    [HttpPost(Constants.ApiRoute.Login)]
    public async Task<IActionResult> Login(UserLogin request)
    {
        var response = await _authRepo.Login(request.Email!, request.Password!);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost(Constants.ApiRoute.Register)]
    public async Task<IActionResult> Register(UserRegister request)
    {
        var response = await _authRepo.Register(
                    new User
                    {
                        Username = request.Username!,
                        Email = request.Email!,
                        Coins = Constants.StartingCoin, 
                        DateOfBirth = request.DateOfBirth,
                        IsConfirmed = request.IsConfirmed,
                        Bio = request.Bio
                    },
                    request.Password!,
                    request.StartUnitId);

        return response.Success ? Ok(response) : BadRequest(response);
    }
}
