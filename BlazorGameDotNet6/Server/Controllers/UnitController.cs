namespace BlazorGameDotNet6.Server.Controllers;

[Route(Constants.API + "/[controller]")]
[ApiController]
public class UnitController : ControllerBase
{
    private readonly DataContext _context;
    public UnitController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUnits() 
    {
        var units = await _context.Units.ToListAsync();

        return Ok(units);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUnit(Unit unit)
    {
        var dbUnit = await _context.Units.FindAsync(unit.Id);
        if (dbUnit == null)
        {
            return NotFound($"Unit with the given Id {unit.Id} doesn't exist.");
        }

        dbUnit.Title = unit.Title;
        dbUnit.Attack = unit.Attack;
        dbUnit.Defense = unit.Defense;
        dbUnit.CoinCost = unit.CoinCost;
        dbUnit.HitPoints = unit.HitPoints;

        await _context.SaveChangesAsync();

        return Ok(dbUnit);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUnit(int id)
    {
        var dbUnit = await _context.Units.FindAsync(id);
        if (dbUnit == null)
        {
            return NotFound($"Unit with the given Id {id} doesn't exist.");
        }

        _context.Units.Remove(dbUnit);
        await _context.SaveChangesAsync();

        return Ok(await _context.Units.ToListAsync());
    }

}
