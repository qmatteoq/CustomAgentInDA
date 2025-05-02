using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<TravelAgency_API>("TravelAgencyApi");

builder.Build().Run();
