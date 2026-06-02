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

builder.Services.AddHttpClient<PriceNest.Api.Services.ScraperService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:3000/");
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
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
    app.MapScalarApiReference();
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

app.Run();
