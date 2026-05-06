using Scalar.AspNetCore;
using PriceNest.Api.Models;
using PriceNest.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase"))
);
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseMiddleware<PriceNest.Api.Middlewares.GlobalExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello");

app.MapGet("/test", () => {
    return new { message = "Testowy endpoint", status = "OK", value = 200 };
});

// app.MapPost("/product", (Product product) =>
// {
//     product.LastUpdated = DateTime.Now;
//     return Results.Created("Stworzono", product);
// });

app.Run();
