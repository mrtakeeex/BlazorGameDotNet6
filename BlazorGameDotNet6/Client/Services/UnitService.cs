namespace BlazorGameDotNet6.Client.Services;

public class UnitService : IUnitService
{
    private readonly IToastService _toastService;
    private readonly HttpClient _client;
    private readonly ICoinService _coinService;

    public UnitService(IToastService toastService, HttpClient client, ICoinService coinService)
    {
        _toastService = toastService;
        _client = client;
        _coinService = coinService;
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
            await _coinService.GetCoins();
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

    public async Task ReviveArmy()
    {
        var result = await _client.PostAsJsonAsync<string>("api/userunit/revive", null);
        
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            _toastService.ShowSuccess(await result.Content.ReadAsStringAsync());
        }
        else
        {
            _toastService.ShowError(await result.Content.ReadAsStringAsync());
        }

        await LoadUserUnitsAsync();
        await _coinService.GetCoins();
    }

    public async Task DeleteUnit(int userUnitId)
    {
        var result = await _client.DeleteAsync($"api/userunit/{userUnitId}");
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            _toastService.ShowSuccess(await result.Content.ReadAsStringAsync());
        }
        else
        {
            _toastService.ShowError(await result.Content.ReadAsStringAsync());
        }

        await LoadUserUnitsAsync();
        await _coinService.GetCoins();
    }
}
