using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.FeatureManagement;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using ODataDemo;
using ODataDemo.Extensions;
using ODataDemo.Models;
using ODataDemo.Serializers;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddFeatureManagement();

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EnableLowerCamelCase();
modelBuilder.EntityType<Order>();
modelBuilder.EntitySet<Customer>("Customers");

builder.Services.AddControllers().AddOData(options =>
    options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata",
        modelBuilder.GetEdmModel(),
        ConfigureODataResourceSerializer(builder.Configuration)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OData Demo", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OData Demo v1");
});

app.Run();

static Action<IServiceCollection> ConfigureODataResourceSerializer(IConfiguration configuration)
{
    return services =>
    {
        // Need to install add Configuration and install feature management for AddRouteComponents separately
        // https://stackoverflow.com/questions/59332463/unable-to-resolve-service-for-type-microsoft-extensions-configuration-iconfigura
        services.AddSingleton(configuration).AddFeatureManagement();

        services.AddSingleton(serviceProvider =>
        {
            var featureManager = serviceProvider.GetService<IFeatureManager>();
            Debug.Assert(featureManager != null);
            var odataSerializerProvider = serviceProvider.GetRequiredService<IODataSerializerProvider>();

            if (featureManager.IsEnabledAsync(FeatureFlags.OmitNull).GetAwaiter().GetResult())
            {
                return new OmitNullResourceSerializer(odataSerializerProvider);
            }
            else if (featureManager.IsEnabledAsync(FeatureFlags.ClientOmitNull).GetAwaiter().GetResult())
            {
                return new ClientOmitNullResourceSerializer(odataSerializerProvider);
            }
            else if (featureManager.IsEnabledAsync(FeatureFlags.OmitDefaultValue).GetAwaiter().GetResult())
            {
                return new OmitPropertyWithDefaultValueResourceSerializer(odataSerializerProvider, new Dictionary<string, object>
                {
                    { nameof(Customer.StringPropertyWithDefaultValueToBeOmitted).ToCamelCase(), string.Empty },
                    { nameof(Customer.BooleanPropertyWithDefaultValueToBeOmitted).ToCamelCase(), false }
                });
            }

            return new ODataResourceSerializer(odataSerializerProvider);
        });
    };
}

public partial class Program { }