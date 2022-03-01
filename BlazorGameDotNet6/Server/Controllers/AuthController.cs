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
                        Bananas = request.Bananas,
                        DateOfBirth = request.DateOfBirth,
                        isConfirmed = request.IsConfirmed,
                        Bio = request.Bio
                    },
                    request.Password,
                    short.Parse(request.StartUnitId));

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}

//public class ModelStateFeatureFilter : IAsyncActionFilter
//{

//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        var state = context.ModelState;
//        context.HttpContext.Features.Set<ModelStateFeature>(new ModelStateFeature(state));
//        await next();
//    }
//}

//public class ModelStateFeature
//{
//    public ModelStateDictionary ModelState { get; set; }

//    public ModelStateFeature(ModelStateDictionary state)
//    {
//        ModelState = state;
//    }
//}
