using Microsoft.AspNetCore.Connections;
using WriteTogether.Dapper;
using WriteTogether.Features.Authorization;
using WriteTogether.Features.Register;
using WriteTogether.Features.Categories;
using WriteTogether.Features.Ratings;
using WriteTogether.Features.Fragments;
using WriteTogether.Features.Stories;
using WriteTogether.Features.StoriesAll;
using WriteTogether.Features.Tags;
using WriteTogether.Features.Users;
using WriteTogether.Features.StoriesMessages;
using WriteTogether.Features.ChatRooms;
using WriteTogether.Features.ChatMessages;

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

builder.Services.AddScoped<RatingsService>();
builder.Services.AddScoped<RatingsRepository>();

builder.Services.AddScoped<FragmentsService>();
builder.Services.AddScoped<FragmentsRepository>();

builder.Services.AddScoped<StoriesService>();
builder.Services.AddScoped<StoriesRepository>();

builder.Services.AddScoped<TagsService>();
builder.Services.AddScoped<TagsRepository>();

builder.Services.AddScoped<StoriesAllService>();
builder.Services.AddScoped<StoriesAllRepository>();

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<UsersRepository>();

builder.Services.AddScoped<StoriesMessagesService>();
builder.Services.AddScoped<StoriesMessagesRepository>();

builder.Services.AddScoped<ChatRoomsService>();
builder.Services.AddScoped<ChatRoomsRepository>();

builder.Services.AddScoped<ChatMessageRepository>();
builder.Services.AddScoped<ChatMessageRepository>();


builder.Services.AddSingleton<DbConnection>();   

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
