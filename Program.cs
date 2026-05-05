using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WriteTogether.Dapper;
using WriteTogether.Features.AiImages;
using WriteTogether.Features.AiText;
using WriteTogether.Features.Authorization;
using WriteTogether.Features.Categories;
using WriteTogether.Features.ChatMessages;
using WriteTogether.Features.ChatRooms;
using WriteTogether.Features.Fragments;
using WriteTogether.Features.LoreEntities;
using WriteTogether.Features.Ratings;
using WriteTogether.Features.Register;
using WriteTogether.Features.Stories;
using WriteTogether.Features.StoriesAll;
using WriteTogether.Features.StoriesCollaborators;
using WriteTogether.Features.StoriesMessages;
using WriteTogether.Features.Tags;
using WriteTogether.Features.Users;
using WriteTogether.Helpers.Cloudinary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://writetogether-app.onrender.com")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 🔐 Authentication (MOVER AQUÍ)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

// Services
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

builder.Services.AddScoped<ChatMessageService>(); 
builder.Services.AddScoped<ChatMessageRepository>();

builder.Services.AddScoped<StoriesCollaboratorsService>();
builder.Services.AddScoped<StoriesCollaboratorsRepository>();

builder.Services.AddHttpClient<AiImagesService>();
builder.Services.AddScoped<AiImagesRepository>();


builder.Services.AddScoped<LoreEntitiesService>();
builder.Services.AddScoped<LoreEntitiesRepository>();

builder.Services.AddHttpClient<AiTextService>();

builder.Services.AddScoped<DbConnection>();

builder.Services.AddSingleton(new Cloudinary(new Account(
    "ddvabh0dq",
    "965192327578266",
    "gHpYKnN3HRSQQn22LMhPVFNKTXQ"
)));

builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();