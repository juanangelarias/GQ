using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GQ.Database;

public class DataContextFactory: IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        // Build config
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..\\GQ.Api"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var optionBuilder = new DbContextOptionsBuilder<DataContext>();
        var connectionString = config.GetConnectionString(nameof(DataContext));
        optionBuilder.UseSqlite(connectionString!, b => b.MigrationsAssembly("GQ.Database"));

        // ToDo: Add Migration User Resolver
        return new DataContext(optionBuilder.Options);  //, new DateConverterService());
    }
}