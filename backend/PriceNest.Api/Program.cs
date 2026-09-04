using Scalar.AspNetCore;
using PriceNest.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.SemanticKernel;
using PriceNest.Api.Plugins;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase"))
);
builder.Services.AddControllers();
builder.Services.AddScoped<PriceNest.Api.Services.ProductService>();
builder.Services.AddScoped<PriceNest.Api.Services.WatchListService>();
builder.Services.AddScoped<PriceNest.Api.Services.AuthService>();
builder.Services.AddScoped<PriceNest.Api.Services.UserService>();

var scraperUrl = builder.Configuration["SCRAPER_URL"] ?? "http://localhost:4000/";

builder.Services.AddHttpClient<PriceNest.Api.Services.ScraperService>(client =>
{
    client.BaseAddress = new Uri(scraperUrl);
});

var frontendUrl = builder.Configuration["FRONTEND_URL"] ?? "http://localhost:3000";

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendUrl).AllowCredentials()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});


builder.Services.AddScoped<PriceNest.Api.Plugins.ProductPlugin>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("access_token", out string? token))
            {
                context.Token = token; 
            }
            return Task.CompletedTask;
        }
    };
});

var apiKey = builder.Configuration["OpenAI:ApiKey"];
builder.Services.AddTransient(sp =>
{
    var builder = Kernel.CreateBuilder();

    builder.AddOpenAIChatCompletion(
        "gpt-4o-mini",
        apiKey ?? throw new Exception("Key missing")
        );
    builder.Plugins.AddFromObject(sp.GetRequiredService<ProductPlugin>());

    return builder.Build();
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => 
    {
        options.Authentication = null; 
    });
}

app.UseHttpsRedirection();

//CORS
app.UseCors();

app.UseMiddleware<PriceNest.Api.Middlewares.GlobalExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Ready");

app.MapGet("/test", () =>
{
    return new { message = "Testowy endpoint", status = "OK", value = 200 };
});

// app.MapPost("/product", (Product product) =>
// {
//     product.LastUpdated = DateTime.Now;
//     return Results.Created("Stworzono", product);
// });
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Próbuję wykonać migracje bazy danych...");
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
        logger.LogInformation("Migracje wykonane pomyślnie!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Wystąpił błąd podczas migracji bazy danych.");
    }
}

app.Run();
