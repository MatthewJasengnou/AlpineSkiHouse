using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models; // Ensures that OpenApiInfo is recognized
using AlpineSkiHouse;


var builder = WebApplication.CreateBuilder(args);

// Add services for Razor pages and CORS
builder.Services.AddRazorPages();
builder.Services.AddCors();

// Configure Swagger to generate API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "AlpineSkiHouse API", 
        Version = "v1",
        Description = "API documentation for Alpine Ski House"
    });
});

// Add DbContext configuration
builder.Services.AddDbContext<AlpineSkiHouseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure CORS and other middleware
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapGet("/hello", () => "Hello, AlpineSkiHouse!");

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");
