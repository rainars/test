using CardValidation.Core.Services;
using CardValidation.Core.Services.Interfaces;
using CardValidation.Infrustructure;

var builder = WebApplication.CreateBuilder(args);

// Bind to all network interfaces (important for Docker)
builder.WebHost.UseUrls("http://0.0.0.0:8080");

ConfigureServices(builder.Services);

var app = builder.Build();

// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Card Validation API v1"));

// Middleware configuration
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Register dependencies
    services.AddTransient<ICardValidationService, CardValidationService>();

    services.AddMvc(options =>
    {
        options.Filters.Add(typeof(CreditCardValidationFilter));
    });
}
