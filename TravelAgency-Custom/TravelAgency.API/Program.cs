using Microsoft.SemanticKernel;
using Scalar.AspNetCore;
using TravelAgency.Shared.Agents;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddKernel();

builder.Services.AddAzureOpenAIChatCompletion(
       deploymentName: builder.Configuration.GetSection("AIServices:AzureOpenAI").GetValue<string>("DeploymentName"),
       endpoint: builder.Configuration.GetSection("AIServices:AzureOpenAI").GetValue<string>("Endpoint"),
       apiKey: builder.Configuration.GetSection("AIServices:AzureOpenAI").GetValue<string>("ApiKey"));

// Add CORS services and configure a policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddOpenTelemetry()
    .WithTracing(b => b.AddSource("*")
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation())
    .WithMetrics(b => b.AddMeter("*")
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation())
    .WithLogging()
    .UseOtlpExporter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


// Enable CORS middleware
app.UseCors();

app.MapGet("/getTravelPlan", async (string prompt, Kernel kernel) => {
    Console.WriteLine($"Received prompt: {prompt}"); // Log the prompt
    TravelAgent agent = new TravelAgent(kernel);
    var result = await agent.InvokeAgentAsync(prompt);
    Console.WriteLine($"The full response is: {result}"); // Log the full response
    return result;
})
.WithName("GetTravelPlan")
.WithDescription("Get a travel plan based on the provided prompt.");

app.Run();

