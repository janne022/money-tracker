using Aspire.Hosting.ApplicationModel;
using Azure.Provisioning;
using Azure.Provisioning.AppContainers;
using Azure.Provisioning.Primitives;
using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

// Workaround for Windows / WSL2 / Docker Desktop issue where 'localhost' resolves to IPv6 [::1] first, Generated with AI
// which times out on container ports instead of falling back to IPv4 127.0.0.1 (see https://github.com/microsoft/aspire/issues/14769).
builder.Eventing.Subscribe<ResourceEndpointsAllocatedEvent>((@event, ct) =>
{
    foreach (var ep in @event.Resource.Annotations.OfType<EndpointAnnotation>())
    {
        if (ep.AllocatedEndpoint is { Address: "localhost" } alloc)
        {
            ep.AllocatedEndpoint = new AllocatedEndpoint(ep, "127.0.0.1", alloc.Port, alloc.BindingMode, alloc.TargetPortExpression, alloc.NetworkID);
        }
    }
    return Task.CompletedTask;
});

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

var server = builder.AddProject<Projects.moneytracker_Server>("server")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(cache)
    .WaitFor(cache)
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
