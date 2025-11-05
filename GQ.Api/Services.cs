using System.Text;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using GQ.Api.GraphQl;
using GQ.Database;
using GQ.Database.Mappings;
using GQ.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace GQ.Api;

public static class Services
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var config = builder.Configuration;

        services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();

        services
            .AddAutoMapper((sp, am) =>
            {
                am.AddCollectionMappers();
                am.UseEntityFrameworkCoreModel<DataContext>(sp);
                am.AddProfile(typeof(SqlMappingsProfile));
            }, typeof(DataContext).Assembly);
        
        services.AddHttpContextAccessor();

        services.AddGraphQLServer()
            .AddQueryType<Query>();
            //.AddMutationType<Mutation>();

        /*services.AddDbContext<DataContext>(opts => { opts.UseSqlServer(config.GetConnectionString("DataContext")); });*/

        #region Swagger

        //services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Description = "GQ Proof of Concept - API",
                Title = "GQ Proof of Concept - API",
                Version = "v1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization Header using Bearer scheme."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });

        #endregion
        
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        
        #region IOC container registrations
        
        services.AddAutoMapper((sp, am) =>
        {
            am.AddCollectionMappers();
            am.UseEntityFrameworkCoreModel<DataContext>(sp);
            am.AddProfile<SqlMappingsProfile>();
        }, typeof(DataContext).Assembly);
        
        // Repositories
        
        services
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<IProductPriceRepository, ProductPriceRepository>()
            .AddScoped<IProductTypeRepository, ProductTypeRepository>();
        
        #endregion
    }
}