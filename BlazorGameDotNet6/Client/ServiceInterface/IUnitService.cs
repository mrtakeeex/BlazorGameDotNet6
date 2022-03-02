namespace BlazorGameDotNet6.Client.Services;

public interface IUnitService
{
    IList<Unit> Units { get; set; }
    IList<UserUnit> MyUnits { get; set; }
    Task AddUnit(int unitId);
    Task LoadUnitsAsync();
    Task LoadUserUnitsAsync();
    Task ReviveArmy();
    Task DeleteUnit(int userUnitId);
}
