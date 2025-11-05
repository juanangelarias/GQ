using System.Net;
using GQ.Database;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace GQ.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File(@"C:\Logs\GQ\API_Log.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .CreateLogger();
        
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog();
        
        builder.WebHost.ConfigureKestrel(so =>
        {
            so.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
            so.Limits.MaxConcurrentConnections = 100;
            so.Limits.MaxConcurrentUpgradedConnections = 100;
            so.Limits.MaxRequestBodySize = 100_000_000;
            so.ListenAnyIP(5046);
            so.Listen(IPAddress.Loopback, 7004, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                listenOptions.UseHttps();
            });
        });
        
        builder.Services.AddDbContext<DataContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("BsgDbContext")));
            options.UseSqlite(builder.Configuration.GetConnectionString("DataContext")));
        
        // Add services to the container.
        Services.ConfigureServices(builder);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseDeveloperExceptionPage();
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI(opts =>
            {
                opts.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                opts.SwaggerEndpoint("/swagger/v1/swagger.json", "GQ Proof of Concept - API v1");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        //app.MapControllers();
        app.MapGraphQL("/graphql");

        Log.Information("GQ Proof of Concept - API v1 web host");
        
        app.Run();
    }
}