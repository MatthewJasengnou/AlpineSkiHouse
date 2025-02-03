using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Service configuration
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

// In-memory data store
var items = new List<Item>();

// Swagger configuration
bool enableSwagger = builder.Configuration.GetValue<bool>("EnableSwagger") || app.Environment.IsDevelopment();
if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlpineSkiHouse API v1");
        c.RoutePrefix = "swagger";
    });
}

// Pipeline configuration
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// API endpoints
app.MapGet("/hello", () => "Hello, AlpineSkiHouse!").WithOpenApi();

// CRUD Endpoints
// GET: Retrieve all items
app.MapGet("/items", () => items);

// GET: Retrieve a single item by ID
app.MapGet("/items/{id}", (int id) => 
    items.FirstOrDefault(i => i.Id == id) is Item item ? Results.Ok(item) : Results.NotFound());

// POST: Add a new item
app.MapPost("/items", (Item item) => {
    items.Add(item);
    return Results.Created($"/items/{item.Id}", item);
});

// PUT: Update an existing item
app.MapPut("/items/{id}", (int id, Item updatedItem) => {
    var index = items.FindIndex(i => i.Id == id);
    if (index == -1) return Results.NotFound();
    items[index] = updatedItem;
    return Results.NoContent();
});

// DELETE: Remove an item
app.MapDelete("/items/{id}", (int id) => {
    var index = items.FindIndex(i => i.Id == id);
    if (index == -1) return Results.NotFound();
    items.RemoveAt(index);
    return Results.NoContent();
});

// Port configuration
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");

// Define the Item model
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
}
