namespace BlazorGameDotNet6.Client.Services;

public class UnitService : IUnitService
{
    private readonly IToastService _toastService;
    private readonly HttpClient _client;
    private readonly IBananaService _bananaService;

    public UnitService(IToastService toastService, HttpClient client, IBananaService bananaService)
    {
        _toastService = toastService;
        _client = client;
        _bananaService = bananaService;
    }

    public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>();

    public IList<Unit> Units { get; set; } = new List<Unit>();

    public async Task AddUnit(int unitId)
    {
        var unit = Units.First(unit => unit.Id == unitId);
        var result = await _client.PostAsJsonAsync<int>("api/userunit", unitId);
        
        if (result.StatusCode != System.Net.HttpStatusCode.OK)
        {
            _toastService.ShowError(await result.Content.ReadAsStringAsync());
        }
        else
        {
            await _bananaService.GetBananas();
            _toastService.ShowSuccess($"Your {unit.Title} has been built!", "Unit built!");
        }
    }

    public async Task LoadUnitsAsync()
    {
        if (Units.Count == 0)
        {
            Units = await _client.GetFromJsonAsync<IList<Unit>>("api/unit");
        }
    }

    public async Task LoadUserUnitsAsync()
    {
        MyUnits = await _client.GetFromJsonAsync<IList<UserUnit>>("api/userunit");
    }
}
