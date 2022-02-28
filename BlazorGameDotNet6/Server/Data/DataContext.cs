namespace BlazorGameDotNet6.Server.Data;

public class DataContext : DbContext
{
    public DbSet<Unit> Units { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserUnit> UserUnits { get; set; }

    // Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
}
