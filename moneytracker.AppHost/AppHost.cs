using Aspire.Hosting.ApplicationModel;
using Azure.Provisioning;
using Azure.Provisioning.AppContainers;
using Azure.Provisioning.Primitives;
using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var aca = builder.AddAzureContainerAppEnvironment("moneytracker")
    .WithDashboard(false)
    .ConfigureInfrastructure(infra =>
    {
        // Fixes so it doesn't try to select express
        var env = infra.GetProvisionableResources().OfType<ContainerAppManagedEnvironment>().Single();
        env.ResourceVersion = "2026-03-02-preview";
        var mode = new BicepValue<string>("WorkloadProfiles");
        ((IBicepValue)mode).Self = new BicepValueReference(env, "EnvironmentMode", ["properties", "environmentMode"]);
        env.ProvisionableProperties["EnvironmentMode"] = mode;
    });

var cache = builder.AddAzureManagedRedis("cache")
    .RunAsContainer();

var postgres = builder.AddAzurePostgresFlexibleServer("moneytrackerdb")
    .RunAsContainer(
        pg => pg.
        WithLifetime(ContainerLifetime.Persistent)
        .WithDataVolume()
        .WithPgAdmin());

var db = postgres.AddDatabase("serverdb");

var migrationService = builder.AddProject<Projects.moneytracker_MigrationService>("migrationservice")
    .WithReference(db)
    .WaitFor(db)
    .WithComputeEnvironment(aca);

var server = builder.AddProject<Projects.moneytracker_Server>("server")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(cache)
    .WaitFor(cache)
    .WaitFor(migrationService)
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints().WithComputeEnvironment(aca);

var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithPnpm()
    .WithReference(server)
    .WaitFor(server);

var scalar = builder.AddScalarApiReference()
    .ExcludeFromManifest();

scalar.WithApiReference(server);

server.PublishWithContainerFiles(webfrontend, "wwwroot");

builder.Build().Run();
