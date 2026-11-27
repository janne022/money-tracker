using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using moneytracker.Server.Domain.Entities;
using moneytracker.Server.Infrastructure.Persistence;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

var frontendOrigin = builder.Configuration["WEBFRONTEND_HTTP"] ?? "";

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Host.UseWolverine(opts =>
{
    opts.UseRuntimeCompilation();
});

builder.AddAzureNpgsqlDbContext<AppDbContext>("serverdb");

builder.Services.AddOutputCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Frontend", configurePolicy: p =>
            {
                p.WithOrigins(frontendOrigin)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
});

builder.Services
    .AddIdentityApiEndpoints<Member>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Lockout.MaxFailedAccessAttempts = 3;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "AuthCookie";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

builder.Services.AddAuthorization();

builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseOutputCache();

app.MapDefaultEndpoints();

app.MapControllers();

app.UseFileServer();

var api = app.MapGroup("/api");

app.UseAuthentication();
app.UseAuthorization();
api.MapIdentityApi<Member>();

app.Run();