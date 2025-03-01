namespace Linker.Cli.Integrations;

using Linker.Cli.Core;
using Microsoft.EntityFrameworkCore;
using Linker.Common.Helpers;

/// <summary>
/// The application's database context.
/// </summary>
public sealed class AppDbContext : DbContext
{
    private readonly string databaseConnectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// For Entity Framework use to scaffold the database.
    /// </summary>
    public AppDbContext()
        : this("Data Source=D:\\db.sqlite;")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="databaseConnectionString">The database connection string.</param>
    public AppDbContext(string databaseConnectionString)
    {
        this.databaseConnectionString = Guard.ThrowIfNullOrWhitespace(databaseConnectionString);
        this.Database.EnsureCreated();
    }

    /// <summary>
    /// Gets or sets the database set for link.
    /// </summary>
    public DbSet<Link> Links { get; set; }

    /// <summary>
    /// Gets or sets the database set for visit.
    /// </summary>
    public DbSet<Visit> Visits { get; set; }

    /// <summary>
    /// Gets or sets the database set for url list.
    /// </summary>
    public DbSet<UrlList> Lists { get; set; }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(this.databaseConnectionString);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Link>()
            .HasMany(l => l.Lists)
            .WithMany(t => t.Links)
            .UsingEntity<Dictionary<string, object>>(
                "LinkUrlList",  // Name of the join table
                l => l.HasOne<UrlList>().WithMany().HasForeignKey("UrlListId"),
                t => t.HasOne<Link>().WithMany().HasForeignKey("LinkId"));

        modelBuilder.Entity<Link>()
            .HasIndex(x => x.Url)
            .IsUnique();

        modelBuilder.Entity<Visit>()
            .HasOne(v => v.Link)
            .WithMany(l => l.Visits)
            .HasForeignKey(v => v.LinkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
