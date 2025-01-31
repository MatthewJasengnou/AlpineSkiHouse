using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer(); // Needed for Swagger
builder.Services.AddSwaggerGen(); // Adding Swagger generation

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Retrieve the EnableSwagger flag from configuration or environment variable
bool enableSwagger = builder.Configuration.GetValue<bool>("EnableSwagger");

// Apply Swagger middleware based on the EnableSwagger flag or if in development environment
if (app.Environment.IsDevelopment() || enableSwagger)
{
    
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlpineSkiHouse v1"));

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Minimal API endpoint
app.MapGet("/hello", () => "Hello, AlpineSkiHouse!");

// Get the port from the environment variable (Render provides this automatically)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");
