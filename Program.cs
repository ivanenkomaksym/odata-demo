using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.ModelBuilder;
using ODataDemo.Models;
using ODataDemo.Serializers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntityType<Order>();
modelBuilder.EntitySet<Customer>("Customers");

builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata",
        modelBuilder.GetEdmModel(),
        // services => services.AddSingleton<ODataResourceSerializer, OmitNullResourceSerializer>()));
        services => services.AddSingleton<ODataResourceSerializer, OmitPropertyWithDefaultValueResourceSerializer>(serviceProvider =>
        {
            var odataSerializerProvider = serviceProvider.GetRequiredService<IODataSerializerProvider>();
            return new OmitPropertyWithDefaultValueResourceSerializer(odataSerializerProvider, new Dictionary<string, object> 
            {
                { nameof(Customer.StringPropertyWithDefaultValueToBeOmitted), string.Empty },
                { nameof(Customer.BooleanPropertyWithDefaultValueToBeOmitted), false }
            });
        })));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
