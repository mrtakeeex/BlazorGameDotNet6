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
        var result = await _client.PostAsJsonAsync<int>(Constants.ApiEndpointPath.UserUnitController_Post, unitId);
        
        if (result.StatusCode != System.Net.HttpStatusCode.OK)
        {
            _toastService.ShowError(await result.Content.ReadAsStringAsync());
        }
        else
        {
            await _coinService.GetCoins();
            _toastService.ShowSuccess($"Your {Units.Single(unit => unit.Id == unitId).Title} has been built!", "Unit built!");
        }
    }

    public async Task LoadUnitsAsync()
    {
        if (!Units.Any())
        {
            Units = await _client.GetFromJsonAsync<IList<Unit>>(Constants.ApiEndpointPath.UnitController_Get);
        }
    }

    public async Task LoadUserUnitsAsync()
    {
        MyUnits = await _client.GetFromJsonAsync<IList<UserUnit>>(Constants.ApiEndpointPath.UserUnitController_Get);
    }

    public async Task ReviveArmy()
    {
        var result = await _client.PostAsJsonAsync<string>(Constants.ApiEndpointPath.UserUnitController_Post_Revive, null);
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
        var result = await _client.DeleteAsync(string.Format(Constants.ApiEndpointPath.UserUnitController_Delete, userUnitId));
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
