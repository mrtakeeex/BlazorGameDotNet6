namespace BlazorGameDotNet6.Server.Data;

public class DataContext : DbContext
{
    public DbSet<Unit> Units { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserUnit> UserUnits { get; set; }
    public DbSet<Battle> Battles { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    // fluent api
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Battle>()
            .HasOne(b => b.Attacker)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Battle>()
            .HasOne(b => b.Opponent)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Battle>()
            .HasOne(b => b.Winner)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
