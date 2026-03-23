using Microsoft.AspNetCore.Connections;
using WriteTogether.Dapper;
using WriteTogether.Features.Authorization;
using WriteTogether.Features.Register;
using WriteTogether.Features.Categories;
using WriteTogether.Features.Profile;
using WriteTogether.Features.Ratings;
using WriteTogether.Features.Fragments;
using WriteTogether.Features.Stories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<AuthorizationRepository>();
builder.Services.AddScoped<AuthorizationService>();

builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<RegisterRepository>();

builder.Services.AddScoped<CategoriesService>();
builder.Services.AddScoped<CategoriesRepository>();

builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<ProfileRepository>();

builder.Services.AddScoped<RatingsService>();
builder.Services.AddScoped<RatingsRepository>();

builder.Services.AddScoped<FragmentsService>();
builder.Services.AddScoped<FragmentsRepository>();

builder.Services.AddScoped<StoriesService>();
builder.Services.AddScoped<StoriesRepository>();


builder.Services.AddSingleton<DbConnection>();   

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
