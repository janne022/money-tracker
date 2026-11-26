using Aspire.Hosting.ApplicationModel;
using Azure.Provisioning;
using Azure.Provisioning.AppContainers;
using Azure.Provisioning.Primitives;
using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

#pragma warning disable ASPIREACAEXPRESS001
var aca = builder.AddAzureContainerAppEnvironment("moneytracker")
    .WithDashboard(false)
    .AsExpress();
#pragma warning restore ASPIREACAEXPRESS001

var postgres = builder.AddAzurePostgresFlexibleServer("moneytrackerdb")
    .RunAsContainer(
        pg => pg.
        WithLifetime(ContainerLifetime.Persistent)
        .WithDataVolume()
        .WithPgAdmin());

var db = postgres.AddDatabase("serverdb");

var server = builder.AddDotnetProject("server", "../moneytracker.Server/moneytracker.Server.csproj")
    .WithReference(db)
    .WaitFor(db)
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WithComputeEnvironment(aca)
    .PublishAsAzureContainerApp((infrastructure, container) =>
    {
        container.Template.Scale.MinReplicas = 0;
        container.Template.Scale.MaxReplicas = 1;
    });

var migrations = server
    .AddEFMigrations("server-migrations")
    .WithReference(db)
    .WaitFor(db)
    .RunDatabaseUpdateOnStart()
    .PublishAsMigrationBundle(publishContainer: true)
    .PublishAsAzureContainerAppJob();

server.WaitForCompletion(migrations);

var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithPnpm()
    .WithReference(server)
    .WaitFor(server);

var scalar = builder.AddScalarApiReference()
    .ExcludeFromManifest();

scalar.WithApiReference(server);

server.PublishWithContainerFiles(webfrontend, "wwwroot");

builder.Build().Run();
