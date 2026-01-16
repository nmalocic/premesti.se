using PremestiSe.Application;
using PremestiSe.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services from layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.Run();
