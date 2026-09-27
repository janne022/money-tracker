using moneytracker.MigrationService;
using moneytracker.Server.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDbContext<AppDbContext>("serverdb");

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();