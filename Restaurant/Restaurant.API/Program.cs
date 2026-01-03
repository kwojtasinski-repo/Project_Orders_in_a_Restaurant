using Restaurant.API.Middleware;
using Restaurant.ApplicationLogic;
using Restaurant.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationLogic(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler();
}

app.Services.UseInfrastructure();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
