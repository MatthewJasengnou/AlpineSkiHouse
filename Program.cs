using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

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

var app = builder.Build();

// In-memory storage for activities and votes
var activities = new List<Activity>
{
    new Activity(1, "Night Skiing", "Experience the slopes under the stars with our illuminated trails"),
    new Activity(2, "Ski Lessons", "Professional instructors for all skill levels"),
    new Activity(3, "Snow Tubing", "Family-friendly tubing lanes with lift access")
};

var votes = new Dictionary<string, Dictionary<int, bool>>();

// API endpoints for activities
app.MapGet("/api/activities", () => activities);
app.MapPost("/api/activities/{id}/vote", (int id, bool isLike, HttpRequest request) =>
{
    var activity = activities.FirstOrDefault(a => a.Id == id);
    if (activity == null) return Results.NotFound();
    
    var userId = request.Cookies["user_id"] ?? Guid.NewGuid().ToString();
    
    if (!votes.ContainsKey(userId)) votes[userId] = new Dictionary<int, bool>();
    if (votes[userId].ContainsKey(id)) return Results.BadRequest("Already voted");
    
    votes[userId][id] = isLike;
    activity.UpdateVotes(isLike); // Use a method to update votes to encapsulate this behavior.
    
    return Results.Ok(activity);
});

// Configure CORS
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Application configuration
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Existing endpoints
app.MapRazorPages();
app.MapGet("/hello", () => "Hello, AlpineSkiHouse!").WithOpenApi();

// Configuration for HTTP server
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");

// Record for activities
public record Activity(int Id, string Name, string Description)
{
    public int Likes { get; set; } = 0;
    public int Dislikes { get; set; } = 0;

    // Method to update likes or dislikes
    public void UpdateVotes(bool isLike)
    {
        if (isLike)
            Likes++;
        else
            Dislikes++;
    }
}

public class Item  // Existing class remains unchanged
{
    public int Id { get; set; }
    public string Name { get; set; }
}
