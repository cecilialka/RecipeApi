using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using RecipeApi.Interfaces;
using RecipeApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", 
    new OpenApiInfo {
        Title = "RecipeApi - V1",
        Version = "V1"
    });

    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.Configure<SpoonacularSettings>(builder.Configuration.GetSection("Spoonacular"));

builder.Services.AddHttpClient("spoonacular", (serviceProvider, client) => 
{
    var settings = serviceProvider
        .GetRequiredService<IOptions<SpoonacularSettings>>().Value;

     if (string.IsNullOrEmpty(settings.ApiKey))
    {
        throw new InvalidOperationException("API Key is not configured properly.");
    }

    client.BaseAddress = new Uri("https://api.spoonacular.com");
    client.DefaultRequestHeaders.Add("x-api-key", settings.ApiKey);
});

builder.Services.AddScoped<ISpoonacularService, SpoonacularService>();
builder.Services.AddScoped<IShoppingListService, ShoppingListService>();
builder.Services.AddScoped<INutritionService, NutritionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();


app.Run();

