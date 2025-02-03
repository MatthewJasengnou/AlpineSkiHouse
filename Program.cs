using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add these new services
builder.Services.AddRazorPages();
builder.Services.AddCors();

// Existing service configuration
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

// New in-memory storage for activities and votes
var activities = new List<Activity>
{
    new Activity(1, "Night Skiing", "Experience the slopes under the stars with our illuminated trails"),
    new Activity(2, "Ski Lessons", "Professional instructors for all skill levels"),
    new Activity(3, "Snow Tubing", "Family-friendly tubing lanes with lift access")
};

var votes = new Dictionary<string, Dictionary<int, bool>>();

// New API endpoints for activities
app.MapGet("/api/activities", () => activities);
app.MapPost("/api/activities/{id}/vote", (int id, bool isLike, HttpRequest request) =>
{
    var activity = activities.FirstOrDefault(a => a.Id == id);
    if (activity == null) return Results.NotFound();
    
    var userId = request.Cookies["user_id"] ?? Guid.NewGuid().ToString();
    
    if (!votes.ContainsKey(userId)) votes[userId] = new Dictionary<int, bool>();
    if (votes[userId].ContainsKey(id)) return Results.BadRequest("Already voted");
    
    votes[userId][id] = isLike;
    if (isLike) activity.Likes++;
    else activity.Dislikes++;
    
    return Results.Ok(activity);
});

// Existing items code remains unchanged
var items = new List<Item>();

// Configure CORS
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Existing pipeline configuration
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

// Existing CRUD endpoints remain unchanged
// [Keep all existing item endpoints here]

// Port configuration remains unchanged
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");

// New records
public record Activity(int Id, string Name, string Description)
{
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}

public class Item  // Existing class remains unchanged
{
    public int Id { get; set; }
    public string Name { get; set; }
}